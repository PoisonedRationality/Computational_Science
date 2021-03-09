using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphData
{
    public interface IGraphPrototype
    {
        public enum GraphType : byte { Graph, Histogram, Text, LeaderBoard }

        public GraphType GetGraphType();

        public void WriteToFile(BinaryWriter bw);

        static public IGraphPrototype ReadFromFile(BinaryReader br)
        {
            byte typeCode = br.ReadByte();
            var graphType = (GraphType)typeCode;

            return graphType switch
            {
                GraphType.Graph => new GraphPrototype(br),
                GraphType.Histogram => new HistogramPrototype(br),
                GraphType.Text => new TextPrototype(br),
                GraphType.LeaderBoard => new LeaderBoardPrototype(br),
                _ => throw new NotImplementedException("Should never reach here")
            };
                
        }
    }
}
