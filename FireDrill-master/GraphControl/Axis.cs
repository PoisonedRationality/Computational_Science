using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using DongUtility;

namespace GraphControl
{
    internal class Axis
    {
        private readonly DrawingGroup drawing = new DrawingGroup();
        public Drawing Drawing { get { return drawing; } }
        public Transform Transform
        {
            get { return drawing.Transform; }
            set { drawing.Transform = value; }
        }
        public string Title { get; set; }

        private readonly LineGeometry line = new LineGeometry();
        private System.Windows.Media.Geometry geoAxis;
        private System.Windows.Media.Geometry scientificNotation;
        private GeometryGroup group;

        private readonly bool bottom;

        private readonly DrawingGroup axisLabels = new DrawingGroup();
        private readonly Dictionary<double, System.Windows.Media.Geometry> labelDict = new Dictionary<double, System.Windows.Media.Geometry>();

        private System.Windows.Media.Geometry MakeText(string text, Point position, int fontSize = 10)
        {
            FormattedText thisText = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                new Typeface("Arial"), fontSize, Brushes.Black, 1)
            // 1 is the pixels per dip, which seems not to matter for us
            {
                TextAlignment = TextAlignment.Center
            };
            return thisText.BuildGeometry(position);
        }

        public Axis(string title, bool bottom = false)
        {
            this.bottom = bottom;
            Title = title;


        }

        public void Update(double width, double height, double min, double max)
        {
            if (bottom)
            {
                line.StartPoint = new Point(0, height);
                line.EndPoint = new Point(width, height);
            }
            else
            {
                line.StartPoint = new Point(0, 0);
                line.EndPoint = new Point(width, 0);
            }

            MakeLabels(width, height, min, max);
            group.Transform = new TranslateTransform(width / 2, height / 3);

        }

        private void MakeLabels(double width, double height, double min, double max)
        {
            axisLabels.Children.Clear();

            if (min == max)
            {
                if (min > 0)
                {
                    min = 0;
                }
                else
                {
                    max = 0;
                }
            }

            int order = OrderOfMagnitude(max - min);
            double scale = Math.Pow(10, order);

            if ((max - min) / scale <= 2)
            {
                scale /= 2;
            }

            double lh = scale * Math.Floor(min / scale);
            double rh = scale * Math.Ceiling(max / scale);

            double location = bottom ? 3 * height / 4 : height / 4;

            // Check for roundoff error
            while (lh + scale == lh)
                scale *= 10;

            int scaleOrder = OrderOfMagnitude(max);
            for (double place = lh; place <= rh; place += scale)
            {
                double pos = Position(min, max, width, place);
                if (pos <= width && pos >= 0)
                {
                    if (Math.Abs(scaleOrder) >= 3)
                        AddPoint(new Point(pos, location), place / UtilityFunctions.Pow(10, scaleOrder));
                    else
                        AddPoint(new Point(pos, location), place);
                }
            }

            AddLabel(new Point(0, bottom ? 5 : 10), Title, scaleOrder);
        }

        private int OrderOfMagnitude(double num)
        {
            return num <= 0 ? 0 : (int)Math.Floor(Math.Log10(Math.Abs(num)));
        }

        private double Position(double min, double max, double width, double x)
        {
            return (x - min) / (max - min) * width;
        }

        const int roundDigits = 3;

        private void AddPoint(Point position, double value)
        {
            Transform finalTransform;
            TranslateTransform trans = new TranslateTransform(position.X, position.Y);
            if (bottom)
            {
                TransformGroup tr = new TransformGroup();
                tr.Children.Add(trans);
                tr.Children.Add(new RotateTransform(90, position.X, position.Y));
                finalTransform = tr;
            }
            else
            {
                finalTransform = trans;
            }

            System.Windows.Media.Geometry newGeom;
            if (labelDict.ContainsKey(value))
            {
                newGeom = labelDict[value];
            }
            else
            {
                string printVersion = (Math.Round(value, roundDigits)).ToString();
                newGeom = MakeText(printVersion, new Point(0, 0));
                labelDict.Add(value, newGeom);
            }

            newGeom.Transform = finalTransform;
            Drawing newDrawing = new GeometryDrawing(Brushes.Black, null, newGeom);
            axisLabels.Children.Add(newDrawing);

        }

        private void AddLabel(Point position, string title, int power)
        {
            drawing.Children.Clear();
            TranslateTransform trans = new TranslateTransform(position.X, position.Y);
            if (bottom)
            {
                TransformGroup tr = new TransformGroup();
                tr.Children.Add(trans);
                tr.Children.Add(new RotateTransform(90, position.X, position.Y));
            }

            if (Math.Abs(power) >= 3)
            {
                const int powerFont = 8;
                geoAxis = MakeText(title + " ×10", new Point(0, bottom ? 5 : 10));
                scientificNotation = MakeText(power+"", new Point(geoAxis.Bounds.Right+(powerFont), geoAxis.Bounds.Top-(powerFont/3.0)), powerFont);
                group = new GeometryGroup();
                group.Children.Add(geoAxis);
                group.Children.Add(scientificNotation);

            }
            else
            {
                geoAxis = MakeText(title, new Point(0, bottom ? 5 : 10));
                group = new GeometryGroup();
                group.Children.Add(geoAxis);
            }
            drawing.Children.Add(new GeometryDrawing(null, new Pen(Brushes.Black, 2), line));
            drawing.Children.Add(new GeometryDrawing(Brushes.Black, null, group));
            drawing.Children.Add(axisLabels);
        }
    }
}
