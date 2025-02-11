using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL;
using System.Windows.Input;
using Assimp;
using System.Diagnostics;
using Tarmac64_Library;
using System.Drawing.Imaging;
using System.Threading;

namespace Tarmac64_Retail
{
    public partial class GLViewer : UserControl
    {
        public GLViewer()
        {
            InitializeComponent();
        }

        float MoveSpeed = 5;


        public enum ControlMode
        {
            Scene,
            Section,
            Render,
            Surface,
            Objects,
        }

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_GL TarmacGL = new TM64_GL();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        double FPS;
        public TM64_GL.TMCamera LocalCamera = new TM64_GL.TMCamera();

        public ControlMode TargetingMode = ControlMode.Scene;
        public int SelectedRender, SelectedSection, SelectedSurface, SelectedObject;
        public int TargetedRender, TargetedSection, TargetedSurface, TargetedObject;


        long FrameTime;
        int FPSStall = 0;
        Stopwatch FrameWatch = new Stopwatch();
        int ScreenRenderIndex = -1;

        public TM64_Paths.Pathlist[] PathMarker = new TM64_Paths.Pathlist[0];
        Bitmap[] ScreenRenders = new Bitmap[6];
        public TM64_Geometry.OK64F3DObject[] CourseModel = new TM64_Geometry.OK64F3DObject[0];
        public TM64_Geometry.OK64F3DObject[] SurfaceModel = new TM64_Geometry.OK64F3DObject[0];
        public List<TM64_Course.OKObject> CourseObjects = new List<TM64_Course.OKObject>();
        public TM64_Course.OKObjectType[] ObjectTypes = new TM64_Course.OKObjectType[0];
        public TM64_Geometry.OK64Texture[] TextureObjects = new TM64_Geometry.OK64Texture[0];


        public Bitmap[] BitmapData = new Bitmap[0];
        public int[] SectionList = new int[0];

        public OpenGL GL = new OpenGL();
        public SharpGL.SceneGraph.Assets.Texture[] GLTexture = new SharpGL.SceneGraph.Assets.Texture[1];
        int GLShadeIndex = 0;
        int GLObjectIndex = 0;


        public float[,] SkyColors = new float[3, 3] {
            { Convert.ToSingle(128/255.0), Convert.ToSingle(184 / 255.0), Convert.ToSingle(248 / 255.0) },
            { Convert.ToSingle(216/255.0), Convert.ToSingle(232 / 255.0), Convert.ToSingle(248 / 255.0) },
            { Convert.ToSingle(0/255.0), Convert.ToSingle(0 / 255.0), Convert.ToSingle(0 / 255.0) },
            };

        public bool UpdateDraw = false;
        public bool AntiFlicker = true;
        public bool DrawSky = true;

        public event EventHandler UpdateParent;

        public int RequestMode, OKObjectIndex, OKSelectedObject = -1;


        public bool SpeedChangeLock = false;

        public void RefreshView()
        {

            GL.End();
            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();
            GL.PushMatrix();
            GL.Ortho2D(-1, 1.0, -1, 1.0);


            GL.MatrixMode(OpenGL.GL_MODELVIEW);


            GL.Disable(OpenGL.GL_CULL_FACE);
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.Enable(OpenGL.GL_BLEND);
            //GLTexture[GLShadeIndex].Destroy(GL);
            GLTexture[GLShadeIndex].Bind(GL);



            if (DrawSky)
            {
                GL.Begin(OpenGL.GL_QUADS);


                GL.Color(SkyColors[0, 0], SkyColors[0, 1], SkyColors[0, 2]);
                GL.Vertex(-1.0, 1.0);
                GL.Vertex(1.0, 1.0);

                GL.Color(SkyColors[1, 0], SkyColors[1, 1], SkyColors[1, 2]);
                GL.Vertex(1.0, 0);
                GL.Vertex(-1.0, 0);

                GL.Vertex(-1.0, 0);
                GL.Vertex(1.0, 0);

                GL.Color(SkyColors[2, 0], SkyColors[2, 1], SkyColors[2, 2]);
                GL.Vertex(1.0, -1.0);
                GL.Vertex(-1.0, -1.0);
            }
            

            GL.End();



            GL.PopMatrix();
            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.PopMatrix();
            GL.Viewport(0, 0, GLWindow.Width, GLWindow.Height);
            GL.LoadIdentity();
            GL.Perspective(90.0f, (double)Width / (double)Height, 1, 15000);
            GL.MatrixMode(OpenGL.GL_MODELVIEW);


        }

        public void CacheTextures()
        {
            GLTexture = new SharpGL.SceneGraph.Assets.Texture[BitmapData.Length + 2];
            GLShadeIndex = GLTexture.Length - 1;
            GLObjectIndex = GLTexture.Length - 2;
            for (int ThisTexture = 0; ThisTexture < BitmapData.Length; ThisTexture++)
            {
                GLTexture[ThisTexture] = new SharpGL.SceneGraph.Assets.Texture();
                if (BitmapData[ThisTexture] != null)
                {
                    GLTexture[ThisTexture].Create(GL, BitmapData[ThisTexture]);
                }
            }

            GLTexture[GLObjectIndex] = new SharpGL.SceneGraph.Assets.Texture();
            GLTexture[GLShadeIndex] = new SharpGL.SceneGraph.Assets.Texture();
        }
        private void GLWindow_Resized(object sender, EventArgs e)
        {
            RefreshView();
            UpdateDraw = true;
        }

        private void CheckInput()
        {
            float Delta = Convert.ToSingle(FrameTime / 33.333);
            long OldTime = DateTime.Now.Ticks;
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (Keyboard.IsKeyDown(Key.W))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(0, LocalCamera, MoveSpeed * Delta * 3.0f);
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(1, LocalCamera, MoveSpeed * Delta * 3.0f);
                }

                if (Keyboard.IsKeyDown(Key.A))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(5, LocalCamera, MoveSpeed * Delta * 3.0f);

                }
                if (Keyboard.IsKeyDown(Key.D))
                {

                    UpdateDraw = true;
                    TarmacGL.MoveCamera(4, LocalCamera, MoveSpeed * Delta * 3.0f);

                }
                if (Keyboard.IsKeyDown(Key.R))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(2, LocalCamera, MoveSpeed * Delta * 3.0f);
                }
                if (Keyboard.IsKeyDown(Key.F))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(3, LocalCamera, MoveSpeed * Delta * 3.0f);
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.W))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(0, LocalCamera, MoveSpeed * Delta);
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(1, LocalCamera, MoveSpeed * Delta);
                }

                if (Keyboard.IsKeyDown(Key.A))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(5, LocalCamera, MoveSpeed * Delta);

                }
                if (Keyboard.IsKeyDown(Key.D))
                {

                    UpdateDraw = true;
                    TarmacGL.MoveCamera(4, LocalCamera, MoveSpeed * Delta);

                }

                if (Keyboard.IsKeyDown(Key.Q))
                {

                    UpdateDraw = true;
                    float RadTurn = Convert.ToSingle((MoveSpeed * Delta / 2.0f) * (Math.PI / 180.0f));
                    LocalCamera.rotation[0] += RadTurn;
                }
                if (Keyboard.IsKeyDown(Key.E))
                {
                    UpdateDraw = true;
                    float RadTurn = Convert.ToSingle((MoveSpeed * Delta / 2.0f) * (Math.PI / 180.0f));
                    LocalCamera.rotation[0] -= RadTurn;
                }
                if (Keyboard.IsKeyDown(Key.R))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(2, LocalCamera, MoveSpeed * Delta);
                }
                if (Keyboard.IsKeyDown(Key.F))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(3, LocalCamera, MoveSpeed * Delta);
                }
                if (Keyboard.IsKeyDown(Key.T))
                {
                    UpdateDraw = true;
                    if (!SpeedChangeLock)
                    {
                        UpdateDraw = true;
                        if (MoveSpeed >= 5)
                        {
                            MoveSpeed += 5;
                        }
                        else
                        {
                            MoveSpeed += 1;
                        }
                    }
                    SpeedChangeLock = true;
                }
                else if (Keyboard.IsKeyDown(Key.G))
                {
                    if (!SpeedChangeLock)
                    {
                        UpdateDraw = true;
                        if (MoveSpeed <= 5)
                        {
                            if (MoveSpeed > 0)
                            {
                                MoveSpeed -= 1;
                            }
                        }
                        else
                        {
                            MoveSpeed -= 5;
                        }
                    }
                    SpeedChangeLock = true;

                }
                else
                {
                    SpeedChangeLock = false;
                }



            }

            TarmacGL.UpdateTarget(LocalCamera);
        }


        private void ShiftST(int ThisTexture)
        {
            double Add = Convert.ToDouble(((FrameTime / 33.333) * (TextureObjects[ThisTexture].textureScrollT) / 32.0) / 4.0);
            TextureObjects[ThisTexture].GLShiftT -= Add;
            Add = Convert.ToDouble(((FrameTime / 33.333) * (TextureObjects[ThisTexture].textureScrollS) / 32.0) / 4.0);
            TextureObjects[ThisTexture].GLShiftS -= Add;

            while (TextureObjects[ThisTexture].GLShiftT > 1)
            {
                TextureObjects[ThisTexture].GLShiftT -= 1;
            }
            while (TextureObjects[ThisTexture].GLShiftT < 0)
            {
                TextureObjects[ThisTexture].GLShiftT += 1;
            }
            while (TextureObjects[ThisTexture].GLShiftS > 1)
            {
                TextureObjects[ThisTexture].GLShiftS -= 1;
            }
            while (TextureObjects[ThisTexture].GLShiftS < 0)
            {
                TextureObjects[ThisTexture].GLShiftS += 1;
            }
        }




        private void DrawSection()
        {
            for (int ThisTexture = 0; ThisTexture < TextureObjects.Length; ThisTexture++)
            {
                if (TextureObjects[ThisTexture].texturePath != null)
                {
                    //check if this is a Framebuffer-Texture
                    if (!RenderCheckbox.Checked || (TextureObjects[ThisTexture].textureScreen == 0))
                    {
                        //Draw Regular Textured Objects.

                        ShiftST(ThisTexture);
                        TarmacGL.DrawTextureFlush(GL, TextureObjects, GLTexture[ThisTexture], ThisTexture);
                        TarmacGL.DrawGLCull(GL, TextureObjects[ThisTexture]);

                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (CourseModel[ThisObject].materialID == ThisTexture)
                            {
                                if (SectionList.Contains(ThisObject))
                                {
                                    TarmacGL.DrawTexturedNoFlush(GL, TextureObjects[ThisTexture], CourseModel[ThisObject]);
                                }
                            }
                        }

                        
                    }
                    else
                    {
                        //Draw Framebuffer Textured Objects.

                        ShiftST(ThisTexture);
                        TarmacGL.DrawTextureFlushScreen(GL, GLWindow.Width, GLWindow.Height, TextureObjects[ThisTexture], GLTexture[ThisTexture]);
                        TarmacGL.DrawGLCull(GL, TextureObjects[ThisTexture]);

                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (CourseModel[ThisObject].materialID == ThisTexture)
                            {
                                if (SectionList.Contains(ThisObject))
                                {
                                    TarmacGL.DrawTexturedNoFlush(GL, TextureObjects[ThisTexture], CourseModel[ThisObject]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Draw Gouraud Objects

                    GL.Disable(OpenGL.GL_CULL_FACE);
                    GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                    GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                    GL.Enable(OpenGL.GL_BLEND);
                    //GLTexture[GLShadeIndex].Destroy(GL);
                    GLTexture[GLShadeIndex].Bind(GL);

                    for (int ThisObject = 0; ThisObject < SectionList.Length; ThisObject++)
                    {
                        if (CourseModel[SectionList[ThisObject]].materialID == ThisTexture)
                        {
                            TarmacGL.DrawGouraud(GL, CourseModel[SectionList[ThisObject]]);
                        }
                    }

                    //re-enable textured polygons
                    GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                    GL.Enable(OpenGL.GL_TEXTURE_2D);
                    GL.Enable(OpenGL.GL_BLEND);
                    GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                    GL.ShadeModel(OpenGL.GL_SMOOTH);
                    GL.Enable(OpenGL.GL_COLOR_MATERIAL);
                    GL.Enable(OpenGL.GL_TEXTURE_2D);
                    GL.FrontFace(OpenGL.GL_CCW);
                }
            }
            if (chkWireframe.Checked)
            {
                GL.End();
                GLTexture[GLShadeIndex].Bind(GL);
                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);

                for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                {
                    if (!SectionList.Contains(ThisObject))
                    {
                        TarmacGL.DrawWire(GL, CourseModel[ThisObject]);
                    }
                }
            }
            GL.End();
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.Begin(OpenGL.GL_TRIANGLES);

        }





        private void DrawDefault()
        {
            for (int ThisTexture = 0; ThisTexture < TextureObjects.Length; ThisTexture++)
            {
                if (TextureObjects[ThisTexture].texturePath != null)
                {
                    //check if this is a Framebuffer-Texture
                    if (!RenderCheckbox.Checked || (TextureObjects[ThisTexture].textureScreen == 0))
                    {
                        //Draw Regular Textured Objects.

                        ShiftST(ThisTexture);
                        TarmacGL.DrawTextureFlush(GL, TextureObjects, GLTexture[ThisTexture], ThisTexture);
                        TarmacGL.DrawGLCull(GL, TextureObjects[ThisTexture]);

                        
                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (CourseModel[ThisObject].materialID == ThisTexture)
                            {
                                TarmacGL.DrawTexturedNoFlush(GL, TextureObjects[ThisTexture], CourseModel[ThisObject]);
                            }
                        }


                    }
                    else
                    {
                        //Draw Framebuffer Textured Objects.

                        ShiftST(ThisTexture);
                        TarmacGL.DrawTextureFlushScreen(GL, GLWindow.Width, GLWindow.Height, TextureObjects[ThisTexture], GLTexture[ThisTexture]);
                        TarmacGL.DrawGLCull(GL, TextureObjects[ThisTexture]);

                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (CourseModel[ThisObject].materialID == ThisTexture)
                            {
                                TarmacGL.DrawTexturedNoFlush(GL, TextureObjects[ThisTexture], CourseModel[ThisObject]);
                            }
                        }
                    }
                }
                else
                {
                    //Draw Gouraud Objects

                    GL.Disable(OpenGL.GL_CULL_FACE);
                    GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                    GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                    GL.Enable(OpenGL.GL_BLEND);
                    //GLTexture[GLShadeIndex].Destroy(GL);
                    GLTexture[GLShadeIndex].Bind(GL);


                    for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                    {
                        if (CourseModel[ThisObject].materialID == ThisTexture)
                        {
                            TarmacGL.DrawGouraud(GL, CourseModel[ThisObject]);
                        }
                    }

                    //re-enable textured polygons
                    GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                    GL.Enable(OpenGL.GL_TEXTURE_2D);
                    GL.Enable(OpenGL.GL_BLEND);
                    GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                    GL.ShadeModel(OpenGL.GL_SMOOTH);
                    GL.Enable(OpenGL.GL_COLOR_MATERIAL);
                    GL.Enable(OpenGL.GL_TEXTURE_2D);
                    GL.FrontFace(OpenGL.GL_CCW);
                }
            }
        }


        private void DrawDefaultGouraud()
        {
            GL.Disable(OpenGL.GL_CULL_FACE);
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.Enable(OpenGL.GL_BLEND);
            //GLTexture[GLShadeIndex].Destroy(GL);
            GLTexture[GLShadeIndex].Bind(GL);

            for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
            {
                TarmacGL.DrawGouraudObjectColor(GL, CourseModel[ThisObject]);
            }


            //re-enable textured polygons
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.ShadeModel(OpenGL.GL_SMOOTH);
            GL.Enable(OpenGL.GL_COLOR_MATERIAL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.FrontFace(OpenGL.GL_CCW);
        }

        private void DrawSectionGouraud()
        {
            GL.Disable(OpenGL.GL_CULL_FACE);
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.Enable(OpenGL.GL_BLEND);
            //GLTexture[GLShadeIndex].Destroy(GL);
            GLTexture[GLShadeIndex].Bind(GL);

            
            for (int ThisObject = 0; ThisObject < SectionList.Length; ThisObject++)
            {
                TarmacGL.DrawGouraudObjectColor(GL, CourseModel[SectionList[ThisObject]]);
            }



            if (chkWireframe.Checked)
            {
                GL.End();
                GLTexture[GLShadeIndex].Bind(GL);
                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);

                for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                {
                    if (!SectionList.Contains(ThisObject))
                    {
                        TarmacGL.DrawWire(GL, CourseModel[ThisObject]);
                    }
                }
            }


            //re-enable textured polygons
            GL.End();
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.ShadeModel(OpenGL.GL_SMOOTH);
            GL.Enable(OpenGL.GL_COLOR_MATERIAL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.FrontFace(OpenGL.GL_CCW);
            GL.Begin(OpenGL.GL_TRIANGLES);
        }




        private void DrawSurface()
        {
            GL.Disable(OpenGL.GL_CULL_FACE);
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.Enable(OpenGL.GL_BLEND);
            //GLTexture[GLShadeIndex].Destroy(GL);
            GLTexture[GLShadeIndex].Bind(GL);

            for (int ThisObject = 0; ThisObject < SurfaceModel.Length; ThisObject++)
            {                
                TarmacGL.DrawGouraudObjectColor(GL, SurfaceModel[ThisObject]);
            }
            

            //re-enable textured polygons
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.ShadeModel(OpenGL.GL_SMOOTH);
            GL.Enable(OpenGL.GL_COLOR_MATERIAL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.FrontFace(OpenGL.GL_CCW);
        }

        private void DrawOKObjects()
        {
            for (int ThisObject = 0; ThisObject < CourseObjects.Count; ThisObject++)
            {
                if ((ThisObject != TargetedObject) && (ThisObject != OKSelectedObject))
                {
                    if (ObjectTypes[CourseObjects[ThisObject].TypeIndex].TextureData != null)
                    {
                        GL.End();
                        GL.Enable(OpenGL.GL_TEXTURE_2D);
                        GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        TarmacGL.DrawOKObjectTextured(GL, GLTexture[GLObjectIndex], CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].TypeIndex]);
                    }
                    else
                    {
                        GL.End();
                        GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                        GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                        GL.Enable(OpenGL.GL_BLEND);
                        GLTexture[GLShadeIndex].Bind(GL);
                        TarmacGL.DrawOKObjectShaded(GL, GLTexture[GLShadeIndex], CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].TypeIndex]);
                    }

                }
            }

            GL.End();
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.Enable(OpenGL.GL_BLEND);
            GLTexture[GLShadeIndex].Bind(GL);

            if (OKSelectedObject != -1)
            {
                TarmacGL.DrawOKObjectShaded(GL, GLTexture[GLShadeIndex], CourseObjects[OKSelectedObject], ObjectTypes[CourseObjects[OKSelectedObject].TypeIndex], LocalCamera.flashWhite);
            }
            if (TargetedObject != -1)
            {
                TarmacGL.DrawOKObjectShaded(GL, GLTexture[GLShadeIndex], CourseObjects[TargetedObject], ObjectTypes[CourseObjects[TargetedObject].TypeIndex], LocalCamera.flashWhite);
            }

        }
        private void DrawOKObjectsGouaraud()
        {
            for (int ThisObject = 0; ThisObject < CourseObjects.Count; ThisObject++)
            {
                if ((ThisObject != TargetedObject) && (ThisObject != OKSelectedObject))
                {
                    GL.End();
                    GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                    GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                    GL.Enable(OpenGL.GL_BLEND);
                    GLTexture[GLShadeIndex].Bind(GL);
                    TarmacGL.DrawOKObjectShaded(GL, GLTexture[GLShadeIndex], CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].TypeIndex]);
                }
            }
        }
        private void DrawScene()
        {
            GL.End();
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);

            GL.AlphaFunc(OpenGL.GL_GREATER, Convert.ToSingle(0.1));
            GL.Enable(OpenGL.GL_ALPHA_TEST);
            GL.Enable(OpenGL.GL_BLEND);
            GL.BlendEquation(OpenGL.GL_ADD);
            GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            GL.ShadeModel(OpenGL.GL_SMOOTH);
            GL.Enable(OpenGL.GL_COLOR_MATERIAL);
            GL.Enable(OpenGL.GL_TEXTURE_2D);
            GL.FrontFace(OpenGL.GL_CCW);

            //draw course model first

            switch (TargetingMode)
            {
                case ControlMode.Scene:
                    {
                        if (CheckboxTextured.Checked)
                        {
                            DrawDefault();
                        }
                        else
                        {
                            DrawDefaultGouraud();
                        }                        
                        break;
                    }

                case ControlMode.Section:
                    {
                        if (CheckboxTextured.Checked)
                        {
                            DrawSection();
                        }
                        else
                        {
                            DrawSectionGouraud();
                        }
                        break;
                    }

                case ControlMode.Render:
                    {
                        if (CheckboxTextured.Checked)
                        {
                            DrawDefault();
                        }
                        else
                        {
                            DrawDefaultGouraud();
                        }
                        break;
                    }

                case ControlMode.Surface:
                    {
                        DrawSurface();
                        break;
                    }

                case ControlMode.Objects:
                    {
                        if (CheckboxTextured.Checked)
                        {
                            DrawDefault();
                            DrawOKObjects();
                        }
                        else
                        {
                            DrawDefaultGouraud();
                            DrawOKObjectsGouaraud();
                        }
                        break;
                    }

            }




            TM64_Geometry TarmacGeo = new TM64_Geometry();
            TM64_Geometry.Face[] Marker = TarmacGeo.CreateStandard(Convert.ToSingle(5.0));

            GL.End();
            GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
            GL.Begin(OpenGL.GL_TRIANGLES);
            if (CheckboxPaths.Checked)
            {
                foreach (var ThisPath in PathMarker)
                {
                    foreach (var ThisMark in ThisPath.pathmarker)
                    {
                        foreach (var ThisFace in Marker)
                        {
                            TarmacGL.DrawMarker(GL, GLTexture[GLShadeIndex], ThisFace, ThisMark.Color, ThisMark);
                        }
                    }
                }
            }

        }

        private void GLWindow_OpenGLDraw(object sender, RenderEventArgs args)
        {
            if (GLWindow.ClientRectangle.Contains(GLWindow.PointToClient(Control.MousePosition)))
            {
                int TempInt = 240;
                if (int.TryParse(FPSBox.Text, out TempInt))
                {
                    if (TempInt > 144)
                    {
                        TempInt = 144;
                    }
                    if (TempInt < 0)
                    {
                        TempInt = 0;
                    }

                    FPSBox.Text = TempInt.ToString();
                    GLWindow.FrameRate = TempInt * 2;

                };

                UpdateDraw = true;
            }
            else
            {
                GLWindow.FrameRate = 1;
            }


            if (Parent.ContainsFocus)
            {
                CheckInput();
            }
            else
            {
                GLWindow.FrameRate = 1;
                return;
            }



            if ((UpdateDraw) || (AntiFlicker))
            {
                LocalCamera.flashWhite = TarmacGL.GetAlphaFlash(LocalCamera.flashWhite, (FrameTime / 33.333));
                LocalCamera.flashRed = TarmacGL.GetAlphaFlash(LocalCamera.flashRed, (FrameTime / 33.333));
                LocalCamera.flashYellow = TarmacGL.GetAlphaFlash(LocalCamera.flashYellow, (FrameTime / 33.333));



                SpeedBox.Text = MoveSpeed.ToString() + ".0";


                FrameTime = FrameWatch.ElapsedMilliseconds;
                FPS = Math.Round(1000.0 / FrameTime, 1);
                FrameWatch.Restart();



                GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                GL.Disable(OpenGL.GL_DEPTH_TEST);
                GL.LoadIdentity();
                RefreshView();
                GL.Enable(OpenGL.GL_DEPTH_TEST);

                AntiFlicker = UpdateDraw;
                UpdateDraw = true;


                if (FPSStall > 10)
                {
                    FPSStall = 0;
                    FPSDisplay.Text = "FPS-" + FPS.ToString(); ;
                }
                else
                {
                    FPSStall++;
                }

                ScreenRenderIndex++;
                if (ScreenRenderIndex == 6)
                {
                    ScreenRenderIndex = 0;
                }


                GL.LookAt(LocalCamera.position.X, LocalCamera.position.Y, LocalCamera.position.Z, LocalCamera.target.X, LocalCamera.target.Y, LocalCamera.target.Z, 0, 0, 1);

                DrawScene();
                GL.End();


                string Error = GL.GetErrorDescription(GL.GetError());

                UpdateDraw = false;
            }


            GL.End();


        }






        private void GLWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {


        }

        private bool MouseTest(out Vector3D Intersection, TM64_Geometry.OK64F3DObject TargetObject, Vector3D RayOrigin, Vector3D RayTarget)
        {
            Intersection = new Vector3D(-1, -1, -1);
            foreach (var face in TargetObject.modelGeometry)
            {

                Intersection = TarmacGeometry.testIntersect(RayOrigin, RayTarget, face.VertData[0], face.VertData[1], face.VertData[2]);
                if (Intersection.X > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool MouseTestOrigin(out Vector3D Intersection, TM64_Geometry.OK64F3DObject TargetObject, Vector3D RayOrigin, Vector3D RayTarget, float[] TargetOrigin, float[] Scale)
        {
            Intersection = new Vector3D(-1, -1, -1);
            foreach (var face in TargetObject.modelGeometry)
            {

                Intersection = TarmacGeometry.testIntersectScale(RayOrigin, RayTarget, face.VertData[0], face.VertData[1], face.VertData[2], TargetOrigin, Scale);
                if (Intersection.X > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            SettingsPanel.Visible = !SettingsPanel.Visible;
        }

        private void CheckboxTextured_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ScreenshotBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog FileSave = new SaveFileDialog();
            FileSave.Filter = "PNG (*.png)|*.png|All Files (*.*)|*.*";
            FileSave.DefaultExt = ".png";
            if (FileSave.ShowDialog() == DialogResult.OK)
            {
                TarmacGL.Save2Picture(GL, 0, 0, GLWindow.Width, GLWindow.Height).Save(FileSave.FileName, ImageFormat.Png);
            }

        }

        private void GLViewer_Load(object sender, EventArgs e)
        {
            TargetedObject = -1;
            OKSelectedObject = -1;
            GL = GLWindow.OpenGL;
            RefreshView();
            FrameWatch.Start();
            LocalCamera.Cursor = TarmacGeometry.CreateStandard(1.5f);
            LocalCamera.flashRed = new float[] { 1.0f, 0.0f, 0.0f, 1.0f, 0.0f };
            LocalCamera.flashYellow = new float[] { 1.0f, 1.0f, 0.0f, 1.0f, 0.0f };
            LocalCamera.flashWhite = new float[] { 1.0f, 1.0f, 1.0f, 0.5f, 0.0f };
        }

        private void GLWindow_Load(object sender, EventArgs e)
        {
            GLTexture[0] = new SharpGL.SceneGraph.Assets.Texture();
            GLShadeIndex = 0;
        }


        private void GLWindow_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UpdateDraw = true;

            switch(TargetingMode)
            {
                case (ControlMode.Scene):
                    {
                        break;
                    }
                case (ControlMode.Render):
                    {
                        if (TargetedRender != -1)
                        {
                            //Render Scene
                            if (e.Button == MouseButtons.Left)
                            {
                                SelectedRender = TargetedRender;
                                RequestMode = 2;
                            }
                        }
                        break;
                    }
                case (ControlMode.Surface):
                    {
                        if (TargetedSurface != -1)
                        {
                            //Render Scene
                            if (e.Button == MouseButtons.Left)
                            {
                                SelectedSurface = TargetedSurface;
                                RequestMode = 2;
                            }
                        }
                        break;
                    }
                case (ControlMode.Section):
                    {
                        if (TargetedSection != -1)
                        {
                            //Section List
                            if (e.Button == MouseButtons.Right)
                            {

                                List<int> TempList = SectionList.ToList();
                                if (TempList.Contains(TargetedSection))
                                {
                                    TempList.Remove(TargetedSection);
                                }
                                else
                                {
                                    TempList.Add(TargetedSection);
                                }
                                SectionList = TempList.ToArray();
                                RequestMode = 1;
                            }
                            if (e.Button == MouseButtons.Left)
                            {
                                SelectedSection = TargetedSection;
                                RequestMode = 2;
                            }
                        }                        
                        break;
                    }
                case (ControlMode.Objects):
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            TM64_Course.OKObject ThisObject = TarmacCourse.NewOKObject();
                            ThisObject.OriginPosition = new short[] { Convert.ToInt16(LocalCamera.marker.X), Convert.ToInt16(LocalCamera.marker.Y), Convert.ToInt16(LocalCamera.marker.Z + 3) };
                            ThisObject.TypeIndex = Convert.ToInt16(OKObjectIndex);
                            CourseObjects.Add(ThisObject);
                            RequestMode = 1;
                        }
                        else if ((Keyboard.IsKeyDown(Key.LeftShift)) && (OKSelectedObject != -1) && (e.Button != MouseButtons.Middle))
                        {
                            CourseObjects[OKSelectedObject].OriginPosition = new short[] { Convert.ToInt16(LocalCamera.marker.X), Convert.ToInt16(LocalCamera.marker.Y), Convert.ToInt16(LocalCamera.marker.Z) };
                            RequestMode = 2;
                        }
                        else
                        {
                            if (TargetedObject != -1)
                            {
                                OKSelectedObject = TargetedObject;
                            }
                            RequestMode = 3;
                        }
                        
                        break;
                    }
            }

            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }

        private void GLWindow_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Middle)
            {

                double[] pointA = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 0);
                double[] pointB = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 1);
                Vector3D RayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));
                Vector3D RayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2]));




                RayTarget.X = RayTarget.X - RayOrigin.X;
                RayTarget.Y = RayTarget.Y - RayOrigin.Y;
                RayTarget.Z = RayTarget.Z - RayOrigin.Z;

                LocalCamera.rotation[0] = Convert.ToSingle(Math.Atan2(RayTarget.Y, RayTarget.X));
                LocalCamera.rotation[1] = Convert.ToSingle(Math.Atan2(
                    RayTarget.Z,
                    Math.Sqrt((RayTarget.X * RayTarget.X) + (RayTarget.Y * RayTarget.Y))
                    ));

                TarmacGL.UpdateTarget(LocalCamera);

            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void CheckboxPaths_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void GLWindow_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OldMouseCoords[0] = -1;
            OldMouseCoords[1] = -1;
        }

        int[] OldMouseCoords = new int[2] {-1,-1};
        private void GLWindow_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!Parent.ContainsFocus)
            {
                return;
            }
            UpdateDraw = true;
            RefreshView();
            
            

            double[] pointA = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 0);
            double[] pointB = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 1);
            Vector3D RayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));             
            Vector3D RayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2]));




            RayTarget.X = RayTarget.X - RayOrigin.X;
            RayTarget.Y = RayTarget.Y - RayOrigin.Y;
            RayTarget.Z = RayTarget.Z - RayOrigin.Z;
            Vector3D Intersection = new Vector3D(0, 0, 0);



            if (e.Button == MouseButtons.Middle)
            {
                float DeltaX = 0;
                float DeltaY = 0;
                if (OldMouseCoords[0] != -1)
                {
                    DeltaX = e.X - OldMouseCoords[0];
                    DeltaY = e.Y - OldMouseCoords[1];
                }
                OldMouseCoords[0] = e.X;
                OldMouseCoords[1] = e.Y;



                LocalCamera.rotation[0] -= (DeltaX * 0.01f);
                LocalCamera.rotation[1] -= (DeltaY * 0.01f);
                

                TarmacGL.UpdateTarget(LocalCamera);

            }

            float objectDistance = -1;
            

            switch (TargetingMode)
            {
                case ControlMode.Scene:
                default:
                    {
                        break;
                    }
                case ControlMode.Section:
                    {
                        TargetedSection = -1;
                        SelectedSection = -1;
                        //Section List
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            for (int ThisObject = 0; ThisObject < SectionList.Length; ThisObject++)
                            {
                                if (MouseTest(out Intersection, CourseModel[SectionList[ThisObject]], RayOrigin, RayTarget))
                                {
                                    if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                    {
                                        objectDistance = Intersection.X;
                                        TargetedSection = SectionList[ThisObject];
                                    }
                                }
                            }
                        }
                        else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                            {
                                if (!SectionList.Contains(ThisObject))
                                {
                                    if (MouseTest(out Intersection, CourseModel[ThisObject], RayOrigin, RayTarget))
                                    {
                                        if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                        {
                                            objectDistance = Intersection.X;
                                            TargetedSection = ThisObject;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                            {
                                if (MouseTest(out Intersection, CourseModel[ThisObject], RayOrigin, RayTarget))
                                {
                                    if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                    {
                                        objectDistance = Intersection.X;
                                        TargetedSection = ThisObject;
                                    }
                                }
                                
                            }
                        }
                        break;
                    }
                case ControlMode.Surface:
                    {
                        // Surface Model
                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (MouseTest(out Intersection, CourseModel[ThisObject], RayOrigin, RayTarget))
                            {
                                if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                {
                                    objectDistance = Intersection.X;
                                    TargetedSurface = ThisObject;
                                }
                            }
                        }
                        break;
                    }
                case ControlMode.Objects:
                    {
                        // Custom Objects
                        for (int ThisList = 0; ThisList < CourseObjects.Count; ThisList++)
                        {
                            float Scale = ObjectTypes[CourseObjects[ThisList].TypeIndex].ModelScale;
                            for (int ThisObject = 0; ThisObject < ObjectTypes[CourseObjects[ThisList].TypeIndex].ModelData.Length; ThisObject++)
                            {
                                float[] OriginPosition = { Convert.ToSingle(CourseObjects[ThisList].OriginPosition[0]), Convert.ToSingle(CourseObjects[ThisList].OriginPosition[1]), Convert.ToSingle(CourseObjects[ThisList].OriginPosition[2]) };
                                float[] ModelScales = { Scale, Scale, Scale };
                                if (MouseTestOrigin(out Intersection, ObjectTypes[CourseObjects[ThisList].TypeIndex].ModelData[ThisObject], RayOrigin, RayTarget, OriginPosition, ModelScales))
                                {
                                    if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                    {
                                        TargetedObject = ThisList;
                                    }
                                }
                            }
                        }
                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (MouseTest(out Intersection, CourseModel[ThisObject], RayOrigin, RayTarget))
                            {
                                if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                {
                                    objectDistance = Intersection.X;
                                }
                            }
                        }
                        break;
                    }
                case ControlMode.Render:
                    {
                        //Section List
                        
                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (MouseTest(out Intersection, CourseModel[ThisObject], RayOrigin, RayTarget))
                            {
                                if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                {
                                    objectDistance = Intersection.X;
                                    TargetedRender = ThisObject;
                                }
                            }

                        }
                        
                        break;
                    }
            }

            if (objectDistance != -1)
            {
               LocalCamera.marker = new Assimp.Vector3D() { X = RayOrigin.X + (RayTarget.X * objectDistance), Y = RayOrigin.Y + (RayTarget.Y * objectDistance), Z = RayOrigin.Z + (RayTarget.Z * objectDistance) };
                
            }
        }
    }
}
