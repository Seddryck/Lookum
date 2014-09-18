using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Lookup
{
    public abstract class EnumLookup<T, K> : BaseLookup<K, string>
    {
        protected EnumLookup()
            : base()
        { }

        protected EnumLookup(bool throwException)
            : base(throwException)
        { }

        protected EnumLookup(string defaultValue)
            : base(defaultValue)
        {}

        protected EnumLookup(Func<K, string> nonMatchBehavior)
            : base(nonMatchBehavior)
        { }

        protected override void OnLoad()
        {
            Type t = typeof(T);
            foreach (var enumMember in Enum.GetValues(t))
                Map.Add((K)enumMember, (Enum.GetName(t, enumMember)));
        }
    }
}
