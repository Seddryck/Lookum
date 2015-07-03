using Lookum.Framework.Lookup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Testing.Lookup
{
    class LookupContainerTest
    {
        public class CountryLookup : DatabaseLookup<string, string>
        {
            public CountryLookup()
                : base(true)
            {
                ConnectionString = ConnectionStringReader.GetSqlClient();
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand()
            {
                var sql = "select [CategoryValue].[Key], [CategoryValueTranslation].[Value] from [CategoryType] inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id] inner join [CategoryValueTranslation] on [CategoryValueTranslation].[CategoryValueId] = [CategoryValue].[Id] inner join [IsoLanguage] on [IsoLanguage].[Id] = [CategoryValueTranslation].[IsoLanguageId] where [IsoLanguage].[IsDefault]=1 and [CategoryType].[Value]=@Category";
                var command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("Category", "Country"));
                return command;
            }
        }

        public class CurrencyLookup : DatabaseLookup<string, string>
        {
            private string isoLanguage;

            public CurrencyLookup(string isoLanguage = "en")
                : base("Unknown")
            {
                this.isoLanguage = isoLanguage;
                ConnectionString = ConnectionStringReader.GetSqlClient();
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand()
            {
                var sql = "select [CategoryValue].[Key], [CategoryValueTranslation].[Value] from [CategoryType] inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id] inner join [CategoryValueTranslation] on [CategoryValueTranslation].[CategoryValueId] = [CategoryValue].[Id] inner join [IsoLanguage] on [IsoLanguage].[Id] = [CategoryValueTranslation].[IsoLanguageId] where [IsoLanguage].[Code]=@IsoLanguage and [CategoryType].[Value]=@Category";
                var command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("Category", "Currency"));
                command.Parameters.Add(new SqlParameter("IsoLanguage", isoLanguage));
                return command;
            }
        }

        [Test]
        public void Load_TwoLookups_CorrectlyLoaded()
        {
            var container = new LookupContainer();
            container.Add(new CountryLookup());
            container.Add(new CurrencyLookup());
            container.Load();

            foreach (var lookup in container)
                Assert.That(lookup.Count(), Is.GreaterThan(0));
        }

        public void Find_TwoLookups_CorrectlyFound()
        {
            var container = new LookupContainer();
            container.Add(new CountryLookup());
            container.Add(new CurrencyLookup());
            var country = container.Find<CountryLookup>();

            Assert.That(country, Is.Not.Null);
            Assert.That(country, Is.TypeOf<CountryLookup>());
        }

        public void Find_OneLookupFindNonExisting_CorrectlyFound()
        {
            var container = new LookupContainer();
            container.Add(new CurrencyLookup());
            var country = container.Find<CountryLookup>();

            Assert.That(country, Is.Null);
        }
    }
}
