using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerControl.Commands
{
    /// <summary>
    /// A command to remove an existing object, by index
    /// </summary>
    public class RemoveObject : VisualizerCommand
    {
        private readonly int index;

        public RemoveObject(int index)
        {
            this.index = index;
        }

        public override void Do(Visualizer viz)
        {
            viz.RemoveParticle(index);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(index);
        }

        internal RemoveObject(BinaryReader br)
        {
            index = br.ReadInt32();
        }
    }
}
