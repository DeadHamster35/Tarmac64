using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AssimpSharp;
using AssimpSharp.FBX;
using SharpDX.Direct3D11;

namespace AssimpView
{
    static class FbxMeshLoader
    {
        public static List<FbxMesh> Load(string path, Device device, out Scene scene)
        {

            byte[] input;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                input = new byte[stream.Length];
                stream.Read(input, 0, (int)stream.Length);
            }
            bool isBinary = false;
            List<Token> tokens;
            if (Encoding.ASCII.GetString(input, 0, 18) == "Kaydara FBX Binary")
            {
                isBinary = true;
                BinaryTokenizer.TokenizeBinary(out tokens, input, input.Length);
            }
            else
            {
                Tokenizer.Tokenize(out tokens, input);
            }
            var parser = new Parser(tokens, isBinary);
            var settings = ImporterSettings.Default;
            var doc = new Document(parser, settings);
            FbxConverter.ConvertToScene(out scene, doc);
            var result = new List<FbxMesh>();
            foreach (var assimpMesh in scene.Meshes)
            {
                var mat = scene.Materials[assimpMesh.MaterialIndex];
                var fbxMesh = new FbxMesh(assimpMesh, mat, device, path);
                result.Add(fbxMesh);
            }
            return result;
        }
    }
}
