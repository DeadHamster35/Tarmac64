using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assimp;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using Tarmac64_Library;

namespace Tarmac64_Library
{
    public class TM64_GL
    {



        public class TMCamera
        {
            public Vector3D position { get; set; }
            public float TargetHeight { get; set; }
            public Vector3D target { get; set; }
            public Vector3D marker { get; set; }
            public double rotation { get; set; }
            public float[] flashRed { get; set; }
            public float[] flashWhite { get; set; }
            public float[] flashYellow { get; set; }
            public TM64_Geometry.Face[] Cursor { get; set; }

        }


        

        public void DrawFace(OpenGL gl, TM64_Geometry.Face subFace)
        {
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.RFloat, subVert.color.GFloat, subVert.color.BFloat, subVert.color.AFloat);
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
            }
        }

        public void DrawMarker( OpenGL gl, Texture glTexture, TM64_Geometry.Face subFace, float[] Color, float[] Position)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            glTexture.Destroy(gl);
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
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x + Position[0], subVert.position.y+ Position[1], subVert.position.z+ Position[2]);
            }
        }
        public float[] GetAlphaFlash(float[] flashColor)
        {
            if (flashColor[4] == 0.0f)
            {
                if (flashColor[3] < 1.0f)
                {
                    flashColor[3] = Convert.ToSingle(flashColor[3] + 0.1);
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
                    flashColor[3] = Convert.ToSingle(flashColor[3] - 0.1);
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

            
            float[] targetPosition = new float[3] { LocalCamera.target.X, LocalCamera.target.Y, LocalCamera.target.Z + 60 };

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
            gl.End();
        }

        public void DrawSection(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashRed);
            
            gl.End();
        }


        public void DrawOKObjectShaded(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType, float[] ObjectColor)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            glTexture.Destroy(gl);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var ThisGeometry in TargetObjectType.ModelData)
            {
                foreach (var Face in ThisGeometry.modelGeometry)
                {
                    if (ObjectColor.Length > 3)
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], ObjectColor[3]);
                            gl.Vertex((subVert.position.x * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (subVert.position.y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (subVert.position.z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                    else
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], 1.0f);
                            gl.Vertex((subVert.position.x * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (subVert.position.y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (subVert.position.z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                }

            }
        }

        public void DrawOKObjectShaded(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            glTexture.Destroy(gl);
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
                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], ObjectColor[3]);
                            gl.Vertex((subVert.position.x * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (subVert.position.y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (subVert.position.z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                    else
                    {
                        foreach (var subVert in Face.VertData)
                        {
                            gl.Color(ObjectColor[0], ObjectColor[1], ObjectColor[2], 1.0f);
                            gl.Vertex((subVert.position.x * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (subVert.position.y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (subVert.position.z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                        }
                    }
                }
            }
        }


        public void DrawOKObjectTextured(OpenGL gl, Texture glTexture, TM64_Course.OKObject TargetObject, TM64_Course.OKObjectType TargetObjectType)
        {
            foreach (var Geometry in TargetObjectType.ModelData)
            {
                gl.End();
                glTexture.Destroy(gl);
                gl.Enable(OpenGL.GL_TEXTURE_2D);
                if (TargetObjectType.TextureData[Geometry.materialID].texturePath == null)
                {
                    MessageBox.Show("Error loading texture for " + TargetObjectType.Name);
                }
                glTexture.Create(gl, TargetObjectType.TextureData[Geometry.materialID].texturePath);
                glTexture.Bind(gl);
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                gl.Begin(OpenGL.GL_TRIANGLES);
                foreach (var Face in Geometry.modelGeometry)
                {                    
                    foreach (var subVert in Face.VertData)
                    {
                        gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, 1.0f);
                        gl.TexCoord(subVert.position.u, subVert.position.v);
                        gl.Vertex((subVert.position.x * TargetObjectType.ModelScale) + TargetObject.OriginPosition[0], (subVert.position.y * TargetObjectType.ModelScale) + TargetObject.OriginPosition[1], (subVert.position.z * TargetObjectType.ModelScale) + TargetObject.OriginPosition[2]);
                    }                    
                }

            }
            
        }


        public void DrawShaded(OpenGL gl, Texture glTexture, TM64_Geometry.OK64F3DObject TargetObject, float[] colorArray)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            glTexture.Destroy(gl);
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

        public void DrawShaded(OpenGL gl, Texture glTexture, TM64_Geometry.OK64F3DObject TargetObject, float[] colorArray, int[] Zone)
        {
            gl.End();
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
        public void DrawTextureFlush(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, Texture glTexture, int TargetID)
        {
            gl.End();

            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);                        
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            gl.Enable(OpenGL.GL_COLOR_MATERIAL);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            glTexture.Destroy(gl);



            if (textureArray[TargetID].texturePath == null)
            {
                MessageBox.Show("Error loading texture for " + textureArray[TargetID].textureName);
            }
            glTexture.Create(gl, textureArray[TargetID].texturePath);
            glTexture.Bind(gl);

            
            gl.Begin(OpenGL.GL_TRIANGLES);
            gl.LoadIdentity();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            
        }
        public void DrawTexturedNoFlush(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {   
            foreach (var subFace in targetObject.modelGeometry)
            {
                DrawFace(gl, subFace);
            }
        }

        public void UpdateTarget(TMCamera LocalCamera)
        {
            float[] localCoord = new float[3];
            float hAngle = Convert.ToSingle(LocalCamera.rotation * (Math.PI / 180));
            
            localCoord[0] = Convert.ToSingle(LocalCamera.position.X + 50 * Math.Cos(hAngle));
            localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + 50 * Math.Sin(hAngle));
            localCoord[2] = LocalCamera.position.Z;
            LocalCamera.target = new Vector3D(localCoord[0], localCoord[1], localCoord[2] + LocalCamera.TargetHeight);
        }

        public void MoveCamera(int direction, TMCamera LocalCamera, float moveDistance)
        {
            float[] localCoord = new float[3];
            switch (direction)
            {
                case 0:
                    {
                        //forward
                        float hAngle = Convert.ToSingle(LocalCamera.rotation * (Math.PI / 180));
                        
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X + moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + moveDistance * Math.Sin(hAngle));
                        localCoord[2] = LocalCamera.position.Z;
                        

                        break;
                    }
                case 1:
                    {
                        //back
                        float hAngle = Convert.ToSingle(LocalCamera.rotation * (Math.PI / 180));
                        
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X - moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y - moveDistance * Math.Sin(hAngle));
                        localCoord[2] = LocalCamera.position.Z;
                        
                        break;
                    }
                case 2:
                    {
                        //up
                        float hAngle = Convert.ToSingle(LocalCamera.rotation * (Math.PI / 180));
                        
                        localCoord[0] = LocalCamera.position.X;
                        localCoord[1] = LocalCamera.position.Y;
                        localCoord[2] = LocalCamera.position.Z + moveDistance;
                        
                        break;
                    }
                case 3:
                    {
                        //down
                        float hAngle = Convert.ToSingle(LocalCamera.rotation * (Math.PI / 180));
                        
                        localCoord[0] = LocalCamera.position.X;
                        localCoord[1] = LocalCamera.position.Y;
                        localCoord[2] = LocalCamera.position.Z - moveDistance;
                        

                        break;
                    }
                case 4:
                    {
                        //strafe
                        float strafeAngle = Convert.ToSingle(LocalCamera.rotation - 90);
                        if (strafeAngle < 0)
                            strafeAngle += 360;
                        float hAngle = Convert.ToSingle(strafeAngle * (Math.PI / 180));
                        
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X + moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + moveDistance * Math.Sin(hAngle));
                        localCoord[2] = LocalCamera.position.Z;
                        

                        break;
                    }
                case 5:
                    {
                        //strafe
                        float strafeAngle = Convert.ToSingle(LocalCamera.rotation + 90);
                        if (strafeAngle > 360)
                            strafeAngle -= 360;
                        float hAngle = Convert.ToSingle(strafeAngle * (Math.PI / 180));
                        
                        localCoord[0] = Convert.ToSingle(LocalCamera.position.X + moveDistance * Math.Cos(hAngle));
                        localCoord[1] = Convert.ToSingle(LocalCamera.position.Y + moveDistance * Math.Sin(hAngle));
                        localCoord[2] = LocalCamera.position.Z;
                        
                        break;
                    }
            }


            LocalCamera.position = new Vector3D(localCoord[0], localCoord[1], localCoord[2]);
            UpdateTarget(LocalCamera);

        }

        public void DrawTextured(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
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
            gl.End();
            
        }

        public void DrawTextured(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            gl.End();
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
            gl.End();
        }


        public void DrawCursor(OpenGL gl, TMCamera LocalCamera, Texture glTexture)
        {
            gl.End();
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
                    gl.Vertex(subVert.position.x + LocalCamera.marker[0], subVert.position.y + LocalCamera.marker[1], subVert.position.z + LocalCamera.marker[2]);
                }
            }

            gl.End();
        }
        public void DrawTarget(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);

            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashWhite);

            gl.End();
        }





        public void DrawTarget(OpenGL gl, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            gl.End();
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            
            DrawShaded(gl, glTexture, targetObject, LocalCamera.flashWhite, Zone);
            
            gl.End();
        }


        public void DrawWire(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera LocalCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
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
            gl.End();
        }

        
    }

}

