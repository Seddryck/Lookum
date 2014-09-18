using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lookum.Framework.Lookup
{
    public abstract class BaseLookup<K, V>
    {
        public bool IsLoaded { get; protected set; }

        /// <summary>
        /// Internal container to store in-memory the different keys/values to use in the lookup
        /// </summary>
        protected IDictionary<K, V> Map { get; private set; }
        /// <summary>
        /// Define the function to be executed when a key is requested but not found in the map.
        /// </summary>
        protected Func<K, V> NonMatchBehavior { get; private set; }

        protected BaseLookup()
        {
            IsLoaded = false;
            Map = new Dictionary<K, V>();
            NonMatchBehavior = delegate { return default(V); };
        }

        protected BaseLookup(bool throwException)
            : this()
        {
            if (throwException)
                NonMatchBehavior = ThrowException;
        }

        protected BaseLookup(V defaultValue)
            : this()
        {
            NonMatchBehavior = delegate {return defaultValue;};
        }

        protected BaseLookup(Func<K, V> nonMatchBehavior)
            : this()
        {
            NonMatchBehavior = nonMatchBehavior;
        }


        private V ThrowException(K id)
        {
            var msg = String.Format("The key '{0}' has not been found during the execution of the lookup filled with {1} values.", id, Map.Keys.Count);
            throw new KeyNotFoundException(msg);
        }

        
        /// <summary>
        /// Return the value associated to the key. If the key is not found apply the behavior specified in NonMatchBehavior
        /// </summary>
        /// <param name="id">The key (guid) that you're looking to translate to a value (string)</param>
        /// <returns>The translated value. IF the key has not been found and the behaviour is set to return a default value then the default value will returned.</returns>
        public virtual V Match(K id)
        {
            if (!IsLoaded)
                throw new NotLoadedLookupException();

            if (Map.ContainsKey(id))
                return Map[id];

            return NonMatchBehavior(id);
        }
    }
}
