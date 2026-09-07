using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using Tarmac64_Library;
using System.Windows.Media.Media3D;
using Assimp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Tarmac64_Library
{
    public class TM64_GL
    {



        public class TMCamera
        {
            public Assimp.Vector3D position { get; set; }
            public float TargetHeight { get; set; }
            public Assimp.Vector3D target { get; set; }
            public Assimp.Vector3D marker { get; set; }
            public double[] rotation { get; set; }
            public float[] flashRed { get; set; }
            public float[] flashWhite { get; set; }
            public float[] flashYellow { get; set; }
            public TM64_Geometry.Face[] Cursor { get; set; }

            public TMCamera()
            {
                rotation = new double[2];
            }
        }

        public class OKObjectTextureCache
        {
            private readonly Dictionary<string, Texture> cachedTextures = new Dictionary<string, Texture>(StringComparer.OrdinalIgnoreCase);
            private Texture missingTexture;

            public static TM64_Texture.OK64TexelData GetFirstTexel(TM64_Course.OKObjectType objectType)
            {
                if (objectType == null || objectType.TextureData == null || objectType.TextureData.Length == 0)
                {
                    return null;
                }
                if (objectType.TextureData[0] == null || objectType.TextureData[0].TexelData == null || objectType.TextureData[0].TexelData.Count == 0)
                {
                    return null;
                }
                return objectType.TextureData[0].TexelData[0];
            }

            public Texture Get(OpenGL gl, TM64_Course.OKObjectType objectType)
            {
                TM64_Texture.OK64TexelData texel = GetFirstTexel(objectType);
                if (texel == null || string.IsNullOrEmpty(texel.texturePath) || !File.Exists(texel.texturePath))
                {
                    return GetMissing(gl);
                }

                Texture texture;
                if (cachedTextures.TryGetValue(texel.texturePath, out texture) && texture != null)
                {
                    return texture;
                }

                texture = new Texture();
                try
                {
                    texture.Create(gl, texel.texturePath);
                }
                catch
                {
                    texture.Destroy(gl);
                    return GetMissing(gl);
                }

                cachedTextures[texel.texturePath] = texture;
                return texture;
            }

            public void Bind(OpenGL gl, TM64_Course.OKObjectType objectType)
            {
                Texture texture = Get(gl, objectType);
                texture.Bind(gl);

                TM64_Texture.OK64TexelData texel = GetFirstTexel(objectType);
                if (texel == null)
                {
                    return;
                }

                uint[] WrapTypes = { OpenGL.GL_REPEAT, OpenGL.GL_REPEAT, OpenGL.GL_MIRRORED_REPEAT, OpenGL.GL_CLAMP_TO_EDGE, OpenGL.GL_MIRRORED_REPEAT };
                gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[texel.SFlag] });
                gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, new uint[] { WrapTypes[texel.TFlag] });
            }

            public void Sync(OpenGL gl, IEnumerable<TM64_Course.OKObjectType> objectTypes)
            {
                HashSet<string> livePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (objectTypes != null)
                {
                    foreach (TM64_Course.OKObjectType objectType in objectTypes)
                    {
                        TM64_Texture.OK64TexelData texel = GetFirstTexel(objectType);
                        if (texel == null || string.IsNullOrEmpty(texel.texturePath) || !File.Exists(texel.texturePath))
                        {
                            continue;
                        }

                        livePaths.Add(texel.texturePath);
                        Get(gl, objectType);
                    }
                }

                List<string> evict = new List<string>();
                foreach (KeyValuePair<string, Texture> entry in cachedTextures)
                {
                    if (!livePaths.Contains(entry.Key))
                    {
                        evict.Add(entry.Key);
                    }
                }

                foreach (string path in evict)
                {
                    cachedTextures[path].Destroy(gl);
                    cachedTextures.Remove(path);
                }
            }

            public void Clear(OpenGL gl)
            {
                foreach (Texture texture in cachedTextures.Values)
                {
                    texture.Destroy(gl);
                }
                cachedTextures.Clear();

                if (missingTexture != null)
                {
                    missingTexture.Destroy(gl);
                    missingTexture = null;
                }
            }

            private Texture GetMissing(OpenGL gl)
            {
                if (missingTexture == null)
                {
                    missingTexture = new Texture();
                    missingTexture.Create(gl, Tarmac64_Library.Properties.Resources.TextureNotFound);
                }
                return missingTexture;
            }
        }

        bool primitiveActive;

        public void EndPrimitive(OpenGL gl)
        {
            if (primitiveActive)
            {
                gl.End();
                primitiveActive = false;
            }
        }

        public void BeginTriangles(OpenGL gl)
        {
            EndPrimitive(gl);
            gl.Begin(OpenGL.GL_TRIANGLES);
            primitiveActive = true;
        }




        public void DrawFace(OpenGL gl, TM64_Geometry.Face subFace, double ShiftS = 0, double ShiftT = 0)
        {
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.RFloat, subVert.color.GFloat, subVert.color.BFloat, subVert.color.AFloat);
                gl.TexCoord(subVert.position.u + ShiftS, subVert.position.v + ShiftT);
                gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
            }
        }

        public void DrawMarker( OpenGL gl, Texture glTexture, TM64_Geometry.Face subFace, float[] Color, TM64_Paths.Marker Point)
        {
            
            float[] ThisColor = new float[4];
            if (Color.Length == 3)
            {
                ThisColor = new float[4] { Color[0], Color[1], Color[2], Convert.ToSingle(1.0) };
            }
            else
            {
                ThisColor = Color;
            }
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(ThisColor[0], ThisColor[1], ThisColor[2], ThisColor[3]);
                gl.Vertex(subVert.position.x + Point.X, subVert.position.y+ Point.Y, subVert.position.z+ Point.Z);
            }
        }
        public float[] GetAlphaFlash(float[] flashColor, double FrameRate)
        {
            
            if (flashColor[4] == 0.0f)
            {
                if (flashColor[3] < 1.0f)
                {
                    flashColor[3] = Convert.ToSingle(flashColor[3] + (FrameRate * 0.1));
                }
                else
                {
                    flashColor[4] = 1.0f;
                }
            }
            else
            {
                if (flashColor[3] > 0.0)
                {
                    flashColor[3] = Convert.ToSingle(flashColor[3] - (FrameRate * 0.1));
                }
                else
                {
                    flashColor[4] = 0.0f;
                }
            }


            if (flashColor[3] > 1.0f)
            {
                flashColor[3] = 1.0f;
                flashColor[4] = 1.0f;
            }
            if (flashColor[3] < 0.0f)
            {
                flashColor[3] = 0.0f;
                flashColor[4] = 0.0f;
            }



            float[] outputColor = { flashColor[0], flashColor[1], flashColor[2], flashColor[3], flashColor[4] };
            return outputColor;
        }

        public Point3D RotatePoint(Point3D Point, float[] ObjectAngles)
        {
            var id = Matrix3D.Identity;
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), ObjectAngles[0]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), ObjectAngles[1]));
            id.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), ObjectAngles[2]));
            return id.Transform(Point);
        }

        private void DrawFace(OpenGL gl, TM64_Geometry.Face subFace, int[] Zone)
        {
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.RFloat, subVert.color.GFloat, subVert.color.BFloat, 1.0f);
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x + (Zone[0] * 500), subVert.position.y + (Zone[1] * 500), subVert.position.z + (Zone[2] * 250));
            }
        }

        public void DrawNorth(OpenGL gl, Texture glTexture, TMCamera LocalCamera)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            BeginTriangles(gl);

            
            float[] targetPosition = new float[3] { Convert.ToSingle(LocalCamera.target.X), Convert.ToSingle(LocalCamera.target.Y), Convert.ToSingle(LocalCamera.target.Z + 60) };

            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(targetPosition[0] + 0.0f, targetPosition[1] + 2.0f, targetPosition[2] + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + -2.0f, targetPosition[1] + -2.0f, targetPosition[2] + 2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + 2.0f, targetPosition[1] + -2.0f, targetPosition[2] + 2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(targetPosition[0] + 0.0f, targetPosition[1] + 2.0f, targetPosition[2] + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + 2.0f, targetPosition[1] + -2.0f, targetPosition[2] + 2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + 2.0f, targetPosition[1] + -2.0f, targetPosition[2] + -2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(targetPosition[0] + 0.0f, targetPosition[1] + 2.0f, targetPosition[2] + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + 2.0f, targetPosition[1] + -2.0f, targetPosition[2] + -2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + -2.0f, targetPosition[1] + -2.0f, targetPosition[2] + -2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(targetPosition[0] + 0.0f, targetPosition[1] + 2.0f, targetPosition[2] + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + -2.0f, targetPosition[1] + -2.0f, targetPosition[2] + -2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(targetPosition[0] + -2.0f, targetPosition[1] + -2.0f, targetPosition[2] + 2.0f);

            EndPrimitive(gl);
        }

        public void DrawSection(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            DrawShaded(gl, targetObject, LocalCamera.flashRed);
        }


        public void DrawOKObjectShaded(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType, float[] ObjectColor)
        {
            BeginTriangles(gl);
            foreach (var ThisGeometry in TargetObjectType.ModelData)
            {
                foreach (var Face in ThisGeometry.modelGeometry)
                {
                    if (ObjectColor.Length > 3)
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            Point3D VertexPoint = new Point3D() { X = subVert.position.x, Y = subVert.position.y, Z = subVert.position.z };
                            float[] ObjectAngle = new float[3] { TargetObject.OriginAngle[0], TargetObject.OriginAngle[1], TargetObject.OriginAngle[2] };
                            Point3D ThreeDPoint = RotatePoint(VertexPoint, ObjectAngle);

                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], ObjectColor[3]);
                            gl.Vertex((ThreeDPoint.X * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (ThreeDPoint.Y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (ThreeDPoint.Z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                    else
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            Point3D VertexPoint = new Point3D() { X = subVert.position.x, Y = subVert.position.y, Z = subVert.position.z };
                            float[] ObjectAngle = new float[3] { TargetObject.OriginAngle[0], TargetObject.OriginAngle[1], TargetObject.OriginAngle[2] };
                            Point3D ThreeDPoint = RotatePoint(VertexPoint, ObjectAngle);

                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], 1.0f);
                            gl.Vertex((ThreeDPoint.X * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (ThreeDPoint.Y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (ThreeDPoint.Z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                }

            }
            EndPrimitive(gl);
        }

        public void DrawOKObjectShaded(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType)
        {
            BeginTriangles(gl);
            foreach (var ThisGeometry in TargetObjectType.ModelData)
            {
                float[] ObjectColor = ThisGeometry.objectColor;
                foreach (var Face in ThisGeometry.modelGeometry)
                {
                    if (ObjectColor.Length > 3)
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            Point3D VertexPoint = new Point3D() { X = subVert.position.x, Y = subVert.position.y, Z = subVert.position.z };
                            float[] ObjectAngle = new float[3] { TargetObject.OriginAngle[0], TargetObject.OriginAngle[1], TargetObject.OriginAngle[2] };
                            Point3D ThreeDPoint = RotatePoint(VertexPoint, ObjectAngle);

                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], ObjectColor[3]);
                            gl.Vertex((ThreeDPoint.X * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (ThreeDPoint.Y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (ThreeDPoint.Z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                    else
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            Point3D VertexPoint = new Point3D() { X = subVert.position.x, Y = subVert.position.y, Z = subVert.position.z };
                            float[] ObjectAngle = new float[3] { TargetObject.OriginAngle[0], TargetObject.OriginAngle[1], TargetObject.OriginAngle[2] };
                            Point3D ThreeDPoint = RotatePoint(VertexPoint, ObjectAngle);

                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], 1.0f);
                            gl.Vertex((ThreeDPoint.X * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (ThreeDPoint.Y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (ThreeDPoint.Z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                }
            }
            EndPrimitive(gl);
        }


        public void DrawOKObjectTextured(OpenGL gl, OKObjectTextureCache textureCache, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType)
        {
            EndPrimitive(gl);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            textureCache.Bind(gl, TargetObjectType);
            BeginTriangles(gl);
            foreach (var Geometry in TargetObjectType.ModelData)
            {
                foreach (var Face in Geometry.modelGeometry)
                {
                    foreach (var subVert in Face.VertData)
                    {
                        Point3D VertexPoint = new Point3D() { X = subVert.position.x, Y = subVert.position.y, Z = subVert.position.z };
                        float[] ObjectAngle = new float[3] { TargetObject.OriginAngle[0], TargetObject.OriginAngle[1], TargetObject.OriginAngle[2] };
                        Point3D ThreeDPoint = RotatePoint(VertexPoint, ObjectAngle);

                        gl.Color(subVert.color.RFloat, subVert.color.GFloat, subVert.color.BFloat, 1.0f);
                        gl.TexCoord(subVert.position.u, subVert.position.v);
                        gl.Vertex((ThreeDPoint.X * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (ThreeDPoint.Y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (ThreeDPoint.Z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                    }
                }
            }
            EndPrimitive(gl);
        }


        public void DrawShaded(OpenGL gl, TM64_Geometry.OK64F3DObject TargetObject, float[] colorArray)
        {
            BeginTriangles(gl);
            foreach (var subFace in TargetObject.modelGeometry)
            {
                if (colorArray.Length > 3)
                {
                    foreach (var subVert in subFace.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                        gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                    }
                }
                else
                {
                    foreach (var subVert in subFace.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], 1.0f);
                        gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                    }
                }
            }
            EndPrimitive(gl);
        }

        public void DrawGouraud(OpenGL gl, TM64_Geometry.OK64F3DObject TargetObject)
        {
            BeginTriangles(gl);
            foreach (var subFace in TargetObject.modelGeometry)
            {
                foreach (var subVert in subFace.VertData)
                {
                    gl.Color(subVert.color.RFloat, subVert.color.GFloat, subVert.color.BFloat, subVert.color.AFloat);
                    gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                }
            }
            EndPrimitive(gl);
        }


        public void DrawGouraudObjectColor(OpenGL gl, TM64_Geometry.OK64F3DObject TargetObject)
        {
            BeginTriangles(gl);
            foreach (var subFace in TargetObject.modelGeometry)
            {
                foreach (var subVert in subFace.VertData)
                {
                    gl.Color(TargetObject.objectColor[0], TargetObject.objectColor[1], TargetObject.objectColor[2], 1.0f);
                    gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                }
            }
            EndPrimitive(gl);
        }

        public void DrawTexturedTexturedNoFlush(OpenGL gL, TM64_Texture.OK64Texture oK64Texture, TM64_Geometry.OK64F3DObject oK64F3DObject)
        {
            throw new NotImplementedException();
        }

        public void DrawShaded(OpenGL gl, Texture glTexture, TM64_Geometry.OK64F3DObject TargetObject, float[] colorArray, int[] Zone)
        {
            EndPrimitive(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            glTexture.Destroy(gl);
            BeginTriangles(gl);
            foreach (var subFace in TargetObject.modelGeometry)
            {
                if (colorArray.Length > 3)
                {
                    foreach (var subVert in subFace.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                        gl.Vertex(subVert.position.x + (Zone[0] * 500), subVert.position.y + (Zone[1] * 500), subVert.position.z + (Zone[2] * 250));
                    }
                }
                else
                {
                    foreach (var subVert in subFace.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], 1.0f);
                        gl.Vertex(subVert.position.x + (Zone[0] * 500), subVert.position.y + (Zone[1] * 500), subVert.position.z + (Zone[2] * 250));
                    }
                }
            }
            EndPrimitive(gl);
        }


        uint[] WrapTypes = new uint[]
        {
                OpenGL.GL_REPEAT,
                OpenGL.GL_REPEAT,
                OpenGL.GL_MIRRORED_REPEAT,
                OpenGL.GL_CLAMP,
                OpenGL.GL_REPEAT,
        };

        public void DrawGLCull(OpenGL GL, TM64_Texture.OK64Texture TextureObject)
        {
            bool Enable = false;

            if (TextureObject.ColorCombine.GeometryBools[5])
            {
                Enable = true;

                GL.CullFace(OpenGL.GL_FRONT_AND_BACK);
                
            }
            else if (TextureObject.ColorCombine.GeometryBools[3])
            {
                Enable = true;

                if (TextureObject.ColorCombine.GeometryBools[4])
                {
                    GL.CullFace(OpenGL.GL_FRONT_AND_BACK);
                    
                }
                else
                {
                    GL.CullFace(OpenGL.GL_FRONT);
                }
            }
            else if (TextureObject.ColorCombine.GeometryBools[4])
            {
                Enable = true;

                GL.CullFace(OpenGL.GL_BACK);
                
            }
            if (Enable)
            {
                GL.Enable(OpenGL.GL_CULL_FACE);
            }
            else
            {
                GL.Disable(OpenGL.GL_CULL_FACE);
            }
        }


        public void DrawTextureFlush(OpenGL gl, TM64_Texture.OK64Texture[] textureArray, Texture glTexture, int TargetID)
        {
            EndPrimitive(gl);

            uint[] WrapTypes = { OpenGL.GL_REPEAT, OpenGL.GL_REPEAT, OpenGL.GL_MIRRORED_REPEAT, OpenGL.GL_CLAMP_TO_EDGE, OpenGL.GL_MIRRORED_REPEAT };

            glTexture.Bind(gl);

            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[textureArray[TargetID].TexelData[0].SFlag] });
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, new uint[] { WrapTypes[textureArray[TargetID].TexelData[0].TFlag] });

            BeginTriangles(gl);
        }
        public void DrawTextureFlushScreen(OpenGL gl, int Width, int Height, TM64_Texture.OK64Texture TextureObject, Texture glTexture)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);

            glTexture.Create(gl, RenderScreen(gl, TextureObject.TexelData[0].textureScreen - 1, Width, Height));
            glTexture.Bind(gl);

            uint[] WrapTypes = { OpenGL.GL_REPEAT, OpenGL.GL_REPEAT, OpenGL.GL_MIRRORED_REPEAT, OpenGL.GL_CLAMP_TO_EDGE, OpenGL.GL_MIRRORED_REPEAT };

            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[TextureObject.TexelData[0].SFlag] });
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[TextureObject.TexelData[0].SFlag] });
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, new uint[] { WrapTypes[TextureObject.TexelData[0].TFlag] });

            BeginTriangles(gl);
        }
        public void DrawTexturedNoFlush(OpenGL gl, TM64_Texture.OK64Texture TextureObject, TM64_Geometry.OK64F3DObject targetObject)
        {
            foreach (var subFace in targetObject.modelGeometry)
            {
                DrawFace(gl, subFace, TextureObject.GLShiftS, TextureObject.GLShiftT);
            }
        }

        public void UpdateTarget(TMCamera LocalCamera)
        {
            
            float[] localCoord = new float[3];
            localCoord[0] = LocalCamera.position.X + Convert.ToSingle(Math.Cos(LocalCamera.rotation[1]) * Math.Cos(LocalCamera.rotation[0])) * 15000;
            localCoord[1] = LocalCamera.position.Y + Convert.ToSingle(Math.Cos(LocalCamera.rotation[1]) * Math.Sin(LocalCamera.rotation[0])) * 15000;
            localCoord[2] = LocalCamera.position.Z + Convert.ToSingle(Math.Sin(LocalCamera.rotation[1])) * 15000;
            
            LocalCamera.target = new Assimp.Vector3D(localCoord[0], localCoord[1], localCoord[2]);
        }


        public void ZoomCameraTarget(float[] TargetPosition, TMCamera LocalCamera)
        {
            LocalCamera.position = new Assimp.Vector3D(TargetPosition[0] + 50, TargetPosition[1], TargetPosition[2] + 10);
            LocalCamera.rotation[0] = 180 * (Math.PI / 180);
            UpdateTarget(LocalCamera);
        }

        public void MoveCamera(int direction, TMCamera LocalCamera, float moveDistance)
        {
            float[] localCoord = new float[3];
            switch (direction)
            {
                case 0:
                    {
                        //forward


                        localCoord[0] = LocalCamera.position.X + Convert.ToSingle(Math.Cos(LocalCamera.rotation[1]) * Math.Cos(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[1] = LocalCamera.position.Y + Convert.ToSingle(Math.Cos(LocalCamera.rotation[1]) * Math.Sin(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[2] = LocalCamera.position.Z + Convert.ToSingle(Math.Sin(LocalCamera.rotation[1])) * moveDistance;

                        /*
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X + moveDistance * Math.Cos(LocalCamera.rotation[0]));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + moveDistance * Math.Sin(LocalCamera.rotation[0]));
                        localCoord[2] = Convert.ToSingle(LocalCamera.position.Z);
                        */


                        break;
                    }
                case 1:
                    {
                        //back
                        localCoord[0] = LocalCamera.position.X - Convert.ToSingle(Math.Cos(LocalCamera.rotation[1]) * Math.Cos(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[1] = LocalCamera.position.Y - Convert.ToSingle(Math.Cos(LocalCamera.rotation[1]) * Math.Sin(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[2] = LocalCamera.position.Z - Convert.ToSingle(Math.Sin(LocalCamera.rotation[1])) * moveDistance;

                        /*
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X - moveDistance * Math.Cos(LocalCamera.rotation[0]));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y - moveDistance * Math.Sin(LocalCamera.rotation[0]));
                        localCoord[2] = Convert.ToSingle(LocalCamera.position.Z);
                        */
                        break;
                    }
                case 2:
                    {
                        //up
                        float strafeAngle = Convert.ToSingle(LocalCamera.rotation[1] + (90.0f * (Math.PI / 180.0f)));
                        if (strafeAngle < 0)
                            strafeAngle += Convert.ToSingle(Math.PI * 2.0f);

                        localCoord[0] = LocalCamera.position.X + Convert.ToSingle(Math.Cos(strafeAngle) * Math.Cos(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[1] = LocalCamera.position.Y + Convert.ToSingle(Math.Cos(strafeAngle) * Math.Sin(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[2] = LocalCamera.position.Z + Convert.ToSingle(Math.Sin(strafeAngle)) * moveDistance;

                        break;
                    }
                case 3:
                    {
                        //down

                        float strafeAngle = Convert.ToSingle(LocalCamera.rotation[1] - (90.0f * (Math.PI / 180.0f)));
                        if (strafeAngle < 0)
                            strafeAngle += Convert.ToSingle(Math.PI * 2.0f);

                        localCoord[0] = LocalCamera.position.X + Convert.ToSingle(Math.Cos(strafeAngle) * Math.Cos(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[1] = LocalCamera.position.Y + Convert.ToSingle(Math.Cos(strafeAngle) * Math.Sin(LocalCamera.rotation[0])) * moveDistance;
                        localCoord[2] = LocalCamera.position.Z + Convert.ToSingle(Math.Sin(strafeAngle)) * moveDistance;


                        break;
                    }
                case 4:
                    {
                        //strafe
                        float strafeAngle = Convert.ToSingle(LocalCamera.rotation[0] - (90.0f * (Math.PI / 180.0f)) );
                        if (strafeAngle < 0)
                            strafeAngle += Convert.ToSingle(Math.PI * 2.0f);
                        
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X + moveDistance * Math.Cos(strafeAngle));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + moveDistance * Math.Sin(strafeAngle));
                        localCoord[2] = Convert.ToSingle(LocalCamera.position.Z);
                        

                        break;
                    }
                case 5:
                    {
                        //strafe
                        float strafeAngle = Convert.ToSingle(LocalCamera.rotation[0] + (90.0f * (Math.PI / 180.0f)));
                        if (strafeAngle < 0)
                            strafeAngle += Convert.ToSingle(Math.PI * 2.0f);

                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X + moveDistance * Math.Cos(strafeAngle));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + moveDistance * Math.Sin(strafeAngle));
                        localCoord[2] = Convert.ToSingle(LocalCamera.position.Z);
                        
                        break;
                    }
            }


            LocalCamera.position = new Assimp.Vector3D(localCoord[0], localCoord[1], localCoord[2]);
            UpdateTarget(LocalCamera);

        }

        public void DrawTextured(OpenGL gl, TM64_Texture.OK64Texture[] textureArray, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            if (textureArray[targetObject.materialID].TexelData[0].texturePath == null)
            {
                MessageBox.Show("Error loading texture for " + targetObject.objectName);
            }
            glTexture.Create(gl, textureArray[targetObject.materialID].TexelData[0].texturePath);
            glTexture.Bind(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            BeginTriangles(gl);
            foreach (var subFace in targetObject.modelGeometry)
            {
                DrawFace(gl, subFace);
            }
            EndPrimitive(gl);
        }

        public void DrawTextured(OpenGL gl, TM64_Texture.OK64Texture[] textureArray, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            if (textureArray[targetObject.materialID].TexelData[0].texturePath == null)
            {
                MessageBox.Show("Error loading texture for " + targetObject.objectName);
            }
            glTexture.Create(gl, textureArray[targetObject.materialID].TexelData[0].texturePath);
            glTexture.Bind(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            BeginTriangles(gl);
            foreach (var subFace in targetObject.modelGeometry)
            {
                DrawFace(gl, subFace, Zone);
            }
            EndPrimitive(gl);
        }


        public void DrawCursor(OpenGL gl, TMCamera LocalCamera, Texture glTexture)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            BeginTriangles(gl);

            foreach (var Face in LocalCamera.Cursor)
            {
                foreach (var subVert in Face.VertData)
                {
                    gl.Color(1.0f, 0.5f, 0f, 1.0f);
                    gl.Vertex(subVert.position.x + LocalCamera.marker.X, subVert.position.y + LocalCamera.marker.Y, subVert.position.z + LocalCamera.marker.Z);
                }
            }

            EndPrimitive(gl);
        }
        public void DrawTarget(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);

            DrawShaded(gl, targetObject, LocalCamera.flashWhite);
        }





        public void DrawTarget(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            EndPrimitive(gl);
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);

            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashWhite, Zone);
        }


        public void DrawWire(OpenGL gl, TM64_Geometry.OK64F3DObject targetObject)
        {
            EndPrimitive(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            BeginTriangles(gl);
            foreach (var subFace in targetObject.modelGeometry)
            {
                var colorArray = targetObject.objectColor;
                if (colorArray.Length > 3)
                {
                    foreach (var subVert in subFace.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], colorArray[3]);
                        gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                    }
                }
                else
                {
                    foreach (var subVert in subFace.VertData)
                    {
                        gl.Color(colorArray[0], colorArray[1], colorArray[2], 1.0f);
                        gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                    }
                }
            }
            EndPrimitive(gl);
        }



        public Bitmap Save2Picture(OpenGL GL, int x, int y, int width, int height)

        {

            var format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

            var lockMode = System.Drawing.Imaging.ImageLockMode.WriteOnly;

            var bitmap = new Bitmap(width, height, format);

            var bitmapRect = new Rectangle(new Point { X = x, Y = y }, new Size { Width = width, Height = height });

            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(bitmapRect, lockMode, format);

            GL.ReadPixels(x, y, width, height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, bmpData.Scan0);

            bitmap.UnlockBits(bmpData);

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            return bitmap;

        }

        double[][] ScreenPointRatios = new double[][]{

            new double[]{ .10, .80 },
            new double[]{ .50, .80 },
            new double[]{ .10, .60 },
            new double[]{ .50, .60 },
            new double[]{ .10, .40 },
            new double[]{ .50, .40 }
            
        };
        public Bitmap RenderScreen(OpenGL GL, int ScreenIndex, int width, int height)
        {
            var format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;

            var lockMode = System.Drawing.Imaging.ImageLockMode.WriteOnly;

            


            int ScreenWidth = Convert.ToInt32(width * 0.40);
            int ScreenHeight = Convert.ToInt32(height * 0.20);

            var bitmap = new Bitmap(ScreenWidth, ScreenHeight, format);
            var bitmapRect = new Rectangle(new Point { X = 0, Y = 0 }, new Size { Width = ScreenWidth, Height = ScreenHeight });

            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(bitmapRect, lockMode, format);
            
            GL.ReadPixels(Convert.ToInt32(width * ScreenPointRatios[ScreenIndex][0]), Convert.ToInt32(height * ScreenPointRatios[ScreenIndex][1]), ScreenWidth, ScreenHeight, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, bmpData.Scan0);

            bitmap.UnlockBits(bmpData);

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            Bitmap Resized = new Bitmap(bitmap, new Size(width = 64, height = 32));
            

            return Resized;
        }

    }

}

