using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphControl
{
    class Legend
    {
        private DrawingGroup drawing = new DrawingGroup();
        public Drawing Drawing { get { return drawing; } }
        public Transform Transform
        {
            get { return drawing.Transform; }
            set { drawing.Transform = value; }
        }

        private IList<Tuple<string, Color>> list = new List<Tuple<string, Color>>();

        /// <summary>
        /// adds the name and the color of the line
        /// </summary>
        /// <param name="timeline"></param>
        public void AddTimeline(Timeline timeline)
        {

            list.Add(new Tuple<string, Color>(timeline.Name, timeline.Color));
            Update();
        }

        /// <summary>
        /// updates the legend with all of the boxes and descriptions
        /// </summary>
        private void Update()
        {
            drawing.Children.Clear();

            double currentY = 0;

            //if there is one thing being graphed, don't include a legend
            if (list.Count <= 1)
            {
                return;
            }
            else
            {
                //makes a new box and description for each tuple in the list
                foreach (var tuple in list)
                {
                    Brush myBrush = new SolidColorBrush(tuple.Item2);
                    FormattedText thisText = new FormattedText(tuple.Item1, CultureInfo.CurrentUICulture,
                        FlowDirection.LeftToRight, new Typeface("Arial"), 10, myBrush, 1);
                    // 1 is pixels per dip, which seems not to matter
                    double boxSize = thisText.Height;
                    var geo = new RectangleGeometry(new Rect(0, currentY, boxSize, boxSize));
                    drawing.Children.Add(new GeometryDrawing(myBrush, null, geo));
                    var textGeo = thisText.BuildGeometry(new Point(1.5 * boxSize, currentY));
                    drawing.Children.Add(new GeometryDrawing(myBrush, null, textGeo));
                    currentY += boxSize;
                }

                //Rectangle border around legend
                var legendRect = new RectangleGeometry(new Rect(-.05 * drawing.Bounds.Width, -.05 * drawing.Bounds.Height, drawing.Bounds.Width * 1.1, drawing.Bounds.Height * 1.1));
                drawing.Children.Insert(0, new GeometryDrawing(Brushes.White, new Pen(Brushes.Black, 1), legendRect));
            }
          
        }
    }
}
