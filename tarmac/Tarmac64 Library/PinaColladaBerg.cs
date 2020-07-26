using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collada141;

namespace Tarmac64_Library
{
    public class CoconutePete
    {
    }

    public class ColladaData
    {
        public ColladaSceneNode[] SceneCollection { get; set; }

    }

   


    public class ColladaSceneNode
    {
        public ColladaSceneNode ChildScene { get; set; }
    }

    

}
