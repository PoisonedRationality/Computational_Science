using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl.Shapes
{
    public class Quadrilateral3D : TriangleBasedShape3D
    {
        public Quadrilateral3D(Vector3D p1, Vector3D p2, Vector3D p3, Vector3D p4) :
            base(new List<Vector3D>() { p1, p2, p3, p2, p3, p4 })
        { }

    }
}
