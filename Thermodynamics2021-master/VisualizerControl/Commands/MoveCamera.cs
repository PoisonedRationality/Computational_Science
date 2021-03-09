using DongUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Media3D;
using WPFUtility;
using static WPFUtility.UtilityFunctions;

namespace VisualizerControl.Commands
{
    public class MoveCamera : VisualizerCommand
    {
        private Vector position;
        private Vector lookDirection;
        private Vector upDirection;
        public MoveCamera(Vector position, Vector lookDirection, Vector upDirection)
        {
            this.position = position;
            this.lookDirection = lookDirection;
            this.upDirection = upDirection;
        }
        public override void Do(Visualizer viz)
        {
            viz.MoveCamera(ConvertToPoint3D(ConvertVector(position)), ConvertVector(lookDirection), ConvertVector(upDirection));
        }

        protected override void WriteContent(BinaryWriter bw)
        {
            bw.Write(position);
            bw.Write(lookDirection);
            bw.Write(upDirection);
        }

        internal MoveCamera(BinaryReader br)
        {
            position = br.ReadVector();
            lookDirection = br.ReadVector();
            upDirection = br.ReadVector();
        }

    }
}
