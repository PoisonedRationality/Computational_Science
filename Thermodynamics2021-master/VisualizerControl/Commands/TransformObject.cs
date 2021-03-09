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
    /// Performs a generic rotation, scale, and translation at once
    /// </summary>
    public class TransformObject : VisualizerCommand
    {
        private readonly int objectIndex;
        private readonly Vector3D position;
        private readonly Vector3D scale;
        private readonly Matrix3D rotation;

        /// <param name="objectIndex">The index of the object to transform</param>
        /// <param name="position">The position of the center of the object, as a Vector3D</param>
        /// <param name="scale">A Vector3D in which each component represents a scale factor in that direction</param>
        public TransformObject(int objectIndex, Vector3D position, Vector3D scale, Matrix3D rotationMatrix)
        {
            this.objectIndex = objectIndex;
            this.position = position;
            this.scale = scale;
            rotation = rotationMatrix;
        }

        public TransformObject(int objectIndex, Vector position, Vector scale, Rotation rotation) :
            this(objectIndex, ConvertVector(position), ConvertVector(scale), ConvertToMatrix3D(rotation.Matrix))
        { }

        public TransformObject(int objectIndex, Vector position, Vector scale) :
            this(objectIndex, position, scale, Rotation.Identity)
        { }

        public override void Do(Visualizer viz)
        {
            viz.TransformParticle(objectIndex, position, scale, rotation);
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(objectIndex);
            bw.Write(position);
            bw.Write(scale);
            bw.Write(rotation);
        }

        internal TransformObject(BinaryReader br)
        {
            objectIndex = br.ReadInt32();
            position = br.ReadVector3D();
            scale = br.ReadVector3D();
            rotation = br.ReadMatrix3D();
        }
    }
}
