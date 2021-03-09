using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static WPFUtility.UtilityFunctions;
using WPFUtility;

namespace VisualizerControl.Shapes
{
    /// <summary>
    /// A simple sphere
    /// </summary>
    public class Sphere3D : Shape3D
    {
        public int NSubdivisions { get; }

        public Sphere3D(int subdivisions = 2) :
            base("Sphere" + subdivisions)
        {
            NSubdivisions = subdivisions;
        }

        /// <summary>
        /// The triangles need to be carried over from MakeVertices() to MakeTriangles().  
        /// Implemented as a private map to save memory.
        /// </summary>
        static private Dictionary<int, List<Triangle>> Triangles = new Dictionary<int, List<Triangle>>();

        /// <summary>
        /// Creates the vertices of an icosahedron with vertices at (0, 0, 1) and (0, 0, -1)
        /// </summary>
        private List<Vertex> MakeIcosahedronVertices()
        {
            var vertices = new List<Vertex>
            {
                new Vertex(new Point3D(0, 0, 1), new Vector3D(0, 0, 1), new Point(0, 0)),
                new Vertex(new Point3D(0, 0, -1), new Vector3D(0, 0, -1), new Point(1, 1))
            };

            double thetaHi = Math.PI / 2 - Math.Atan(.5);
            double thetaLo = Math.PI / 2 + Math.Atan(.5);
            const double deltaPhi = 2 * Math.PI / 10;
            bool alternator = true;

            for (double iphi = 0; iphi < Math.PI * 2 + deltaPhi / 2; iphi += deltaPhi)
                // The addition to the cutoff avoids roundoff issues
            {
                double theta = alternator ? thetaHi : thetaLo;
                Vector3D position = Vector3DExtensions.SphericalCoordinates(1, iphi, theta);
                double u = iphi / (2 * Math.PI);
                double v = theta / (Math.PI);
                vertices.Add(new Vertex(ConvertToPoint3D(position), position, new Point(u, v)));

                // Switch the alternator
                alternator = alternator ? false : true;
            }

           return vertices;
        }

        /// <summary>
        /// A simple triangle that remembers its own vertex references and subdivides itself
        /// </summary>
        private struct Triangle
        {
            public int V1 { get; }
            public int V2 { get; }
            public int V3 { get; }

            /// <summary>
            /// Vertices must be given in counterclockwise rounding order
            /// </summary>
            public Triangle (int vertex1, int vertex2, int vertex3)
            {
                V1 = vertex1;
                V2 = vertex2;
                V3 = vertex3;
            }

            /// <summary>
            /// Finds the midpoint between two points on the unit sphere
            /// </summary>
            private Vertex CreateMidpoint(Vertex one, Vertex two)
            {
                Vector3D midpoint = Vector3DExtensions.Midpoint(ConvertToVector3D(one.Position), ConvertToVector3D(two.Position));
                double u = midpoint.Azimuthal() / (2 * Math.PI);
                double v = midpoint.Polar() / Math.PI;
                midpoint.Normalize();
                return new Vertex(ConvertToPoint3D(midpoint), midpoint, new Point(u, v));
            }

            /// <summary>
            /// Subdivides each triangles into four triangles, triforce-style
            /// </summary>
            /// <param name="vertices">The vertices, which will be added to by this operation</param>
            /// <returns>The new list of triangles, which should replace the old</returns>
            public List<Triangle> Subdivide (ref List<Vertex> vertices)
            {
                int v12 = vertices.Count;
                vertices.Add(CreateMidpoint(vertices[V1], vertices[V2]));
                int v13 = vertices.Count;
                vertices.Add(CreateMidpoint(vertices[V1], vertices[V3]));
                int v23 = vertices.Count;
                vertices.Add(CreateMidpoint(vertices[V2], vertices[V3]));

                return new List<Triangle>()
                {
                    new Triangle(V1, v12, v13),
                    new Triangle(V2, v23, v12),
                    new Triangle(V3, v13, v23),
                    new Triangle(v12, v23, v13)
                };
            }
        }

        /// <summary>
        /// Subdivides all triangles into four triangles, triforce-style
        /// </summary>
        private List<Triangle> SubdivideTriangles(List<Triangle> original, ref List<Vertex> vertices)
        {
            var newTriangles = new List<Triangle>();

            foreach (var triangle in original)
            {
                newTriangles.AddRange(triangle.Subdivide(ref vertices));
            }

            return newTriangles;
        }

        /// <summary>
        /// Creates the triangles for an icosahedron
        /// </summary>
        private List<Triangle> IcosahedronTriangles()
        {
            var triangles = new List<Triangle>();

            // Top triangles
            for (int i = 2; i < 12; i += 2)
            {
                int nexti = i == 10 ? 2 : i + 2;
                triangles.Add(new Triangle(0, i, nexti));
            }

            // Center band
            for (int i = 2; i < 12; ++i)
            {
                int nexti = i == 11 ? 2 : i + 1;
                int nextNexti = i > 9 ? i - 8 : i + 2;
                if (i % 2 == 0)
                {
                    triangles.Add(new Triangle(i, nexti, nextNexti));
                }
                else
                {
                    triangles.Add(new Triangle(i, nextNexti, nexti));
                }
            }

            // Bottom triangles
            for (int i = 3; i < 12; i += 2)
            {
                int nexti = i == 11 ? 3 : i + 2;
                triangles.Add(new Triangle(1, nexti, i));
            }

            return triangles;
        }

        protected override List<Vertex> MakeVertices()
        {
            var vertices = MakeIcosahedronVertices();
            var triangles = IcosahedronTriangles();

            for (int i = 0; i < NSubdivisions; ++i)
            {
                triangles = SubdivideTriangles(triangles, ref vertices);
            }

            // Store the triangles for use in MakeTriangles()
            Triangles[NSubdivisions] = triangles;
            return vertices;
        }

        protected override Int32Collection MakeTriangles()
        {
            var finalTriangles = new Int32Collection();

            foreach (var triangle in Triangles[NSubdivisions])
            {
                finalTriangles.Add(triangle.V1);
                finalTriangles.Add(triangle.V2);
                finalTriangles.Add(triangle.V3);
            }

            return finalTriangles;
        }

    }
}
