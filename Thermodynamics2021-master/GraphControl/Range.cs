using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphControl
{
    public struct Range
    { 
        public struct RangePair
        {
            /// <summary>
            /// A data structure for min and max
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            public RangePair(double min, double max)
            {
                Min = min;
                Max = max;
            }

            public double Min;
            public double Max;
            public double Range => Max - Min;

            /// <summary>
            /// Compares inserted value to current max and min and if it is greater than the max or less than the min
            /// it becomes that value.
            /// </summary>
            /// <param name="val">New min or max</param>
            public void SetMinMax(double val)
            {
                if (val < Min)
                    Min = val;
                if (val > Max)
                    Max = val;
            }

            /// <summary>
            /// Updates range for the inserted RangePair
            /// </summary>
            /// <param name="pair">RangePair with new max and min</param>
            public void AdjustRange(RangePair pair)
            {
                AdjustRange(pair.Min, pair.Max);
            }

            /// <summary>
            /// Updates range for the inserted min and max and checks if the inserted values are 
            /// less than min and greater than max.
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            public void AdjustRange(double min, double max)
            {
                if (min < Min)
                    Min = min;
                if (max > Max)
                    Max = max;
            }
        }

        /// <summary>
        /// Gives a default range where any inserted value for min would be less and any value for max would be more
        /// </summary>
        /// <returns>A default range of two range that are easily replacable for the x and y axis</returns>
        static public Range DefaultRange()
        {
            return new Range(Double.MaxValue, Double.MaxValue, Double.MinValue, Double.MinValue);
        }

        /// <summary>
        /// initializes a range which consists of two range pairs, one for the x and y axis
        /// </summary>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        public Range(double minx, double miny, double maxx, double maxy)
        {
            X = new RangePair(minx, maxx);
            Y = new RangePair(miny, maxy);
        }

        public RangePair X;
        public RangePair Y;

        public double Width => X.Range;
        public double Height => Y.Range;

        /// <summary>
        /// Sets min and max for both RangePairs in Range  
        /// </summary>
        /// <param name="xVal">New min or max for x axis</param>
        /// <param name="yVal">New min or max for y axis</param>
        public void SetMinMax(double xVal, double yVal)
        {
            X.SetMinMax(xVal);
            Y.SetMinMax(yVal);
        }

        /// <summary>
        /// Updates range for inserted range on both RangePairs of Range
        /// </summary>
        /// <param name="other">A Range of new maxes and mins</param>
        public void AdjustRange(Range other)
        {
            X.AdjustRange(other.X);
            Y.AdjustRange(other.Y);
        }

    }
}
