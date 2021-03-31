using Geometry.Geometry3D;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace VisualizerControl.Shapes
{
    public class Tetrahedron3D : TriangleBasedShape3D
    {
        public Tetrahedron3D(Vector3D point1, Vector3D point2, Vector3D point3, Vector3D point4) :
            base(new List<Triangle3D>() { new Triangle3D(point1, point2, point3),
                new Triangle3D(point1, point2, point4),
                new Triangle3D(point1, point3, point4),
                new Triangle3D(point2, point3, point4)})
        {
        }
    }
}
