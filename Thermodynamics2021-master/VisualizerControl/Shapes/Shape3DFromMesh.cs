using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace VisualizerControl.Shapes
{
    internal class Shape3DFromMesh : Shape3D
    {
        internal Shape3DFromMesh(string name, bool freezeMesh) :
            base(name, freezeMesh)
        { }

        // These should never be called if this class is used properly
        protected override Int32Collection MakeTriangles()
        {
            throw new NotImplementedException();
        }

        protected override List<Vertex> MakeVertices()
        {
            throw new NotImplementedException();
        }
    }
}
