using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualizerBaseClasses;
using VisualizerControl.Commands;

namespace VisualizerControl
{
    /// <summary>
    /// An interface for a command issued to the visualizer
    /// </summary>
    abstract public class VisualizerCommand : ICommand<Visualizer>
    {
        public abstract void Do(Visualizer viz);
        public void WriteToFile(BinaryWriter bw)
        {
            CommandType type = enumDictionary[GetType()];
            bw.Write((byte)type);
            WriteContent(bw);
        }

        static public VisualizerCommand ReadFromFile(BinaryReader br)
        {
            byte typeCode = br.ReadByte();
            CommandType type = (CommandType)typeCode;

            return type switch
            {
                CommandType.AddObject => new AddObject(br),
                CommandType.RemoveObject => new RemoveObject(br),
                CommandType.MoveObject => new MoveObject(br),
                CommandType.TransformObject => new TransformObject(br),
                CommandType.ChangeMaterial => new ChangeMaterial(br),
                CommandType.MoveCamera => new MoveCamera(br),
                CommandType.ClearAll => new ClearAll(),
                _ => throw new NotImplementedException(),
            };
        }

        abstract protected void WriteContent(BinaryWriter bw);

        private enum CommandType : byte
        {
            AddObject, RemoveObject, MoveObject, TransformObject, ChangeMaterial,
            MoveCamera, ClearAll
        }

        private static readonly Dictionary<Type, CommandType> enumDictionary = new Dictionary<Type, CommandType>()
        {
            { typeof(AddObject), CommandType.AddObject },
            { typeof(RemoveObject), CommandType.RemoveObject },
            { typeof(MoveObject), CommandType.MoveObject },
            { typeof(TransformObject), CommandType.TransformObject },
            { typeof(ChangeMaterial), CommandType.ChangeMaterial },
            { typeof(MoveCamera), CommandType.MoveCamera },
            { typeof(ClearAll), CommandType.ClearAll }
        };

    }
}
