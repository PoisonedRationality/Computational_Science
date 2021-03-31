using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry
{
    /// <summary>
    /// An all-purpose exception for geometry errors.
    /// Common for divison by zero, no-slope errors, etc.
    /// </summary>

    public class GeometryException : Exception
    {
        public GeometryException(string what) :
            base(what)
        { }
    }
}
