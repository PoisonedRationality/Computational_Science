using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WPFUtility
{
    static public class BinaryExtensions
    {
        public static void Write(this BinaryWriter bw, Vector3D vector)
        {
            bw.Write(vector.X);
            bw.Write(vector.Y);
            bw.Write(vector.Z);
        }

        public static Vector3D ReadVector3D(this BinaryReader br)
        {
            double x = br.ReadDouble();
            double y = br.ReadDouble();
            double z = br.ReadDouble();
            return new Vector3D(x, y, z);
        }

        public static void Write(this BinaryWriter bw, Matrix3D matrix)
        {
            bw.Write(matrix.M11);
            bw.Write(matrix.M12);
            bw.Write(matrix.M13);
            bw.Write(matrix.M14);
            bw.Write(matrix.M21);
            bw.Write(matrix.M22);
            bw.Write(matrix.M23);
            bw.Write(matrix.M24);
            bw.Write(matrix.M31);
            bw.Write(matrix.M32);
            bw.Write(matrix.M33);
            bw.Write(matrix.M34);
            bw.Write(matrix.OffsetX);
            bw.Write(matrix.OffsetY);
            bw.Write(matrix.OffsetZ);
            bw.Write(matrix.M44);
        }

        public static Matrix3D ReadMatrix3D(this BinaryReader br)
        {
            double m11 = br.ReadDouble();
            double m12 = br.ReadDouble();
            double m13 = br.ReadDouble();
            double m14 = br.ReadDouble();
            double m21 = br.ReadDouble();
            double m22 = br.ReadDouble();
            double m23 = br.ReadDouble();
            double m24 = br.ReadDouble();
            double m31 = br.ReadDouble();
            double m32 = br.ReadDouble();
            double m33 = br.ReadDouble();
            double m34 = br.ReadDouble();
            double offsetX = br.ReadDouble();
            double offsetY = br.ReadDouble();
            double offsetZ = br.ReadDouble();
            double m44 = br.ReadDouble();

            return new Matrix3D(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, offsetX,
                offsetY, offsetZ, m44);
        }

        public static void Write(this BinaryWriter bw, MeshGeometry3D mesh)
        {
            // Number of vertices
            bw.Write(mesh.Positions.Count);

            foreach (var position in mesh.Positions)
                bw.Write(position);
            foreach (var normal in mesh.Normals)
                bw.Write(normal);
            foreach (var textureCoordinate in mesh.TextureCoordinates)
                bw.Write(textureCoordinate);

            // Triangle array
            bw.Write(mesh.TriangleIndices.Count);

            foreach (var triangleIndex in mesh.TriangleIndices)
                bw.Write(triangleIndex);
        }

        public static MeshGeometry3D ReadMeshGeometry3D(this BinaryReader br)
        {
            var mesh = new MeshGeometry3D();

            int nVertices = br.ReadInt32();

            mesh.Positions = new Point3DCollection(nVertices);
            mesh.Normals = new Vector3DCollection(nVertices);
            mesh.TextureCoordinates = new PointCollection(nVertices);

            for (int i = 0; i < nVertices; ++i)
                mesh.Positions.Add(br.ReadPoint3D());
            for (int i = 0; i < nVertices; ++i)
                mesh.Normals.Add(br.ReadVector3D());
            for (int i = 0; i < nVertices; ++i)
                mesh.TextureCoordinates.Add(br.ReadPoint());

            var nTriangleIndices = br.ReadInt32();

            mesh.TriangleIndices = new Int32Collection(nTriangleIndices);

            for (int i = 0; i < nTriangleIndices; ++i)
                mesh.TriangleIndices.Add(br.ReadInt32());

            return mesh;
        }

        public static void Write(this BinaryWriter bw, Point3D point)
        {
            bw.Write(point.X);
            bw.Write(point.Y);
            bw.Write(point.Z);
        }

        public static Point3D ReadPoint3D(this BinaryReader br)
        {
            double x = br.ReadDouble();
            double y = br.ReadDouble();
            double z = br.ReadDouble();

            return new Point3D(x, y, z);
        }

        public static void Write(this BinaryWriter bw, Point point)
        {
            bw.Write(point.X);
            bw.Write(point.Y);
        }

        public static Point ReadPoint(this BinaryReader br)
        {
            double x = br.ReadDouble();
            double y = br.ReadDouble();

            return new Point(x, y);
        }
        
        public static void Write(this BinaryWriter bw, Color color)
        {
            bw.Write(color.R);
            bw.Write(color.G);
            bw.Write(color.B);
            bw.Write(color.A);
        }

        public static Color ReadColor(this BinaryReader br)
        {
            byte r = br.ReadByte();
            byte g = br.ReadByte();
            byte b = br.ReadByte();
            byte a = br.ReadByte();

            return Color.FromArgb(a, r, g, b);
        }
    }
}
