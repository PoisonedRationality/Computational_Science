using GraphData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphControl
{
    public interface IUpdating
    {
        void Update(GraphDataPacket data);
    }
}
