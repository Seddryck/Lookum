using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Lookup
{
    public interface ILookup
    {
        void Load();
        int Count();
    }
}
