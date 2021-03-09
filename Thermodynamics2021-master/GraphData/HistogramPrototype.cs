using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GraphData
{
    public class HistogramPrototype : IGraphPrototype
    {
        public int NBins { get; }
        public Color Color { get; }
        public string XAxisTitle { get; }

        public HistogramPrototype(int nbins, Color color, string xAxisTitle)
        {
            NBins = nbins;
            Color = color;
            XAxisTitle = xAxisTitle;
        }

        IGraphPrototype.GraphType IGraphPrototype.GetGraphType()
        {
            return IGraphPrototype.GraphType.Histogram;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(NBins);
            bw.Write(Color);
            bw.Write(XAxisTitle);
        }

        internal HistogramPrototype(BinaryReader br)
        {
            NBins = br.ReadInt32();
            Color = br.ReadColor();
            XAxisTitle = br.ReadString();
        }
    }
}
