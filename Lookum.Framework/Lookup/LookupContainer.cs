using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Lookup
{
    public class LookupContainer : HashSet<ILookup>
    {
        public void Load()
        {
            Parallel.ForEach(this, lookup => lookup.Load());
        }

        public T Find<T>() where T : ILookup
        {
            return (T) (this.FirstOrDefault(l => l is T));
        }
    }
}
