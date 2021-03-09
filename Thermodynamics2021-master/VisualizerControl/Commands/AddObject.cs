using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace VisualizerControl.Commands
{
    /// <summary>
    /// A command to add an object to the visualizer.
    /// This is added by index; it is the user's job to keep track of the indices.
    /// </summary>
    public class AddObject : VisualizerCommand
    {
        private readonly ObjectPrototype obj;
        private readonly int index;

        /// <param name="obj">The Object3D to add</param>
        /// <param name="index">An assigned index, to be used for subsequent commands.  Must be unique.</param>
        public AddObject(ObjectPrototype obj, int index)
        {
            this.obj = obj;
            this.index = index;
        }

        public override void Do(Visualizer viz)
        {
            var obj3d = new Object3D(obj);
            viz.AddParticle(obj3d, index);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            obj.WriteToFile(bw);
            bw.Write(index);
        }

        internal AddObject(BinaryReader br)
        {
            obj = ObjectPrototype.ReadFromFile(br);
            index = br.ReadInt32();
        }
    }
}
