using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphData
{
    public class GraphDataPacket
    {
        public GraphDataPacket()
        { }

        public void AddData(double newdatum)
        {
            data.Enqueue(newdatum);
        }

        public void AddTextData(string newString)
        {
            textData.Enqueue(newString);
        }

        public void AddSet(IEnumerable<double> list)
        {
            AddData(list.Count());
            foreach (var element in list)
            {
                AddData(element);
            }
        }

        public IEnumerable<double> GetSet()
        {
            int size = (int)GetData();
            for (int i = 0; i < size; ++i)
            {
                yield return GetData();
            }
        }

        public int GetSetSize()
        {
            return (int)(data.Peek());
        }

        public void RemoveFromFront()
        {
            data.Dequeue();
        }

        public void RemoveFromFront(int number)
        {
            for (int i = 0; i < number; ++i)
            {
                RemoveFromFront();
            }
        }

        public double GetData()
        {
            return data.Dequeue();
        }

        public string GetTextData()
        {
            return textData.Dequeue();
        }

        public IEnumerable<double> GetData(int number)
        {
            while (number-- > 0)
            {
                yield return GetData();
            }
        }

        public void WriteData(BinaryWriter bw)
        {
            bw.Write(data.Count);
            foreach (double datum in data)
            {
                bw.Write(datum);
            }
            bw.Write(textData.Count);
            foreach (string textDatum in textData)
            {
                bw.Write(textDatum);
            }
        }

        public GraphDataPacket(BinaryReader br)
        {
            int dataSize = br.ReadInt32();
            for (int i = 0; i < dataSize; ++i)
            {
                data.Enqueue(br.ReadDouble());
            }
            int textSize = br.ReadInt32();
            for (int i = 0; i < textSize; ++i)
            {
                textData.Enqueue(br.ReadString());
            }
        }

        private readonly Queue<double> data = new Queue<double>();
        private readonly Queue<string> textData = new Queue<string>();
    }
}
