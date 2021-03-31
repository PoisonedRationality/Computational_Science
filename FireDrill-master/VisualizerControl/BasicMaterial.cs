using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WPFUtility;

namespace VisualizerControl
{
    public class BasicMaterial
    {
        public Color Color { get; }
        public double Fresnel { get; }
        public double Roughness { get; }

        static public bool operator==(BasicMaterial lhs, BasicMaterial rhs)
        {
            return lhs.Color == rhs.Color && lhs.Fresnel == rhs.Fresnel && lhs.Roughness == rhs.Roughness;
        }

        static public bool operator!=(BasicMaterial lhs, BasicMaterial rhs)
        {
            return !(lhs == rhs);
        }

        public BasicMaterial(Color color, double fresnel, double roughness)
        {
            Color = color;
            Fresnel = fresnel;
            Roughness = roughness;
        }

        public BasicMaterial(BinaryReader br)
        {
            Color = br.ReadColor();
            Fresnel = br.ReadDouble();
            Roughness = br.ReadDouble();
        }

        public void WriteContent(BinaryWriter bw)
        {
            bw.Write(Color);
            bw.Write(Fresnel);
            bw.Write(Roughness);
        }

        public override int GetHashCode()
        {
            return Color.A ^ Color.R
                ^ Color.G ^ Color.B
                ^ Fresnel.GetHashCode() ^ Roughness.GetHashCode();
        }
    }
}
