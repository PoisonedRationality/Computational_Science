using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static VisualizerControl.Shapes.Shape3D;

namespace VisualizerControl.Shapes
{
    /// <summary>
    /// A factory class for creating cylinders
    /// </summary>
    static class CylinderFactory
    {
        /// <summary>
        /// Struct to assist constructing edges of cylinder
        /// </summary>
        private struct TwoPoints
        {
            public TwoPoints(double edgeX, double edgeY, double uv)
            {
                EdgePoint = new Point(edgeX, edgeY);
                UVPoint = uv;
            }

            public Point EdgePoint { get; }
            public double UVPoint { get; }
        }

        static public List<Vertex> MakeVertices(bool withEnds, int nSegments)
        {
            var points = new List<Vertex>();

            double phiSeg = 2 * Math.PI / nSegments;

            // 0: center of top
            // 1 to NSeg: top edge
            // NSeg + 1 to 2 * NSeg: top side
            // 2 * NSeg + 1 to 3 * NSeg: bottom side
            // 3 * NSeg + 1 to 4 * NSeg: bottom edge
            // 4 * NSeg + 1: center of botom

            List<TwoPoints> edgePoints = new List<TwoPoints>();

            // Points along the edge
            for (double iphi = 0; iphi < 2 * Math.PI; iphi += phiSeg)
            {
                double x = Math.Cos(iphi);
                double y = Math.Sin(iphi);
                edgePoints.Add(new TwoPoints(x, y, iphi / (2 * Math.PI)));
            }

            if (withEnds)
            {
                // The center of the top 
                points.Add(new Vertex(new Point3D(0, 0, 1), new Vector3D(0, 0, 1), new Point(.5, .5)));

                foreach (var point in edgePoints)
                {
                    points.Add(new Vertex(new Point3D(point.EdgePoint.X, point.EdgePoint.Y, 1), new Vector3D(0, 0, 1), point.EdgePoint));
                }
            }

            foreach (var point in edgePoints)
            {
                points.Add(new Vertex(new Point3D(point.EdgePoint.X, point.EdgePoint.Y, 1), new Vector3D(point.EdgePoint.X, point.EdgePoint.Y, 0),
                    new Point(point.UVPoint, 0)));
            }

            foreach (var point in edgePoints)
            {
                points.Add(new Vertex(new Point3D(point.EdgePoint.X, point.EdgePoint.Y, -1), new Vector3D(point.EdgePoint.X, point.EdgePoint.Y, 0),
                    new Point(point.UVPoint, 1)));
            }

            if (withEnds)
            {
                foreach (var point in edgePoints)
                {
                    points.Add(new Vertex(new Point3D(point.EdgePoint.X, point.EdgePoint.Y, -1), new Vector3D(0, 0, -1), point.EdgePoint));
                }

                // The center of the bottom 
                points.Add(new Vertex(new Point3D(0, 0, -1), new Vector3D(0, 0, -1), new Point(.5, .5)));
            }

            return points;
        }

        static public Int32Collection MakeTriangles(bool withEnds, int nSegments)
        {
            var triangles = new Int32Collection();

            const int topCenterIndex = 0;
            int bottomCenterIndex = 4 * nSegments + 1;
            int firstTopRing = withEnds ? 1 : 0;
            int lastTopRing = withEnds ? nSegments : nSegments - 1;

            for (int index = firstTopRing; index <= lastTopRing; ++index)
            {
                int nextIndex = index == lastTopRing ? firstTopRing : index + 1;

                if (withEnds)
                {
                    // Top circle
                    triangles.Add(index);
                    triangles.Add(nextIndex);
                    triangles.Add(topCenterIndex);

                    // Bottom circle - opposite orientation to face out
                    triangles.Add(index + 3 * nSegments);
                    triangles.Add(bottomCenterIndex);
                    triangles.Add(nextIndex + 3 * nSegments);
                }

                int increment = withEnds ? nSegments : 0;

                // Sides
                triangles.Add(index + increment);
                triangles.Add(index + increment + nSegments);
                triangles.Add(nextIndex + increment);

                triangles.Add(index + increment + nSegments);
                triangles.Add(nextIndex + increment + nSegments);
                triangles.Add(nextIndex + increment);
            }

            return triangles;
        }

    }
}
