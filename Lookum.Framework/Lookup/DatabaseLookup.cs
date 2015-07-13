using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Diagnostics;


namespace Lookum.Framework.Lookup
{
    public abstract class DatabaseLookup<K, V> : BaseLookup<K, V>
    {
        protected string ConnectionString { get; set; }
        protected int CommandTimeOut { get; set; } 

        protected DatabaseLookup()
            : base()
        { }

        protected DatabaseLookup(bool throwException)
            : base(throwException)
        { }

        protected DatabaseLookup(V defaultValue)
            : base(defaultValue)
        {}

        protected DatabaseLookup(Func<K, V> nonMatchBehavior)
            : base(nonMatchBehavior)
        { }
        

        /// <summary>
        /// Load the Lookup to store in memory all the keys/values found in the database
        /// </summary>
        /// <param name="connectionString">The connectionString to use to connect to the database</param>
        /// <param name="commandTimeout">Maximum time allowed to execute the query before throwing an error</param>
        public virtual void Load(string connectionString, int commandTimeout)
        {
            ConnectionString = connectionString;
            CommandTimeOut = commandTimeout;
        }

        public void Load(string connectionString)
        {
            this.Load(connectionString, -1);
        }

        protected override void OnLoad()
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (var cmd = BuildCommand(conn, CommandTimeOut))
                {
                    //Effective execution of the query
                    conn.Open();
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        K id;
                        V value;

                        var items = new List<object>();
                        for (int i = 0; i < dr.FieldCount; i++)
                            items.Add(dr.GetValue(i));

                        //Mapping of the two fields to a map container.
                        if (IsPrimitive(typeof(K)))
                            id = BuildPrimitiveKey(items.ToArray());
                        else
                            id = BuildKey(items.ToArray());

                        if (IsPrimitive(typeof(V)))
                            value = BuildPrimitiveValue(items.ToArray());
                        else
                            value = BuildValue(items.ToArray());

                        if (Map.Keys.Contains(id))
                            ManageDuplicateKey(id, value);                            

                        Map.Add(id, value);
                    }
                }
            }
        }

        protected bool IsPrimitive(Type t)
        {
            return t.IsPrimitive || t.IsValueType || (t == typeof(string));
        }

        protected virtual K BuildKey(object[] items)
        {
            throw new ArgumentException("By default, Lookum is expecting a primitive for the key. If you want to return an object in place of a primitive for the value, you need to override the method 'BuildKey' of your database lookup.");
        }

        private Z BuildPrimitive<Z>(object item)
        {
            try
            {
                return (Z)item;
            }
            catch (InvalidCastException ex)
            {
                var stackTrace = new StackTrace();
                var primitiveRole = stackTrace.GetFrame(1).GetMethod().Name.Replace("BuildPrimitive", "").ToLower();

                throw new InvalidCastException(
                    String.Format(
                            "The specified type for the {2} of the lookup ({0}) doesn't match with the type returned by the database ({1})"
                            , typeof(Z).Name
                            , item.GetType().Name
                            , primitiveRole
                            )
                    , ex);
            }
        }

        protected virtual K BuildPrimitiveKey(object[] items)
        {
            return BuildPrimitive<K>(items[0]);
        }

        protected virtual V BuildValue(object[] items)
        {
            throw new ArgumentException("By default, Lookum is expecting a primitive for the value. If you want to return an object in place of a primitive for the value, you need to override the method 'BuildValue' of your database lookup.");
        }

        protected virtual V BuildPrimitiveValue(object[] items)
        {
            return BuildPrimitive<V>(items[items.Length-1]);
        }

        protected IDbCommand BuildCommand(SqlConnection conn, int commandTimeout)
        {
            var cmd = BuildCommand();
            cmd.Connection = conn;

            if (commandTimeout >= 0)
                cmd.CommandTimeout = commandTimeout;

            return cmd;
        }

        protected abstract IDbCommand BuildCommand();

        protected string ReadResourceContent(string filename)
        {
            if (!filename.Contains('.'))
                filename += ".sql";
            
            var path = String.Format("{0}.{1}.{2}", this.GetType().Namespace, "Resources", filename);
            var content = string.Empty;

            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }

        protected virtual void ManageDuplicateKey(K id, V value)
        {
            var msg = String.Format("The key '{0}' has already been added to the lookup and you're trying to add it a second time. The lookup currently contains {1} element{2}."
                                                        , id
                                                        , Map.Count()
                                                        , Map.Count() > 1 ? "s" : string.Empty
                                                    );
            throw new InvalidOperationException(msg);
        }
    }
}
