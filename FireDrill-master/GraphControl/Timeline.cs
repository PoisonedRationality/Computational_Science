using DongUtility;
using GraphData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphControl
{
    public class Timeline : IUpdating
    {
        public string Name { get; set; }

        public Color Color { get; }

        private PolyLineSegment line = new PolyLineSegment();
        private PathGeometry geom = new PathGeometry();
        private GeometryDrawing drawing = new GeometryDrawing();
        public Transform Transform
        {
            get
            {
                return geom.Transform;
            }
            set
            {
                geom.Transform = value;
            }
        }

        public double Thickness { get; set; } = 3;

        /// <summary>
        /// Create an Object Timeline for creating and updating a graph.
        /// </summary>
        /// <param name="name">Name of the graph</param>
        /// <param name="valx">Function for X (delegate).</param>
        /// <param name="valy">Function for Y (delegate).</param>
        /// <param name="color">Color for the line on the graph.</param>
        public Timeline(string name, Color color)
        {
            Name = name;
            Color = color;

            drawing.Geometry = geom;
            PathFigure path = new PathFigure();
            geom.Figures.Add(path);
            path.Segments.Add(line);
            geom.Transform = Transform;

            drawing.Pen = new Pen(new SolidColorBrush(color), Thickness);
        }

        /// <summary>
        /// Returns the current calculated graph.
        /// </summary>
        /// <returns>Returns a drawing of type GeometryDrawing</returns>
        public GeometryDrawing GetDrawing()
        {
            return drawing;
        }

        private bool justStarted = true;
        /// <summary>
        /// Adds a point to the graph to be drawn.
        /// </summary>
        /// <param name="xVal">X value of the point.</param>
        /// <param name="yVal">Y value of the point</param>
        public void AddPoint(double xVal, double yVal)
        {
            if (justStarted)
            {
                geom.Figures[0].StartPoint = new Point(xVal, yVal);
                justStarted = false;
            }
            line.Points.Add(new Point(xVal, yVal));

            range.SetMinMax(xVal: xVal, yVal: yVal);

            if (line.Points.Count > MaximumPoints)
            {
                DeletePoint();
            }
        }

        /// <summary>
        /// Converts all sotred points into tuples.
        /// </summary>
        /// <returns>Returns a list of points as tuple pairs.</returns>
        public List<Tuple<double, double>> ExtractPoints()
        {
            var list = new List<Tuple<double, double>>();

            foreach (var point in line.Points)
            {
                list.Add(new Tuple<double, double>(point.X, point.Y));
            }

            return list;
        }

        static public double MaximumPoints { get; set; } = 1000;
        /// <summary>
        /// Delete the first stored point.
        /// </summary>
        private void DeletePoint()
        {
            Point toRemove = line.Points[0];

            line.Points.RemoveAt(0);
            if (toRemove.X <= range.X.Min)
            {
                range.X = new Range.RangePair(line.Points.Min(x => x.X), range.X.Max);
            }
            if (toRemove.X >= range.X.Max)
            {
                range.X = new Range.RangePair(range.X.Min, line.Points.Max(x => x.X));
            }

            if (toRemove.Y <= range.Y.Min)
            {
                range.Y = new Range.RangePair(line.Points.Min(x => x.Y), range.Y.Max);
            }
            if (toRemove.Y >= range.Y.Max)
            {
                range.Y = new Range.RangePair(range.Y.Min, line.Points.Max(x => x.Y));
            }
            geom.Figures[0].StartPoint = toRemove;

        }

        /// <summary>
        /// Adds a new point based on the given funcions.
        /// </summary>
        public void Update(GraphDataPacket data)
        {
            double x = data.GetData();
            double y = data.GetData();
            AddPoint(x, y);
        }

        private Range range = Range.DefaultRange();
        public Range Range { get { return range; } }
    }
}
