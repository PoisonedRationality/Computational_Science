using DongUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace GraphData
{
    public class TimelinePrototype
    {
        public string Name { get; }
        public Color Color { get; }

        public TimelinePrototype(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(Name);
            bw.Write(Color);
        }

        internal TimelinePrototype(BinaryReader br)
        {
            Name = br.ReadString();
            Color = br.ReadColor();
        }
    }
}
