using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl.Shapes
{
    /// <summary>
    /// A cylinder with caps
    /// </summary>
    public class Cylinder3D : Shape3D
    {
        static public int NSegments { get; set; } = 16;

        public Cylinder3D() :
            base("Cylinder")
        {
        }

        protected override List<Vertex> MakeVertices()
        {
            // CylinderFactory does all the work
            return CylinderFactory.MakeVertices(true, NSegments);
        }

        protected override Int32Collection MakeTriangles()
        {
            return CylinderFactory.MakeTriangles(true, NSegments);
        }
    }
}
