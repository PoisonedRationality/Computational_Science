using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizerControl.Commands
{
    public class ClearAll : VisualizerCommand
    {
        public override void Do(Visualizer viz)
        {
            viz.Clear();
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            // Has no extra data to add
        }
    }
}
