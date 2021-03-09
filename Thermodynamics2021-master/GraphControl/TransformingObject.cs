using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GraphControl
{
    abstract public class TransformingObject
    {
        public MatrixTransform Transform { get; set; } = new MatrixTransform();
        
        protected DrawingGroup drawing = new DrawingGroup();
        public Drawing Drawing { get { return drawing; } }

        private Axis xAxis;
        private Axis yAxis;

        public TransformingObject(string xTitle, string yTitle)
        {
            xAxis = new Axis(xTitle);
            yAxis = new Axis(yTitle, true);
            drawing.Children.Add(xAxis.Drawing);
            drawing.Children.Add(yAxis.Drawing);
        }

        internal void UpdateMatrix(double width, double height, double widthOffset, double heightOffset, Range range)
        {
            Matrix mat = Transform.Matrix;
            double ax = width / (range.Width);
            double ay = height / (-range.Height);
            mat.M11 = ax;
            mat.M22 = ay;
            mat.OffsetX = -ax * range.X.Min + widthOffset;
            mat.OffsetY = -ay * range.Y.Max + heightOffset;
            Transform.Matrix = mat;
        }

        protected void UpdateAxes(double width, double height, double widthOffset, double heightOffset, Range range)
        {
            xAxis.Transform = new TranslateTransform(widthOffset, height + heightOffset);
            TransformGroup group = new TransformGroup();
            group.Children.Add(new RotateTransform(-90, 0, 0));
            group.Children.Add(new TranslateTransform(0, height + heightOffset));
            yAxis.Transform = group;

            xAxis.Update(width, heightOffset, range.X.Min, range.X.Max);
            yAxis.Update(height, widthOffset, range.Y.Min, range.Y.Max);
        }
    }
}
