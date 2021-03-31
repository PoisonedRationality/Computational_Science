using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WPFUtility;

namespace VisualizerControl.Shapes
{
    /// <summary>
    /// An abstract base class for shapes
    /// </summary>
    abstract public class Shape3D
    {
        /// <summary>
        /// The mesh for the class
        /// </summary>
        internal MeshGeometry3D Mesh
        {
            get
            {
                // If it is already in the dictionary, don't generate a new one
                if (ShapeName != "" && meshes.ContainsKey(ShapeName))
                {
                    return meshes[ShapeName];
                }
                else
                {
                    var mesh = MakeMesh();
                    if (freezeMesh)
                    {
                        mesh.Freeze();
                    }
                    meshes[ShapeName] = mesh;

                    return mesh;
                }
            }
        }

        private static readonly Dictionary<BinaryWriter, HashSet<string>> namesUsedInWriting
            = new Dictionary<BinaryWriter, HashSet<string>>();

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ShapeName);
            bw.Write(freezeMesh);
            if (!namesUsedInWriting.ContainsKey(bw))
            {
                namesUsedInWriting.Add(bw, new HashSet<string>());
            }

            if (!namesUsedInWriting[bw].Contains(ShapeName))
            {
                bw.Write(Mesh);
                namesUsedInWriting[bw].Add(ShapeName);
            }
        }

        private static readonly Dictionary<BinaryReader, HashSet<string>> namesUsedInReading
            = new Dictionary<BinaryReader, HashSet<string>>();

        public static Shape3D ReadShapeFromFile(BinaryReader br)
        {
            string name = br.ReadString();
            bool freeze = br.ReadBoolean();

            if (!namesUsedInReading.ContainsKey(br))
            {
                namesUsedInReading.Add(br, new HashSet<string>());
            }

            if (!namesUsedInReading[br].Contains(name))
            {
                var mesh = br.ReadMeshGeometry3D();
                meshes[name] = mesh;
                namesUsedInReading[br].Add(name);
            }

            return new Shape3DFromMesh(name, freeze);
        }

        /// <summary>
        /// A static dictionary holding many meshes.
        /// This way, only one mesh is created per unique shape.
        /// </summary>
        private static ConcurrentDictionary<string, MeshGeometry3D> meshes = new ConcurrentDictionary<string, MeshGeometry3D>();

        /// <summary>
        /// Protected constructor to create unique shapes
        /// </summary>
        /// <param name="shapeName">Name of the shape</param>
        /// <param name="freezeMesh">Whether the mesh can safely be frozen for performance reasons</param>
        protected Shape3D(string shapeName, bool freezeMesh = true)
        {
            ShapeName = shapeName;
            this.freezeMesh = freezeMesh;
        }

        public static Shape3D CreateShape(DongUtility.Shapes.Shapes3D shape, bool freezeMesh = true)
        {
            switch (shape)
            {
                case DongUtility.Shapes.Shapes3D.Cube:
                    return new Cube3D();

                case DongUtility.Shapes.Shapes3D.Sphere:
                    return new Sphere3D();

                case DongUtility.Shapes.Shapes3D.Cylinder:
                    return new Cylinder3D();

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The name of the shape, used to avoid duplicate meshes
        /// </summary>
        public string ShapeName { get; }
        /// <summary>
        /// Whether or not to freeze the mesh
        /// </summary>
        private readonly bool freezeMesh;

        /// <summary>
        /// A class to hold vertex information
        /// </summary>
        public class Vertex
        {
            internal Vertex(Point3D position, Vector3D normal, Point textureCoordinate)
            {
                Position = position;
                Normal = normal;
                TextureCoordinate = textureCoordinate;
            }

            /// <summary>
            /// The position of the vertex
            /// </summary>
            public Point3D Position { get; set; }
            /// <summary>
            /// The normal vector at the vertex
            /// </summary>
            public Vector3D Normal { get; set; }
            /// <summary>
            /// The (u,v) coordinate of this point
            /// </summary>
            public Point TextureCoordinate { get; set; }
        }

        /// <summary>
        /// Create the actual mesh
        /// </summary>
        private MeshGeometry3D MakeMesh()
        {
            var mesh = new MeshGeometry3D();

            var vertices = MakeVertices();

            mesh.Positions = new Point3DCollection(vertices.Count);
            mesh.Normals = new Vector3DCollection(vertices.Count);
            mesh.TextureCoordinates = new PointCollection(vertices.Count);
            mesh.TriangleIndices = MakeTriangles();

            foreach (var vertex in vertices)
            {
                mesh.Positions.Add(vertex.Position);
                mesh.Normals.Add(vertex.Normal);
                mesh.TextureCoordinates.Add(vertex.TextureCoordinate);
            }

            return mesh;
        }

        /// <summary>
        /// Create the vertices
        /// </summary>
        /// <returns>A list of vertices</returns>
        abstract protected List<Vertex> MakeVertices();
        /// <summary>
        /// Create triangle mapping
        /// </summary>
        /// <returns>A list of indices that come in sets of three, matching the results of MakeVertices()</returns>
        abstract protected Int32Collection MakeTriangles();
    }
}
