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
    /// A simple cube of side TWO
    /// </summary>
    public class Cube3D : Shape3D
    {
        public Cube3D() :
            base("Cube")
        { }

        protected override List<Vertex> MakeVertices()
        {
            var list = new List<Vertex>();

            Point3D[] points = {new Point3D(-1, -1, -1), new Point3D(-1, -1, 1),new Point3D(-1, 1, -1),new Point3D(-1, 1, 1),
                new Point3D(1, -1, -1),new Point3D(1, -1, 1),new Point3D(1, 1, -1),new Point3D(1, 1, 1) };

            var xPos = new Vector3D(1, 0, 0);
            var xNeg = new Vector3D(1, 0, 0);
            var yPos = new Vector3D(0, 1, 0);
            var yNeg = new Vector3D(0, 1, 0);
            var zPos = new Vector3D(0, 0, 1);
            var zNeg = new Vector3D(0, 0, 1);

            MakeSquare(ref list, points[0], points[2], points[6], points[4], zNeg); // bottom
            MakeSquare(ref list, points[1], points[5], points[7], points[3], zPos); // top
            MakeSquare(ref list, points[3], points[2], points[0], points[1], xNeg); // left
            MakeSquare(ref list, points[5], points[4], points[6], points[7], xPos); // right
            MakeSquare(ref list, points[1], points[0], points[4], points[5], yNeg); // front
            MakeSquare(ref list, points[7], points[6], points[2], points[3], yPos); // back

            return list;
        }

        /// <summary>
        /// Creates a single square from four points
        /// Points should be wound in counterclockwise direction
        /// </summary>
        /// <param name="vertices">The current list of vertices</param>
        /// <param name="normal">The direction of the normal</param>
        private void MakeSquare(ref List<Vertex> vertices, Point3D one, Point3D two, Point3D three, Point3D four, Vector3D normal)
        {
            var ur = new Point(0, 0);
            var lr = new Point(0, 1);
            var ul = new Point(1, 0);
            var ll = new Point(1, 1);

            vertices.Add(new Vertex(one, normal, ur));
            vertices.Add(new Vertex(two, normal, lr));
            vertices.Add(new Vertex(three, normal, ll));
            vertices.Add(new Vertex(four, normal, ul));
        }

        protected override Int32Collection MakeTriangles()
        {
            var list = new Int32Collection();

            int[] baseNum = { 0, 1, 2, 0, 2, 3 };

            // If the vertices are put in sensibly, these triangles make themselves easily
            for (int i = 0; i < 6; ++i)
            {
                foreach (int num in baseNum)
                {
                    list.Add(num + i * 4);
                }
            }

            return list;
        }


    }
}
