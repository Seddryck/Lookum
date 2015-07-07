using Lookum.Framework.Lookup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Lookum.Framework.Testing.Lookup
{
    class DatabaseInsertLookupTest
    {
        [TearDown]
        public void Dispose()
        {
            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionStringReader.GetSqlClient();
                conn.Open();
                var sql = "delete from [state] where id>2;";
                var command = conn.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        public class StateLookup : DatabaseInsertLookup<string, int>
        {
            public StateLookup()
                : base()
            {
                ConnectionString = ConnectionStringReader.GetSqlClient();
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand()
            {
                var sql = "select label, id from [state];";
                var command = new SqlCommand();
                command.CommandText = sql;
                return command;
            }

            protected override IDbCommand BuildInsertCommand(string value)
            {
                var sql = "insert into [state] values (@label); select cast(SCOPE_IDENTITY() as int);";
                var command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("label", value));
                return command;
            }
        }

        [Test]
        public void Match_ExistingKey_ReturnValue()
        {
            var stateLookup = new StateLookup();
            stateLookup.Load();
            var state = stateLookup.Match("Started");

            Assert.That(state, Is.EqualTo(1));
        }

        [Test]
        public void Match_NonExistingKey_ReturnNewValue()
        {
            var stateLookup = new StateLookup();
            stateLookup.Load();
            var country = stateLookup.Match("Waiting");

            Assert.That(country, Is.GreaterThan(2));
        }

        [Test]
        public void Match_InitiatlyNonExistingKey_ReturnNewValue()
        {
            var stateLookup = new StateLookup();
            stateLookup.Load();
            var stateFirst = stateLookup.Match("Paused");
            var stateSecond = stateLookup.Match("Paused");

            Assert.That(stateFirst, Is.EqualTo(stateSecond));
        }
    }
}
