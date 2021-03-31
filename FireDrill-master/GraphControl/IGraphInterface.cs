using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GraphControl
{
    public interface IGraphInterface : IUpdating
    {
        void UpdateTransform(double width, double height, double widthOffset, double heightOffset);
        Drawing Drawing { get; }
    }
}
