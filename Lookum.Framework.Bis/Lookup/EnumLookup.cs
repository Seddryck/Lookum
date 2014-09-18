using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lookum.Framework.Lookup
{
    public class EnumLookup<T> : BaseLookup<int, string> where T : Enum
    {

        protected EnumLookup()
            : base()
        { }

        protected EnumLookup(bool throwException)
            : base()
        { }

        protected EnumLookup(string defaultValue)
            : base()
        { }

        protected EnumLookup(Func<string, string> nonMatchBehavior)
            : base()
        { }

        public virtual void Load()
        {
            LoadFromValue();
        }

        protected void LoadFromValue()
        {
            Type t = typeof(T);
            foreach (var v in Enum.GetValues(t))
                Map.Add(Convert.ToInt32(v), Enum.GetName(t, v));
        }
    }
}
