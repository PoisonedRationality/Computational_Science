using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GraphData
{
    public class LeaderBoardPrototype : IGraphPrototype
    {
        public List<LeaderBarPrototype> Prototypes { get; } = new List<LeaderBarPrototype>();
        public IGraphPrototype.GraphType GetGraphType() => IGraphPrototype.GraphType.LeaderBoard;

        public LeaderBoardPrototype(IEnumerable<LeaderBarPrototype> leaderBars)
        {
            Prototypes.AddRange(leaderBars);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(Prototypes.Count);
            foreach (var prototype in Prototypes)
            {
                bw.Write(prototype.Name);
                bw.Write(prototype.Color);
            }
        }

        internal LeaderBoardPrototype(BinaryReader br)
        {
            int count = br.ReadInt32();
            for (int i = 0; i < count; ++i)
            {
                string name = br.ReadString();
                Color color = br.ReadColor();
                Prototypes.Add(new LeaderBarPrototype(name, color));
            }
        }
    }
}
