using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GraphData
{
    public class RealTimeGraphDataInterface : IGraphDataInterface
    {
        public RealTimeGraphDataInterface(GraphDataManager manager)
        {
            Manager = manager;
        }

        public GraphDataManager Manager { get; }
        public IEnumerable<IGraphPrototype> Graphs => Manager.Graphs;

        public GraphDataPacket GetData() => Manager.GetData();
    }
}
