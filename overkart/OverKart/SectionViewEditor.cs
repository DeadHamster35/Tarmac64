using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OverKart64;
using AssimpSharp;

namespace OverKart64
{
    public partial class SectionViewEditor : Form
    {
        public SectionViewEditor()
        {
            InitializeComponent();
        }


        OK64 mk = new OK64();
        //

        OpenFileDialog vertopen = new OpenFileDialog();
        SaveFileDialog vertsave = new SaveFileDialog();
        FolderBrowserDialog textsave = new FolderBrowserDialog();

        string[] viewstrings = { "North", "East", "South", "West" };
        AssimpSharp.Scene fbx = new AssimpSharp.Scene();

        AssimpSharp.FBX.FBXImporter assimpSharpImporter = new AssimpSharp.FBX.FBXImporter();
        


        //
        OK64.SectionView[] course = new OK64.SectionView[0]; //will be resized when loaded from OK64.LoadSVL();
        int section_count = 0;


        bool loaded = false;


        private void Loadbtn_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {

                string rawmodel = vertopen.FileName;
                fbx = new AssimpSharp.Scene();
                fbx = assimpSharpImporter.ReadFile(rawmodel);

                List<string> objectlist = new List<string>();
                objectlistbox.Items.Clear();

                var parentobj = fbx.RootNode.FindNode("Course Master Objects");
                foreach (var child in parentobj.Children)
                {
                    objectlist.Add(child.Name);

                }
                string[] objectarray = objectlist.ToArray();
                Array.Sort(objectarray);
                foreach (var piece in objectarray)
                {
                    objectlistbox.Items.Add(piece);
                }

                svlload.Enabled = true;
                addbtn.Enabled = true;
                removebtn.Enabled = true;
                sectionbox.Enabled = true;
                viewbox.Enabled = true;
                updatelistbtn.Enabled = true;
                exportbtn.Enabled = true;
            }



        }

        private void Svlload_Click(object sender, EventArgs e)
        {
            if (vertopen.ShowDialog() == DialogResult.OK)
            {


                string inputSVL = vertopen.FileName;

                course = mk.LoadSVL(inputSVL);

                section_count = course.Length;

                for (int current_section = 1; current_section <= section_count; current_section++)
                {
                    sectionbox.Items.Add("Section " + current_section.ToString());    
                }
                viewbox.SelectedIndex = 0;
                
                loaded = true;
                sectionbox.SelectedIndex = 0;

            }
        }



        private void UncheckAllItems()
        {
            while (objectlistbox.CheckedIndices.Count > 0)
                objectlistbox.SetItemChecked(objectlistbox.CheckedIndices[0], false);
        }

        private void UpdateList()
        {
            if (loaded == true)
            {
                

                UncheckAllItems();

                int object_count = course[sectionbox.SelectedIndex].viewlist[viewbox.SelectedIndex].objectlist.Length;

                for (int current_object = 0; current_object < object_count; current_object++)
                {
                    string object_name = course[sectionbox.SelectedIndex].viewlist[viewbox.SelectedIndex].objectlist[current_object];
                    int object_index = objectlistbox.Items.IndexOf(object_name);
                    if (object_index == -1)
                    {
                        MessageBox.Show("Missing Object! " + object_name);
                    }
                    else
                    {
                        objectlistbox.SetItemChecked(object_index, true);
                    }
                }

                sectiondisplay.Text = sectionbox.Items[sectionbox.SelectedIndex].ToString() + "-" + viewbox.Items[viewbox.SelectedIndex].ToString();
                countdisplay.Text = objectlistbox.CheckedItems.Count.ToString()+" Objects";

                int facecount = 0;
                int vertcount = 0;
                foreach (var checkedobject in objectlistbox.CheckedItems)
                {
                    var object_node = fbx.RootNode.FindNode(checkedobject.ToString());

                    foreach (var mesh in object_node.Meshes)
                    {
                        var current_subobject = fbx.Meshes[mesh];
                        facecount = facecount + current_subobject.NumFaces;





                        List<OK64.Vertex> RealVerts = new List<OK64.Vertex>();
                        int[] local_index = new int[current_subobject.NumVertices];


                        for (int count_local = 0; count_local < current_subobject.NumVertices; count_local++)
                        {
                            short local_x = Convert.ToInt16(current_subobject.Vertices[count_local].X);
                            short local_y = Convert.ToInt16(current_subobject.Vertices[count_local].Y);
                            short local_z = Convert.ToInt16(current_subobject.Vertices[count_local].Z);
                            float local_u = Convert.ToSingle(current_subobject.TextureCoords[0][count_local][0]);
                            float local_v = Convert.ToSingle(current_subobject.TextureCoords[0][count_local][1]);
                            if (RealVerts.Count == 0)
                            {
                                RealVerts.Add(new OK64.Vertex { });
                                int count_addedindex = RealVerts.Count - 1;
                                RealVerts[count_addedindex].position = new OK64.Position { };

                                RealVerts[count_addedindex].position.x = local_x;
                                RealVerts[count_addedindex].position.y = local_y;
                                RealVerts[count_addedindex].position.z = local_z;

                                local_index[count_local] = RealVerts.Count - 1;
                                //MessageBox.Show("No Match");
                            }
                            else
                            {

                                bool match = false;
                                for (int count_realvert = 0; count_realvert < RealVerts.Count; count_realvert++)
                                {
                                    if ((local_x == RealVerts[count_realvert].position.x) & (local_y == RealVerts[count_realvert].position.y) & (local_z == RealVerts[count_realvert].position.z) & (local_u == RealVerts[count_realvert].position.u) & (local_v == RealVerts[count_realvert].position.v))
                                    {
                                        local_index[count_local] = count_realvert;
                                        match = true;
                                        //MessageBox.Show("Match");
                                    }
                                }

                                if (match == false)
                                {
                                    RealVerts.Add(new OK64.Vertex { });
                                    int count_addedindex = RealVerts.Count - 1;
                                    RealVerts[count_addedindex].position = new OK64.Position { };

                                    RealVerts[count_addedindex].position.x = local_x;
                                    RealVerts[count_addedindex].position.y = local_y;
                                    RealVerts[count_addedindex].position.z = local_z;
                                    RealVerts[count_addedindex].position.u = local_u;
                                    RealVerts[count_addedindex].position.v = local_v;


                                    local_index[count_local] = RealVerts.Count - 1;
                                    //MessageBox.Show("No Match");
                                }
                            }
                        }




                        vertcount = vertcount + RealVerts.Count;
                    }

                }

                facedisplay.Text = facecount.ToString() + " Faces";
                vertdisplay.Text = vertcount.ToString() + " Verts";
            }
        }




        private void Sectionbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void Viewbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void updatebtn(object sender, EventArgs e)
        {
            course[sectionbox.SelectedIndex].viewlist[viewbox.SelectedIndex].objectlist = new string[objectlistbox.CheckedItems.Count];

            var objectlist = course[sectionbox.SelectedIndex].viewlist[viewbox.SelectedIndex].objectlist;
            for (int current_item = 0; current_item < objectlistbox.CheckedItems.Count; current_item++)
            {
                objectlist[current_item] = objectlistbox.CheckedItems[current_item].ToString();
            }
            UpdateList();
        }

        private void Exportbtn_Click(object sender, EventArgs e)
        {
            if (vertsave.ShowDialog() == DialogResult.OK)
            {
                string savePath = vertsave.FileName;
                System.IO.File.AppendAllText(savePath, section_count + Environment.NewLine);

                for (int section = 0; section < section_count; section++)
                {
                    for (int view = 0; view < 4; view++)
                    {
                        System.IO.File.AppendAllText(savePath, "Section " + (section + 1).ToString() +" "+viewstrings[view] + Environment.NewLine);

                        int object_count = course[section].viewlist[view].objectlist.Count();
                        System.IO.File.AppendAllText(savePath, object_count.ToString() + Environment.NewLine);
                        for (int object_index = 0; object_index < object_count; object_index++)
                        {
                            string object_name = course[section].viewlist[view].objectlist[object_index];
                            System.IO.File.AppendAllText(savePath, object_name + Environment.NewLine);
                        }
                    }
                }
            }
        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            
            
            Array.Resize(ref course, section_count+1);
            course[section_count] = new OK64.SectionView();
            course[section_count].viewlist = new OK64.ViewList[4];
            for (int view = 0; view < 4; view++)
            {
                course[section_count].viewlist[view] = new OK64.ViewList();
                course[section_count].viewlist[view].objectlist = new string[0];

            }
            section_count++;
            sectionbox.Items.Add("Section " + section_count.ToString());
            viewbox.SelectedIndex = 0;
            loaded = true;
            sectionbox.SelectedIndex = section_count - 1;
        }

        private void Removebtn_Click(object sender, EventArgs e)
        {
            List<OK64.SectionView> courselist = course.ToList();
            int removeindex = sectionbox.SelectedIndex;
            courselist.RemoveAt(removeindex);
            course = courselist.ToArray();

            section_count = section_count - 1;
            sectionbox.Items.Clear();
            for (int current_section = 1; current_section <= section_count; current_section++)
            {
                sectionbox.Items.Add("Section " + current_section.ToString());
            }
            viewbox.SelectedIndex = 0;

            loaded = true;
            sectionbox.SelectedIndex = removeindex - 1;



            
            

        }
    }
}
