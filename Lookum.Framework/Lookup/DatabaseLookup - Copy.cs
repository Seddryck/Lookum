using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;


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
                        //Mapping of the two fields to a map container.
                        var id = (K)dr.GetValue(0);
                        var value = (V)dr.GetValue(1);
                        Map.Add(id, value);
                    }
                }
            }
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
    }
}
