using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Lookup
{
    public abstract class DatabaseIncrementalLookup<K, V> : DatabaseLookup<K, V>
    {
        /// <summary>
        /// Internal container to store in-memory the different keys/values that have not been found
        /// </summary>
        protected HashSet<K> SetNotFound { get; private set; }

        protected DatabaseIncrementalLookup()
            : base()
        { }

        protected DatabaseIncrementalLookup(bool throwException)
            : base(throwException)
        { }

        protected DatabaseIncrementalLookup(V defaultValue)
            : base(defaultValue)
        {}

        protected DatabaseIncrementalLookup(Func<K, V> nonMatchBehavior)
            : base(nonMatchBehavior)
        { }

        /// <summary>
        /// Load the Lookup to store in memory the keys/values requested and eventually found in the database
        /// </summary>
        protected override void OnLoad()
        {
            //do nothing!
        }

        public override V Match(K id)
        {
            if (SetNotFound == null)
                SetNotFound = new HashSet<K>();

            if (Map.ContainsKey(id))
                return Map[id];

            if (SetNotFound.Contains(id))
                return NonMatchBehavior(id);

            key = id;
            base.OnLoad();

            if (Map.ContainsKey(id))
                return Map[id];
            else
            {
                SetNotFound.Add(id);
                return NonMatchBehavior(id);
            }
        }

        protected K key;

        protected override IDbCommand BuildCommand()
        {
            return BuildCommand(key);
        }

        protected abstract IDbCommand BuildCommand(K key);

    }
}
