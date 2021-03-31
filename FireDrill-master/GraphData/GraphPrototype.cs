using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphData
{
    public class GraphPrototype : IGraphPrototype
    {
        public string XAxisTitle { get; }
        public string YAxisTitle { get; }
        public List<TimelinePrototype> Timelines { get; } = new List<TimelinePrototype>();

        public GraphPrototype(string xAxisTitle, string yAxisTitle)
        {
            XAxisTitle = xAxisTitle;
            YAxisTitle = yAxisTitle;
        }

        public void AddTimeline(TimelinePrototype timeline)
        {
            Timelines.Add(timeline);
        }

        IGraphPrototype.GraphType IGraphPrototype.GetGraphType()
        {
            return IGraphPrototype.GraphType.Graph;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(XAxisTitle);
            bw.Write(YAxisTitle);
            bw.Write(Timelines.Count);
            foreach (var timeline in Timelines)
            {
                timeline.WriteToFile(bw);
            }
        }

        internal GraphPrototype(BinaryReader br)
        {
            XAxisTitle = br.ReadString();
            YAxisTitle = br.ReadString();
            int nTimelines = br.ReadInt32();
            for (int i = 0; i < nTimelines; ++i)
            {
                var timeline = new TimelinePrototype(br);
                Timelines.Add(timeline);
            }
        }
    }
}
