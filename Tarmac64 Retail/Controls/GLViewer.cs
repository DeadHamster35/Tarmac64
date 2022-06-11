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

namespace Tarmac64_Retail
{
    public partial class GLViewer : UserControl
    {
        public GLViewer()
        {
            InitializeComponent();
        }
        float MoveSpeed = 5;

        TM64 Tarmac = new TM64();
        TM64_Course TarmacCourse = new TM64_Course();
        TM64_GL TarmacGL = new TM64_GL();
        TM64_Geometry TarmacGeometry = new TM64_Geometry();
        double FPS;
        public TM64_GL.TMCamera LocalCamera = new TM64_GL.TMCamera();
        public int TargetMode = 0;

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
        public int[] SectionList = new int[0];

        public OpenGL GL = new OpenGL();
        public SharpGL.SceneGraph.Assets.Texture GLTexture = new SharpGL.SceneGraph.Assets.Texture();

        public bool UpdateDraw = false;
        public bool AntiFlicker = true;

        public event EventHandler UpdateParent;

        public int TargetedObject, SelectedObject, RequestMode, OKObjectIndex, OKSelectedObject = -1;


        public bool SpeedChangeLock = false;

        public void RefreshView()
        {
            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();
            GL.Viewport(0, 0, GLWindow.Width, GLWindow.Height);
            GL.Perspective(90.0f, (double)Width / (double)Height, 1, 1500000);
            GL.MatrixMode(OpenGL.GL_MODELVIEW);
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
                UpdateDraw = true;
                if (Keyboard.IsKeyDown(Key.R))
                {
                    LocalCamera.TargetHeight += MoveSpeed / 5 * Delta;
                }
                if (Keyboard.IsKeyDown(Key.F))
                {
                    LocalCamera.TargetHeight -= MoveSpeed / 5 * Delta;
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
                    LocalCamera.rotation += (MoveSpeed * Delta / 7.5);
                    if (LocalCamera.rotation < 0)
                        LocalCamera.rotation += 360;
                    if (LocalCamera.rotation > 360)
                        LocalCamera.rotation -= 360;
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    UpdateDraw = true;
                    LocalCamera.rotation -= (MoveSpeed * Delta / 7.5);
                    if (LocalCamera.rotation < 0)
                        LocalCamera.rotation += 360;
                    if (LocalCamera.rotation > 360)
                        LocalCamera.rotation -= 360;
                }
                if (Keyboard.IsKeyDown(Key.Q))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(5, LocalCamera, MoveSpeed * Delta);
                }
                if (Keyboard.IsKeyDown(Key.E))
                {
                    UpdateDraw = true;
                    TarmacGL.MoveCamera(4, LocalCamera, MoveSpeed * Delta);
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


        private void DrawScene()
        {
            
            if (CheckboxTextured.Checked)
            {


                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                
                GL.Enable(OpenGL.GL_BLEND);
                GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                GL.ShadeModel(OpenGL.GL_SMOOTH);
                GL.Enable(OpenGL.GL_COLOR_MATERIAL);
                GL.Enable(OpenGL.GL_TEXTURE_2D);
                GL.FrontFace(OpenGL.GL_CCW);
                if (TargetMode == 1)
                {
                    for (int ThisTexture = 0; ThisTexture < TextureObjects.Length; ThisTexture++)
                    {
                        if (TextureObjects[ThisTexture].textureName != null)
                        {

                            if ((RenderCheckbox.Checked) && (TextureObjects[ThisTexture].textureScreen > 0))
                            {
                                TarmacGL.DrawTextureFlushScreen(GL, GLWindow.Width, GLWindow.Height, TextureObjects[ThisTexture], GLTexture);
                                
                            }
                            else
                            {
                                TarmacGL.DrawTextureFlush(GL, TextureObjects, GLTexture, ThisTexture);
                            }
                            TarmacGL.DrawGLCull(GL, TextureObjects[ThisTexture]);

                            for (int ThisObject = 0; ThisObject < SectionList.Length; ThisObject++)
                            {
                                if (CourseModel[SectionList[ThisObject]].materialID == ThisTexture)
                                {
                                    double Add = Convert.ToDouble(((FrameTime / 33.333) * (TextureObjects[ThisTexture].textureScrollT) / 32.0) / 4.0);
                                    TextureObjects[ThisTexture].GLShiftT += Add;
                                    Add = Convert.ToDouble(((FrameTime / 33.333) * (TextureObjects[ThisTexture].textureScrollS) / 32.0) / 4.0);
                                    TextureObjects[ThisTexture].GLShiftS += Add;

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

                                    TarmacGL.DrawTexturedNoFlush(GL, TextureObjects[ThisTexture], CourseModel[SectionList[ThisObject]]);
                                    
                                        

                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int ThisTexture = 0; ThisTexture < TextureObjects.Length; ThisTexture++)
                    {
                        if (TextureObjects[ThisTexture].textureName != null)
                        {

                            if ((RenderCheckbox.Checked) && (TextureObjects[ThisTexture].textureScreen > 0))
                            {
                                TarmacGL.DrawTextureFlushScreen(GL, GLWindow.Width, GLWindow.Height, TextureObjects[ThisTexture], GLTexture);
                            }
                            else
                            {
                                TarmacGL.DrawTextureFlush(GL, TextureObjects, GLTexture, ThisTexture);
                            }
                            TarmacGL.DrawGLCull(GL, TextureObjects[ThisTexture]);

                            double Add = Convert.ToDouble(((FrameTime/ 33.333) * (TextureObjects[ThisTexture].textureScrollT) / 32.0) / 4.0);
                            TextureObjects[ThisTexture].GLShiftT += Add;
                            Add = Convert.ToDouble(((FrameTime / 33.333) * (TextureObjects[ThisTexture].textureScrollS) / 32.0) / 4.0);
                            TextureObjects[ThisTexture].GLShiftS += Add;

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


                            for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                            {
                                if (CourseModel[ThisObject].materialID == ThisTexture)
                                {
                                   
                                    
                                    TarmacGL.DrawTexturedNoFlush(GL, TextureObjects[ThisTexture], CourseModel[ThisObject]);
                                }
                            }
                        }
                    }
                }
                GL.End();
                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                GL.Enable(OpenGL.GL_BLEND);
                GLTexture.Destroy(GL);
                foreach (var Geometry in SurfaceModel)
                {
                    TarmacGL.DrawShaded(GL, GLTexture, Geometry, LocalCamera.flashRed);
                }
                if (TargetMode == 3)
                {

                    
                    for (int ThisObject = 0; ThisObject < CourseObjects.Count; ThisObject++)
                    {
                        if (ThisObject != TargetedObject)
                        {
                            if (ObjectTypes[CourseObjects[ThisObject].ObjectIndex].TextureData != null)
                            {

                                GL.Enable(OpenGL.GL_TEXTURE_2D);
                                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                                TarmacGL.DrawOKObjectTextured(GL, GLTexture, CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].ObjectIndex]);
                            }
                            else
                            {
                                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                                GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                                GL.Enable(OpenGL.GL_BLEND);
                                GLTexture.Destroy(GL);
                                TarmacGL.DrawOKObjectShaded(GL, GLTexture, CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].ObjectIndex]);
                            }
                                
                        }
                    }
                }
            }            
            else
            {
                
                
                GL.End();
                GL.Disable(OpenGL.GL_CULL_FACE);
                GL.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_FILL);
                GL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                GL.Enable(OpenGL.GL_BLEND);
                GLTexture.Destroy(GL);
                if (TargetMode == 1)
                {
                    for (int ThisObject = 0; ThisObject < SectionList.Length; ThisObject++)
                    {
                        TarmacGL.DrawShaded(GL, GLTexture, CourseModel[SectionList[ThisObject]], CourseModel[SectionList[ThisObject]].objectColor);                        
                    }
                }
                else
                {
                    for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                    {                        
                        TarmacGL.DrawShaded(GL, GLTexture, CourseModel[ThisObject], CourseModel[ThisObject].objectColor);                        
                    }
                }

                foreach (var Geometry in SurfaceModel)
                {
                    TarmacGL.DrawShaded(GL, GLTexture, Geometry, LocalCamera.flashRed);
                }
                if (TargetMode == 3)
                {
                    for (int ThisObject = 0; ThisObject < CourseObjects.Count; ThisObject++)
                    {
                        if (ThisObject != TargetedObject)
                        {
                            TarmacGL.DrawOKObjectShaded(GL, GLTexture, CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].ObjectIndex]);
                        }
                    }
                }
            }


            TarmacGL.DrawCursor(GL, LocalCamera, GLTexture);
            GL.Disable(OpenGL.GL_CULL_FACE);
            TM64_Geometry TarmacGeo = new TM64_Geometry();
            TM64_Geometry.Face[] Marker = TarmacGeo.CreateStandard(Convert.ToSingle(5.0));
            if (CheckboxPaths.Checked)
            {
                foreach (var ThisPath in PathMarker)
                {
                    foreach (var ThisMark in ThisPath.pathmarker)
                    {
                        foreach (var ThisFace in Marker)
                        {
                            TarmacGL.DrawMarker(GL, GLTexture, ThisFace, ThisMark.Color, ThisMark);
                        }
                    }
                }
            }
            
            switch (TargetMode)
            {
                case 0:
                default:
                    {
                        break;
                    }
                case 1:
                case 2:
                    {
                        if (TargetedObject != -1)
                        {
                            TarmacGL.DrawShaded(GL, GLTexture, CourseModel[TargetedObject], LocalCamera.flashWhite);
                        }
                        break;
                    }
                case 3:
                    {
                        if (TargetedObject != -1)
                        {                            
                            TarmacGL.DrawOKObjectShaded(GL, GLTexture, CourseObjects[TargetedObject], ObjectTypes[CourseObjects[TargetedObject].ObjectIndex], LocalCamera.flashWhite);
                        }                        
                        break;
                    }
            }

        }

        private void GLWindow_OpenGLDraw(object sender, RenderEventArgs args)
        {

            LocalCamera.flashWhite = TarmacGL.GetAlphaFlash(LocalCamera.flashWhite, (FrameTime / 33.333));
            LocalCamera.flashRed = TarmacGL.GetAlphaFlash(LocalCamera.flashRed, (FrameTime / 33.333));
            LocalCamera.flashYellow = TarmacGL.GetAlphaFlash(LocalCamera.flashYellow, (FrameTime / 33.333));




            if (GLWindow.ClientRectangle.Contains(GLWindow.PointToClient(Control.MousePosition)))
            {
                UpdateDraw = true;
                if (Parent.ContainsFocus)
                {
                    CheckInput();
                }
                
            }
            
            SpeedBox.Text = MoveSpeed.ToString() + ".0";


            FrameTime = FrameWatch.ElapsedMilliseconds;
            FPS = Math.Round(1000.0 / FrameTime, 1);
            FrameWatch.Restart();

            if ((UpdateDraw) || (AntiFlicker))
            {
                GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                GL.LoadIdentity();

                AntiFlicker = UpdateDraw;
                UpdateDraw = true;

                RefreshView();




                if (FPSStall > 10)
                {
                    FPSStall = 0;
                    string PrintString = "FPS-" + FPS.ToString();
                    FPSDisplay.Text = PrintString;
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
                GL.Flush();

                


                UpdateDraw = false;
            }
            
                
            
            
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
            
            
        }


        private void GLWindow_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UpdateDraw = true;
            
            
            if ( (TargetedObject != -1) && (TargetMode == 1) )
            {

                //Section List
                if (e.Button == MouseButtons.Right)
                {
                    List<int> TempList = SectionList.ToList();
                    if (TempList.Contains(TargetedObject))
                    {
                        TempList.Remove(TargetedObject);
                    }
                    else
                    {
                        TempList.Add(TargetedObject);
                    }
                    SectionList = TempList.ToArray();

                    RequestMode = 1;
                }
                else
                {
                    SelectedObject = TargetedObject;

                    RequestMode = 2;
                }
            }

            if (TargetMode == 3)
            {
                // Custom Objects
                SelectedObject = -1;
                if (e.Button == MouseButtons.Right)
                {
                    TM64_Course.OKObject ThisObject = TarmacCourse.NewOKObject();
                    ThisObject.OriginPosition = new short[] { Convert.ToInt16(LocalCamera.marker.X), Convert.ToInt16(LocalCamera.marker.Y), Convert.ToInt16(LocalCamera.marker.Z + 3) };
                    ThisObject.ObjectIndex = Convert.ToInt16(OKObjectIndex);
                    CourseObjects.Add(ThisObject);
                    RequestMode = 1;
                }
                else if ((Keyboard.IsKeyDown(Key.LeftShift)) && (OKSelectedObject != -1))
                {
                    CourseObjects[OKSelectedObject].OriginPosition = new short[] { Convert.ToInt16(LocalCamera.marker.X), Convert.ToInt16(LocalCamera.marker.Y), Convert.ToInt16(LocalCamera.marker.Z) };
                    RequestMode = 2;
                }
                else
                {
                    SelectedObject = TargetedObject;
                    RequestMode = 3;
                }
            }

            if (UpdateParent != null)
            {
                UpdateParent(this, EventArgs.Empty);
            }
        }

        private void GLWindow_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            UpdateDraw = true;
            RefreshView();
            
            double[] pointA = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 0);
            double[] pointB = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 1);
            Vector3D RayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));             
            Vector3D RayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2]));
            Vector3D Intersection = new Vector3D(0, 0, 0); 

            

            float objectDistance = -1;
            TargetedObject = -1;
            SelectedObject = -1;

            switch (TargetMode)
            {
                case 0:
                default:
                    {
                        break;
                    }
                case 1:
                    {
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
                                        TargetedObject = SectionList[ThisObject];
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
                                        TargetedObject = ThisObject;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        // Surface Model
                        for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                        {
                            if (MouseTest(out Intersection, CourseModel[ThisObject], RayOrigin, RayTarget))
                            {
                                if ((objectDistance == -1) || (objectDistance > Intersection.X))
                                {
                                    objectDistance = Intersection.X;
                                    TargetedObject = ThisObject;
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        // Custom Objects
                        for (int ThisList = 0; ThisList < CourseObjects.Count; ThisList++)
                        {
                            float Scale = ObjectTypes[CourseObjects[ThisList].ObjectIndex].ModelScale;
                            for (int ThisObject = 0; ThisObject < ObjectTypes[CourseObjects[ThisList].ObjectIndex].ModelData.Length; ThisObject++)
                            {
                                float[] OriginPosition = { Convert.ToSingle(CourseObjects[ThisList].OriginPosition[0]), Convert.ToSingle(CourseObjects[ThisList].OriginPosition[1]), Convert.ToSingle(CourseObjects[ThisList].OriginPosition[2]) };
                                float[] ModelScales = { Scale, Scale, Scale };
                                if (MouseTestOrigin(out Intersection, ObjectTypes[CourseObjects[ThisList].ObjectIndex].ModelData[ThisObject], RayOrigin, RayTarget, OriginPosition, ModelScales))
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
            }

            if (objectDistance != -1)
            {
               LocalCamera.marker = new Assimp.Vector3D() { X = RayOrigin.X + (RayTarget.X * objectDistance), Y = RayOrigin.Y + (RayTarget.Y * objectDistance), Z = RayOrigin.Z + (RayTarget.Z * objectDistance) };
                
            }
        }
    }
}
