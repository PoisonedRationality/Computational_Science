using DongUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VisualizerControl.Shapes;
using WPFUtility;
using static WPFUtility.UtilityFunctions;

namespace VisualizerControl
{
    public class ObjectPrototype
    {
        internal Shape3D Shape { get; }
        internal BasicMaterial Material { get; }

        internal Vector3D Position { get; }
        internal Vector3D Scale { get; }
        internal Matrix3D Rotation { get; }

        public void WriteToFile(BinaryWriter bw)
        {
            Shape.WriteToFile(bw);
            Material.WriteContent(bw);
            bw.Write(Position);
            bw.Write(Scale);
            bw.Write(Rotation);
        }

        public static ObjectPrototype ReadFromFile(BinaryReader br)
        {
            var shape = Shape3D.ReadShapeFromFile(br);
            var material = new BasicMaterial(br);
            var position = br.ReadVector3D();
            var scale = br.ReadVector3D();
            var rotation = br.ReadMatrix3D();

            return new ObjectPrototype(shape, material, position, scale, rotation);
        }

        public ObjectPrototype(Shape3D shape, Color color, double fresnel, double roughness) :
            this(shape, new BasicMaterial(color, fresnel, roughness))
        { }

        public ObjectPrototype(Shape3D shape, BasicMaterial material) :
            this(shape, material, new Vector3D(0, 0, 0), new Vector3D(1, 1, 1))
        {}

        public ObjectPrototype(Shape3D shape, BasicMaterial material, Vector3D position, Vector3D scale) :
            this(shape, material, position, scale, Matrix3D.Identity)
        { }

        public ObjectPrototype(Shape3D shape, BasicMaterial material, Vector3D position, Vector3D scale,
            Matrix3D rotation)
        {
            if (!position.IsValid() || !scale.IsValid())
                throw new ArgumentException("Infinity or not-a-number passed into ObjectPrototype!");

            Shape = shape;
            Material = material;
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public ObjectPrototype(Shape3D shape, BasicMaterial material, Vector position, Vector scale, Rotation rotation) :
            this(shape, material, ConvertVector(position), ConvertVector(scale), ConvertToMatrix3D(rotation.Matrix))
        { }

        public ObjectPrototype(Shape3D shape, BasicMaterial material, Vector position, Vector scale) :
            this(shape, material, position, scale, DongUtility.Rotation.Identity)
        { }
    }

}
