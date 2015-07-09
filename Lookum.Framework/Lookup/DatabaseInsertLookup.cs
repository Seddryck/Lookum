using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Lookum.Framework.Lookup
{
    public abstract class DatabaseInsertLookup<K, V> : DatabaseLookup<K, V>
    {
        protected DatabaseInsertLookup()
            : base()
        {
            NonMatchBehavior = InsertElementNotFound;
        }

        protected V InsertElementNotFound(K key)
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (var cmd = BuildInsertCommand(conn, CommandTimeOut, key))
                {
                    conn.Open();
                    var item = cmd.ExecuteScalar();
                    var value = BuildPrimitiveValue(new object[] {item});
                    Map.Add(key, value);
                    return value;
                }
            }
        }

        protected IDbCommand BuildInsertCommand(SqlConnection conn, int commandTimeout, K key)
        {
            var cmd = BuildInsertCommand(key);
            cmd.Connection = conn;

            if (commandTimeout >= 0)
                cmd.CommandTimeout = commandTimeout;

            return cmd;
        }

        protected abstract IDbCommand BuildInsertCommand(K key);

    }
}
