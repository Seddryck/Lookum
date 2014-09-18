using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lookum.Framework.Lookup
{
    public class  EnumDecorationLookup<T> : EnumLookup<T>
    {
        protected string Decoration { get; private set; }

        protected EnumDecorationLookup(string decoration)
            : base()
        {
            Decoration = decoration;
        }

        protected EnumDecorationLookup(string decoration, bool throwException)
            : base()
        {
            Decoration = decoration;
        }

        protected EnumDecorationLookup(string decoration, string defaultValue)
            : base()
        {
            Decoration = decoration;
        }

        protected EnumDecorationLookup(string decoration, Func<int, string> nonMatchBehavior)
            : base()
        {
            Decoration = decoration;
        }

        
        
        public virtual void Load()
        {
            LoadFromDecoration(Decoration);
        }


        public void LoadFromDecoration(string decoration)
        {
            Type t = typeof(T);
            foreach (var v in Enum.GetValues(t))
                Map.Add(Convert.ToInt32(v), reflectedValue);
        }
    }
}
