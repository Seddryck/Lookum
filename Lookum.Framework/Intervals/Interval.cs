using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Intervals
{
    public class Interval<T>
    {
        public T Start { get; set; }
        public T End { get; set; }

        public static Interval<T> Intersect(IEnumerable<T> starts, IEnumerable<T> ends)
        {
            var minStart = starts.Min();
            var maxStart = starts.Max();
            
            var minEnd = ends.Min();
            var maxEnd = starts.Max();

            return new Interval<T>() { Start = maxStart, End = minEnd };
        }
    }
}
