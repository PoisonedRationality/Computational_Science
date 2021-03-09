using DongUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using WPFUtility;
using static WPFUtility.UtilityFunctions;

namespace VisualizerControl.Commands
{
    /// <summary>
    /// A command to move an existing object to a new position
    /// The object is accessed by index.
    /// </summary>
    public class MoveObject : VisualizerCommand
    {
        private readonly Vector3D newPosition;
        private readonly int index;

        public MoveObject(int index, Vector3D newPosition)
        {
            if (!newPosition.IsValid())
                throw new ArgumentException("Infinity or not-a-number passed into MoveObject!");

            this.newPosition = newPosition;
            this.index = index;
        }

        public MoveObject(int index, Vector newPosition) :
            this(index, ConvertVector(newPosition))
        { }

        public MoveObject(int index, double x, double y, double z) :
            this(index, new Vector3D(x, y, z))
        {}

        public override void Do(Visualizer viz)
        {
            viz.MoveParticle(index, newPosition);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(index);
            bw.Write(newPosition);
        }

        internal MoveObject(BinaryReader br)
        {
            index = br.ReadInt32();
            newPosition = br.ReadVector3D();
        }
    }
}
