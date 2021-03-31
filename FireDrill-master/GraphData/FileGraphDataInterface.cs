using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GraphData
{
    public class FileGraphDataInterface : IGraphDataInterface
    {
        private readonly BinaryReader br;

        public FileGraphDataInterface(BinaryReader br)
        {
            this.br = br;
        }

        public IEnumerable<IGraphPrototype> Graphs
        {
            get
            {
                int nGraphs = br.ReadInt32();
                for (int i = 0; i < nGraphs; ++i)
                {
                    yield return IGraphPrototype.ReadFromFile(br);
                }
            }
        }

        public GraphDataPacket GetData()
        {
            return new GraphDataPacket(br);
        }
    }
}
