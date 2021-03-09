using DongUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphData
{
    public class UpdatingFunctions
    {
        public delegate void GetFunction(GraphDataPacket packet);

        private readonly List<GetFunction> functions = new List<GetFunction>();

        public void AddFunction(GetFunction function)
        {
            functions.Add(function);
        }

        public void AddFunctions(UpdatingFunctions other)
        {
            functions.AddRange(other.functions);
        }

        public GraphDataPacket GetData()
        {
            var data = new GraphDataPacket();
            foreach (var func in functions)
            {
                func(data);
            }
            return data;
        }
    }
}
