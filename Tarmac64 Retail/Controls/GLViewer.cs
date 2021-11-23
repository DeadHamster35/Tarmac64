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

        public TM64_GL.TMCamera LocalCamera = new TM64_GL.TMCamera();
        public int TargetMode = 0;

        long FrameTime;
        Stopwatch FrameWatch = new Stopwatch();


        public TM64_Paths.Pathlist[] PathMarker = new TM64_Paths.Pathlist[0];

        public TM64_Geometry.OK64F3DObject[] CourseModel = new TM64_Geometry.OK64F3DObject[0];        
        public TM64_Geometry.OK64F3DObject[] SurfaceModel = new TM64_Geometry.OK64F3DObject[0];
        public List<TM64_Course.OKObject> CourseObjects = new List<TM64_Course.OKObject>();
        public TM64_Course.OKObjectType[] ObjectTypes = new TM64_Course.OKObjectType[0]; 

        public TM64_Geometry.OK64Texture[] TextureObjects = new TM64_Geometry.OK64Texture[0];
        public int[] SectionList = new int[0];

        public OpenGL GL = new OpenGL();
        public SharpGL.SceneGraph.Assets.Texture GLTexture = new SharpGL.SceneGraph.Assets.Texture();

        public bool UpdateDraw = false;

        public event EventHandler UpdateParent;

        public int TargetedObject, SelectedObject, RequestMode, OKObjectIndex, OKSelectedObject = -1;

        

        public void RefreshView()
        {
            GL.MatrixMode(OpenGL.GL_PROJECTION);
            GL.LoadIdentity();
            GL.Perspective(90.0f, (double)Width / (double)Height, 0.01, 15000);
            GL.MatrixMode(OpenGL.GL_MODELVIEW);
        }
        private void GLWindow_Resized(object sender, EventArgs e)
        {
            RefreshView();
            UpdateDraw = true;
        }


        private void DrawScene()
        {

            if (CheckboxTextured.Checked)
            {
                if (TargetMode == 1)
                {
                    for (int ThisTexture = 0; ThisTexture < TextureObjects.Length; ThisTexture++)
                    {
                        if (TextureObjects[ThisTexture].textureName != null)
                        {
                            TarmacGL.DrawTextureFlush(GL, TextureObjects, GLTexture, ThisTexture);
                            for (int ThisObject = 0; ThisObject < SectionList.Length; ThisObject++)
                            {
                                if (CourseModel[SectionList[ThisObject]].materialID == ThisTexture)
                                {
                                    TarmacGL.DrawTexturedNoFlush(GL, TextureObjects, GLTexture, CourseModel[SectionList[ThisObject]]);
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
                            TarmacGL.DrawTextureFlush(GL, TextureObjects, GLTexture, ThisTexture);
                            for (int ThisObject = 0; ThisObject < CourseModel.Length; ThisObject++)
                            {
                                if (CourseModel[ThisObject].materialID == ThisTexture)
                                {
                                    TarmacGL.DrawTexturedNoFlush(GL, TextureObjects, GLTexture, CourseModel[ThisObject]);
                                }
                            }
                        }
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
                            if (ObjectTypes[CourseObjects[ThisObject].ObjectIndex].TextureData != null)
                            {
                                TarmacGL.DrawOKObjectTextured(GL, GLTexture, CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].ObjectIndex]);
                            }
                            else
                            {
                                TarmacGL.DrawOKObjectShaded(GL, GLTexture, CourseObjects[ThisObject], ObjectTypes[CourseObjects[ThisObject].ObjectIndex]);
                            }
                                
                        }
                    }
                }
            }            
            else
            {
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

            TM64_Geometry TarmacGeo = new TM64_Geometry();
            TM64_Geometry.Face[] Marker = TarmacGeo.CreateStandard(Convert.ToSingle(5.0));
            foreach (var ThisPath in PathMarker)
            {
                foreach (var ThisMark in ThisPath.pathmarker)
                {
                    foreach (var ThisFace in Marker)
                    {
                        float[] Position = new float[3] { Convert.ToSingle(ThisMark.xval), Convert.ToSingle(ThisMark.yval), Convert.ToSingle(ThisMark.zval) };
                        TarmacGL.DrawMarker(GL, GLTexture, ThisFace, ThisMark.Color, Position);
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
            
            LocalCamera.flashWhite = TarmacGL.GetAlphaFlash(LocalCamera.flashWhite);
            LocalCamera.flashRed = TarmacGL.GetAlphaFlash(LocalCamera.flashRed);
            LocalCamera.flashYellow = TarmacGL.GetAlphaFlash(LocalCamera.flashYellow);

            

            FrameWatch.Restart();


            double FPS = 0;
            FPS = Math.Round(1000.0 / FrameTime, 1);
            string PrintString = "FPS-" + FPS.ToString();
            

            if (GLWindow.ClientRectangle.Contains(GLWindow.PointToClient(Control.MousePosition)))
            {
                UpdateDraw = true;
            }
            
            if (UpdateDraw)
            {

                GL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                GL.LoadIdentity();
                GL.LookAt(LocalCamera.position.X, LocalCamera.position.Y, LocalCamera.position.Z, LocalCamera.target.X, LocalCamera.target.Y, LocalCamera.target.Z, 0, 0, 1);
                GL.DrawText(5, GLWindow.Height - 11, 1.0f, 0.0f, 0.0f, "Arial", 10.0f, PrintString);
                DrawScene();
                
                UpdateDraw = false;

                GL.End();
                GL.Flush();
            }



            FrameTime = FrameWatch.ElapsedMilliseconds;
            
            
        }






        private void GLWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            UpdateDraw = true;

            long OldTime = DateTime.Now.Ticks;
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (Keyboard.IsKeyDown(Key.R))
                {
                    LocalCamera.TargetHeight += MoveSpeed / 5;
                }
                if (Keyboard.IsKeyDown(Key.F))
                {
                    LocalCamera.TargetHeight -= MoveSpeed / 5;
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.W))
                {
                    TarmacGL.MoveCamera(0, LocalCamera, MoveSpeed);
                }
                if (Keyboard.IsKeyDown(Key.S))
                {
                    TarmacGL.MoveCamera(1, LocalCamera, MoveSpeed);
                }
                if (Keyboard.IsKeyDown(Key.A))
                {
                    LocalCamera.rotation += (MoveSpeed / 5);
                    if (LocalCamera.rotation < 0)
                        LocalCamera.rotation += 360;
                    if (LocalCamera.rotation > 360)
                        LocalCamera.rotation -= 360;
                }
                if (Keyboard.IsKeyDown(Key.D))
                {
                    LocalCamera.rotation -= (MoveSpeed / 5);
                    if (LocalCamera.rotation < 0)
                        LocalCamera.rotation += 360;
                    if (LocalCamera.rotation > 360)
                        LocalCamera.rotation -= 360;
                }
                if (Keyboard.IsKeyDown(Key.Q))
                {
                    TarmacGL.MoveCamera(5, LocalCamera, MoveSpeed);
                }
                if (Keyboard.IsKeyDown(Key.E))
                {
                    TarmacGL.MoveCamera(4, LocalCamera, MoveSpeed);
                }
                if (Keyboard.IsKeyDown(Key.R))
                {
                    TarmacGL.MoveCamera(2, LocalCamera, MoveSpeed);
                }
                if (Keyboard.IsKeyDown(Key.F))
                {
                    TarmacGL.MoveCamera(3, LocalCamera, MoveSpeed);
                }
                if (Keyboard.IsKeyDown(Key.T))
                {
                    MoveSpeed += 5;
                }
                if (Keyboard.IsKeyDown(Key.G))
                {
                    MoveSpeed -= 5;
                }

                
            }

            TarmacGL.UpdateTarget(LocalCamera);
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

        private void GLViewer_Load(object sender, EventArgs e)
        {
            GL = GLWindow.OpenGL;
            RefreshView();

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
                    ThisObject.OriginPosition = new short[] { Convert.ToInt16(LocalCamera.marker[0]), Convert.ToInt16(LocalCamera.marker[1]), Convert.ToInt16(LocalCamera.marker[2] + 3) };
                    ThisObject.ObjectIndex = Convert.ToInt16(OKObjectIndex);
                    CourseObjects.Add(ThisObject);
                    RequestMode = 1;
                }
                else if ((Keyboard.IsKeyDown(Key.LeftShift)) && (OKSelectedObject != -1))
                {
                    CourseObjects[OKSelectedObject].OriginPosition = new short[] { Convert.ToInt16(LocalCamera.marker[0]), Convert.ToInt16(LocalCamera.marker[1]), Convert.ToInt16(LocalCamera.marker[2]) };
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


            double[] pointA = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 0);
            double[] pointB = GL.UnProject(e.Location.X, GLWindow.Height - e.Location.Y, 1);

            Vector3D RayOrigin = new Vector3D(Convert.ToSingle(pointA[0]), Convert.ToSingle(pointA[1]), Convert.ToSingle(pointA[2]));
            Vector3D RayTarget = new Vector3D(Convert.ToSingle(pointB[0]), Convert.ToSingle(pointB[1]), Convert.ToSingle(pointB[2]));
            Vector3D Intersection = new Vector3D(0, 0, 0);


            float objectDistance = -1;
            TargetedObject = -1;
            SelectedObject = -1;
            TM64_Geometry tmGeo = new TM64_Geometry();

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
                                        DistanceBox.Text = objectDistance.ToString();
                                        TargetedObject = SectionList[ThisObject];
                                        TargetBox.Text = TargetedObject.ToString();
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
                                        DistanceBox.Text = objectDistance.ToString();
                                        TargetedObject = ThisObject;
                                        TargetBox.Text = TargetedObject.ToString();
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
                                    DistanceBox.Text = objectDistance.ToString();
                                    TargetedObject = ThisObject;
                                    TargetBox.Text = TargetedObject.ToString();
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
                                        TargetBox.Text = TargetedObject.ToString();
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
                                    DistanceBox.Text = objectDistance.ToString();
                                }
                            }
                        }
                        break;
                    }
            }

            if (objectDistance != -1)
            {
                LocalCamera.marker = new Vector3D(RayOrigin.X + (RayTarget.X * objectDistance), RayOrigin.Y + (RayTarget.Y * objectDistance), RayOrigin.Z + (RayTarget.Z * objectDistance));
            }

        }
    }
}
