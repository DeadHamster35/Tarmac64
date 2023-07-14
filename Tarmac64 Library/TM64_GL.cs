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
            gl.Begin(OpenGL.GL_TRIANGLES);
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
                gl.Vertex(subVert.position.x + Point.xval, subVert.position.y+ Point.yval, subVert.position.z+ Point.zval);
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
                gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, 1.0f);
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x + (Zone[0] * 500), subVert.position.y + (Zone[1] * 500), subVert.position.z + (Zone[2] * 250));
            }
        }

        public void DrawNorth(OpenGL gl, Texture glTexture, TMCamera LocalCamera)
        {
            glTexture.Destroy(gl);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);

            
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
            
        }

        public void DrawSection(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashRed);
            
            
        }


        public void DrawOKObjectShaded(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType, float[] ObjectColor)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
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
        }

        public void DrawOKObjectShaded(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
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
        }


        public void DrawOKObjectTextured(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType)
        {
            OpenFileDialog FileReplace = new OpenFileDialog();
            foreach (var Geometry in TargetObjectType.ModelData)
            {
                
                glTexture.Destroy(gl);
                if (TargetObjectType.TextureData[Geometry.materialID].texturePath == null)
                {
                    MessageBox.Show("Error loading texture for " + TargetObjectType.Name);
                    if (FileReplace.ShowDialog == DialogResult.OK)
                    {
                        if (FileReplace.FileName != null)
                        {
                            if(File.Exists(FileReplace.FileName))
                            {
                                TargetObjectType.TextureData[Geometry.materialID].texturePath = FileReplace.FileName;
                            }
                            else
                            {
                                TargetObjectType.TextureData[Geometry.materialID].texturePath = 
                            }
                        }
                    }
                }
                if (File.Exists(TargetObjectType.TextureData[Geometry.materialID].texturePath)) ;
                {

                }

                glTexture.Create(gl, TargetObjectType.TextureData[Geometry.materialID].texturePath);
                glTexture.Bind(gl);

                gl.Begin(OpenGL.GL_TRIANGLES);
                foreach (var Face in Geometry.modelGeometry)
                {                    
                    foreach (var subVert in Face.VertData)
                    {
                        Point3D VertexPoint = new Point3D() { X = subVert.position.x, Y = subVert.position.y, Z = subVert.position.z };
                        float[] ObjectAngle = new float[3] { TargetObject.OriginAngle[0], TargetObject.OriginAngle[1], TargetObject.OriginAngle[2] };
                        Point3D ThreeDPoint = RotatePoint(VertexPoint, ObjectAngle);

                        gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, 1.0f);
                        gl.TexCoord(subVert.position.u, subVert.position.v);
                        gl.Vertex((ThreeDPoint.X * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (ThreeDPoint.Y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (ThreeDPoint.Z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                    }                    
                }

            }
            
        }


        public void DrawShaded(OpenGL gl, Texture glTexture, TM64_Geometry.OK64F3DObject TargetObject, float[] colorArray)
        {
            
            gl.Begin(OpenGL.GL_TRIANGLES);
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
        }

        public void DrawGouraud(OpenGL gl, Texture glTexture, TM64_Geometry.OK64F3DObject TargetObject)
        {

            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in TargetObject.modelGeometry)
            {
                foreach (var subVert in subFace.VertData)
                {
                    gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, subVert.color.A);
                    gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
                }
            }
        }

        public void DrawTexturedTexturedNoFlush(OpenGL gL, TM64_Geometry.OK64Texture oK64Texture, TM64_Geometry.OK64F3DObject oK64F3DObject)
        {
            throw new NotImplementedException();
        }

        public void DrawShaded(OpenGL gl, Texture glTexture, TM64_Geometry.OK64F3DObject TargetObject, float[] colorArray, int[] Zone)
        {
            
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            glTexture.Destroy(gl);
            gl.Begin(OpenGL.GL_TRIANGLES);
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
            
        }


        uint[] WrapTypes = new uint[]
        {
                OpenGL.GL_REPEAT,
                OpenGL.GL_REPEAT,
                OpenGL.GL_MIRRORED_REPEAT,
                OpenGL.GL_CLAMP,
                OpenGL.GL_REPEAT,
        };

        public void DrawGLCull(OpenGL GL, TM64_Geometry.OK64Texture TextureObject)
        {
            bool Enable = false;

            if (TextureObject.GeometryBools[5])
            {
                Enable = true;

                GL.CullFace(OpenGL.GL_FRONT_AND_BACK);
                
            }
            else if (TextureObject.GeometryBools[3])
            {
                Enable = true;

                if (TextureObject.GeometryBools[4])
                {
                    GL.CullFace(OpenGL.GL_FRONT_AND_BACK);
                    
                }
                else
                {
                    GL.CullFace(OpenGL.GL_FRONT);
                }
            }
            else if (TextureObject.GeometryBools[4])
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


        public void DrawTextureFlush(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, Texture glTexture, int TargetID)
        {


            gl.End();

            uint[] WrapTypes = { OpenGL.GL_REPEAT, OpenGL.GL_REPEAT, OpenGL.GL_MIRRORED_REPEAT, OpenGL.GL_CLAMP_TO_EDGE, OpenGL.GL_MIRRORED_REPEAT };

            if (textureArray[TargetID].texturePath == null)
            {
                MessageBox.Show("Error loading texture for " + textureArray[TargetID].textureName);
            }
            glTexture.Bind(gl);


            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[textureArray[TargetID].SFlag] });
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, new uint[] { WrapTypes[textureArray[TargetID].TFlag] });


        }
        public void DrawTextureFlushScreen(OpenGL gl, int Width, int Height, TM64_Geometry.OK64Texture TextureObject, Texture glTexture)
        {

            

            glTexture.Destroy(gl);

            uint[] WrapTypes = { OpenGL.GL_REPEAT, OpenGL.GL_REPEAT, OpenGL.GL_MIRRORED_REPEAT, OpenGL.GL_CLAMP_TO_EDGE, OpenGL.GL_MIRRORED_REPEAT };

            glTexture.Create(gl, RenderScreen(gl, TextureObject.textureScreen - 1, Width, Height));
            glTexture.Bind(gl);

            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[TextureObject.SFlag] });
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, new uint[] { WrapTypes[TextureObject.SFlag] });
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, new uint[] { WrapTypes[TextureObject.TFlag] });
        }
        public void DrawTexturedNoFlush(OpenGL gl, TM64_Geometry.OK64Texture TextureObject, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.Begin(OpenGL.GL_TRIANGLES);
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

        public void DrawTextured(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            
            glTexture.Destroy(gl);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            if (textureArray[targetObject.materialID].texturePath == null)
            {
                MessageBox.Show("Error loading texture for " + targetObject.objectName);
            }
            glTexture.Create(gl, textureArray[targetObject.materialID].texturePath);
            glTexture.Bind(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                DrawFace(gl, subFace);
            }
            
            
        }

        public void DrawTextured(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            
            glTexture.Destroy(gl);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            if (textureArray[targetObject.materialID].texturePath == null)
            {
                MessageBox.Show("Error loading texture for " + targetObject.objectName);
            }
            glTexture.Create(gl, textureArray[targetObject.materialID].texturePath);
            glTexture.Bind(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                DrawFace(gl, subFace, Zone);
            }
            
        }


        public void DrawCursor(OpenGL gl, TMCamera LocalCamera, Texture glTexture)
        {
            
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);

            foreach (var Face in LocalCamera.Cursor)
            {
                foreach (var subVert in Face.VertData)
                {
                    gl.Color(1.0f, 0.5f, 0f, 1.0f);
                    gl.Vertex(subVert.position.x + LocalCamera.marker.X, subVert.position.y + LocalCamera.marker.Y, subVert.position.z + LocalCamera.marker.Z);
                }
            }

            
        }
        public void DrawTarget(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);

            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashWhite);

            
        }





        public void DrawTarget(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            
            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashWhite, Zone);
            
            
        }


        public void DrawWire(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            gl.Begin(OpenGL.GL_TRIANGLES);
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

