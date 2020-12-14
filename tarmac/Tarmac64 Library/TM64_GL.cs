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
    class TM64_GL
    {



        public class TMCamera
        {
            public Vector3D position { get; set; }
            public Vector3D target { get; set; }
            public double rotation { get; set; }
            public float[] flashRed { get; set; }
            public float[] flashWhite { get; set; }
            public float[] flashYellow { get; set; }

        }

        private OpenGL DrawFace(OpenGL gl, TM64_Geometry.Face subFace)
        {
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, 1.0f);
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x, subVert.position.y, subVert.position.z);
            }
            return gl;
        }

        private OpenGL DrawFace(OpenGL gl, TM64_Geometry.Face subFace, int[] Zone)
        {
            foreach (var subVert in subFace.VertData)
            {
                gl.Color(subVert.color.R, subVert.color.G, subVert.color.B, 1.0f);
                gl.TexCoord(subVert.position.u, subVert.position.v);
                gl.Vertex(subVert.position.x + (Zone[0] * 500), subVert.position.y + (Zone[1] * 500), subVert.position.z + (Zone[2] * 250));
            }
            return gl;
        }

        public OpenGL DrawNorth(OpenGL gl, Texture glTexture, TMCamera localCamera)
        {
            glTexture.Destroy(gl);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);


            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + 2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(localCamera.target.X + 0.0f, localCamera.target.Y + 2.0f, localCamera.target.Z + 0.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + -2.0f);
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(localCamera.target.X + -2.0f, localCamera.target.Y + -2.0f, localCamera.target.Z + 2.0f);
            gl.End();
            return gl;
        }
        public OpenGL DrawSection(OpenGL gl, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                gl = DrawShaded(gl, glTexture, subFace, localCamera.flashRed);
            }
            gl.End();
            return gl;
        }

        public OpenGL DrawShaded(OpenGL gl, TM64_Geometry.Face subFace, float[] colorArray, int[] Zone)
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
            return gl;
        }

        public OpenGL DrawShaded(OpenGL gl, Texture glTexture, TM64_Geometry.Face subFace, float[] colorArray)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            glTexture.Destroy(gl);
            gl.Begin(OpenGL.GL_TRIANGLES);
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
            return gl;
        }


        public OpenGL DrawTextured(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
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
                gl = DrawFace(gl, subFace);
            }
            gl.End();
            return gl;
        }

        public OpenGL DrawShaded(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            glTexture.Destroy(gl);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                gl = DrawShaded(gl, glTexture, subFace, targetObject.objectColor);
            }
            gl.End();
            return gl;
        }


        public OpenGL DrawTarget(OpenGL gl, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
        {
            gl.End();
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                gl = DrawShaded(gl, glTexture, subFace, localCamera.flashWhite);
            }
            gl.End();
            return gl;
        }





        public OpenGL DrawTextured(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
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
                gl = DrawFace(gl, subFace, Zone);
            }
            gl.End();
            return gl;
        }

        public OpenGL DrawShaded(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            gl.End();
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            glTexture.Destroy(gl);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                gl = DrawShaded(gl, subFace, targetObject.objectColor, Zone);
            }
            gl.End();
            return gl;
        }


        public OpenGL DrawTarget(OpenGL gl, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject, int[] Zone)
        {
            gl.End();
            glTexture.Destroy(gl);
            gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            gl.Enable(OpenGL.GL_BLEND);
            gl.Begin(OpenGL.GL_TRIANGLES);
            foreach (var subFace in targetObject.modelGeometry)
            {
                gl = DrawShaded(gl, subFace, localCamera.flashWhite, Zone);
            }
            gl.End();
            return gl;
        }


        public OpenGL DrawWire(OpenGL gl, TM64_Geometry.OK64Texture[] textureArray, TMCamera localCamera, Texture glTexture, TM64_Geometry.OK64F3DObject targetObject)
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

            return gl;
        }

        
    }

}

