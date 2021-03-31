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
    public class GraphUnderlying : TransformingObject, IGraphInterface
    {
        private List<Timeline> timelines = new List<Timeline>();
        private const double roundoffRatio = 2e-5;

        public Timeline GetTimeline(int index)
        {
            return timelines[index];
        }

        private Legend legend = new Legend();

        public GraphUnderlying(string xTitle, string yTitle) :
            base(xTitle, yTitle)
        {
            drawing.Children.Add(legend.Drawing);
        }

        public void AddTimeline(Timeline timeline)
        {
            timelines.Add(timeline);
            drawing.Children.Insert(0, timeline.GetDrawing());
            timeline.Transform = Transform;
            legend.AddTimeline(timeline);
        }

        public void Update(GraphDataPacket data)
        {
            for (int i = 0; i < timelines.Count; ++i)
            {
                timelines[i].Update(data);
            }
        }

        public void UpdateTransform(double width, double height, double widthOffset, double heightOffset)
        {
            Range range = Range.DefaultRange();

            foreach (var timeline in timelines)
            {
                range.AdjustRange(timeline.Range);
            }

            if (range.X.Min == range.X.Max)
            {
                range.X = new Range.RangePair(-1, 1);
            }
            if (range.Y.Min != 0)
            {
                double ratio = Math.Abs(range.Y.Range / range.Y.Min);
                if (ratio < roundoffRatio)
                {
                    double diff = roundoffRatio - ratio;
                    double shift = diff / 2 * range.Y.Min;
                    range.Y = new Range.RangePair(range.Y.Min - shift, range.Y.Max + shift);
                }

            }

            if (double.IsInfinity(range.Width))
            {
                range.X = new Range.RangePair(0, 1);

            }
            if (double.IsInfinity(range.Height))
            {
                range.Y = new Range.RangePair(0, 1);
            }

            //centers the line in the middle of the graphs
            double alignNum = range.Y.Range * .1;
            range.Y = new Range.RangePair(range.Y.Min - alignNum, range.Y.Max + alignNum);

            UpdateMatrix(width, height, widthOffset, heightOffset, range);
            UpdateAxes(width, height, widthOffset, heightOffset, range);
            SetLegend(width, height, widthOffset, heightOffset);
        }

        private const double legendMarginFactor = .1;

        private void SetLegend(double width, double height, double widthOffset, double heightOffset)
        {
            double rhs = width * (1 - legendMarginFactor) + widthOffset;
            double lhs = rhs - legend.Drawing.Bounds.Width;
            double top = legendMarginFactor * height + heightOffset;
            legend.Transform = new TranslateTransform(lhs, top);
        }

    }
}

