using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VisualizerControl.Shapes
{
    /// <summary>
    /// A cylinder with no caps at the end
    /// </summary>
    public class CaplessCylinder3D : Shape3D
    {
        /// <summary>
        /// The number of segments to construct the sides of the cylinder
        /// </summary>
        static public int NSegments { get; set; } = 16;

        public CaplessCylinder3D() :
            base("Capless cylinder")
        {
        }

        protected override List<Vertex> MakeVertices()
        {
            return CylinderFactory.MakeVertices(false, NSegments);
        }

        protected override Int32Collection MakeTriangles()
        {
            return CylinderFactory.MakeTriangles(false, NSegments);
        }
    }
}
