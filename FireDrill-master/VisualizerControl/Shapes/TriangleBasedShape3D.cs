using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace VisualizerControl.Shapes
{
    /// <summary>
    /// A shape made out of a hard-edged set of triangles
    /// </summary>
    public class TriangleBasedShape3D : Shape3D
    {
        static private int counter = 0;
        private readonly List<Vector3D> trianglePoints;

        /// <summary>
        /// Creates a unique name for counting;
        /// </summary>
        /// <returns></returns>
        static private string MakeName()
        {
            return "TriangleBasedShape3D" + counter++;
        }

        /// <summary>
        /// Converts a list of Triangle3Ds to a list of vertices for the constructor
        /// </summary>
        static private List<Vector3D> ExtractVertices(List<Triangle3D> triangles)
        {
            var newList = new List<Vector3D>();

            triangles.ForEach((x) => newList.AddRange(x.Points));

            return newList;
        }

        /// <param name="vectors">An array of all the triangle vertices. Must be arranged in groups of three.</param>
        /// <param name="freezeMesh">Whether to the mesh can be safely frozen for performance reasons</param>
        public TriangleBasedShape3D(List<Vector3D> vectors, bool freezeMesh = true) :
            base(MakeName(), freezeMesh)
        {
            // Make sure it can be interpreted as an array of triangles
            if (vectors.Count % 3 != 0)
                throw new ArgumentException("Illegal list of vectors passed to TriangleBasedShape3D constructor!");

            trianglePoints = vectors;
        }

        /// <param name="triangles">A list of Triangle3Ds that will make up the shape</param>
        /// <param name="freezeMesh"></param>
        public TriangleBasedShape3D(List<Triangle3D> triangles, bool freezeMesh = true) :
            this(ExtractVertices(triangles), freezeMesh)
        { }

        protected override Int32Collection MakeTriangles()
        {
            var result = new Int32Collection();

            for (int i = 0; i < trianglePoints.Count; i += 3)
            {
                // The front side
                result.Add(i);
                result.Add(i + 1);
                result.Add(i + 2);
                // The back side
                result.Add(i);
                result.Add(i + 2);
                result.Add(i + 1);
            }
            return result;
        }

        protected override List<Vertex> MakeVertices()
        {
            var result = new List<Vertex>();

            for (int i = 0; i < trianglePoints.Count; i += 3)
            {
                result.AddRange(Triangle3D.MakeVerticesForTriangle(trianglePoints[i], trianglePoints[i + 1], trianglePoints[i + 2]));
            }

            return result;
        }
    }
}
