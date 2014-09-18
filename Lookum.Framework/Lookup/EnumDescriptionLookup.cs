using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Lookup
{
    public abstract class EnumDescriptionLookup<T, K> : EnumLookup <T, K>
    {
        protected EnumDescriptionLookup()
            : base()
        { }

        protected EnumDescriptionLookup(bool throwException)
            : base(throwException)
        { }

        protected EnumDescriptionLookup(string defaultValue)
            : base(defaultValue)
        {}

        protected EnumDescriptionLookup(Func<K, string> nonMatchBehavior)
            : base(nonMatchBehavior)
        { }

        protected override void OnLoad()
        {
            Type t = typeof(T);

            foreach (var enumValue in Enum.GetValues(t))
            {
                var key = (K)enumValue;
                var field = enumValue.GetType().GetField(enumValue.ToString());
                var value = string.Empty;
                if (null != field)
                {
                    var attrs = field.GetCustomAttributes
                            (typeof(System.ComponentModel.DescriptionAttribute), true);
                    if (attrs != null && attrs.Length == 1)
                        value =  ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description;
                }
                Map.Add(key, value);
            }
        }
    }
}
