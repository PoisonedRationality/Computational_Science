using GraphControl;
using GraphData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GraphControl
{
    public class Histogram : TransformingObject, IGraphInterface
    {
        private IList<HistoBin> histo = new List<HistoBin>();
        private IList<RectangleGeometry> geoList = new List<RectangleGeometry>();

        private class HistoBin
        {
            public HistoBin(double lowEdge)
            {
                this.LowEdge = lowEdge;
            }
            public int Num { get; set; } = 0;
            public double LowEdge { get; set; }
        }

        public Histogram(int nBins, Color color, string xTitle) :
            base(xTitle, "Frequency")
        {
            NBins = nBins;

            Brush mybrush = new SolidColorBrush(color);
            for (int i = 0; i < nBins; ++i)
            {
                var geo = new RectangleGeometry
                {
                    Transform = Transform
                };
                geoList.Add(geo);

                var geoDraw = new GeometryDrawing(mybrush, null, geo);
                drawing.Children.Add(geoDraw);

            }

        }

        public int NBins { get; }

        public void Update(GraphDataPacket data)
        {
            var list = data.GetSet().ToList();

            list.Sort();

            histo.Clear();

            double range = list.Last() - list.First();
            double binSize = range / NBins;

            int currentBin = 0;
            double currentLow = list.First() - binSize;
            histo.Add(new HistoBin(currentLow += binSize));

            foreach (var number in list)
            {
                if (number < currentLow + binSize || histo.Count == NBins)
                {
                    ++histo[currentBin].Num;
                }
                else
                {
                    histo.Add(new HistoBin(currentLow += binSize));
                    ++currentBin;
                }
            }

            for (int i = 0; i < histo.Count; ++i)
            {
                geoList[i].Rect = new System.Windows.Rect(histo[i].LowEdge, 0, binSize, histo[i].Num);
            }
        }

        public void UpdateTransform(double width, double height, double widthOffset, double heightOffset)
        {
            Range range;

            if (histo.Count == 0)
            {
                range = new Range(0, 0, 1, 1);
            }
            else
            {
                range = new Range(histo[0].LowEdge, 0, histo.Last().LowEdge, histo.Max((x) => x.Num));
            }

            UpdateMatrix(width, height, widthOffset, heightOffset, range);

            UpdateAxes(width, height, widthOffset, heightOffset, range);
        }
    }
}
