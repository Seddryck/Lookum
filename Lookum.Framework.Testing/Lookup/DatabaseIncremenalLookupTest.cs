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
    public class DatabaseIncrementalLookupTest
    {
        public class CountryLookup : DatabaseIncrementalLookup<string, string>
        {
            public CountryLookup()
                : base("Unknown")
            {
                ConnectionString = @"Data Source=.\sql2014;Initial Catalog=Lookum_Testing;Integrated Security=true";
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand(string key)
            {
                var sql = "select [CategoryValue].[Key], [CategoryValueTranslation].[Value] from [CategoryType] inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id] inner join [CategoryValueTranslation] on [CategoryValueTranslation].[CategoryValueId] = [CategoryValue].[Id] inner join [IsoLanguage] on [IsoLanguage].[Id] = [CategoryValueTranslation].[IsoLanguageId] where [IsoLanguage].[IsDefault]=1 and [CategoryType].[Value]=@Category and [CategoryValue].[Key]=@Key";
                var command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("Category", "Country"));
                command.Parameters.Add(new SqlParameter("Key", key));
                return command;
            }
        }

        [Test]
        public void Match_IncrementalExistingKeys_ReturnValue()
        {
            var countryLookup = new CountryLookup();
            countryLookup.Load();
            var country = countryLookup.Match("US");
            Assert.That(country, Is.EqualTo("United States of America"));

            country = countryLookup.Match("BE");
            Assert.That(country, Is.EqualTo("Belgium"));

            country = countryLookup.Match("US");
            Assert.That(country, Is.EqualTo("United States of America"));
        }

        [Test]
        public void Match_IncrementalExistingKeys_IsReallyIncremental()
        {
            var countryLookup = new CountryLookup();
            countryLookup.Load();
            Assert.That(countryLookup.Count(), Is.EqualTo(0));

            var country = countryLookup.Match("US");
            Assert.That(countryLookup.Count(), Is.EqualTo(1));

            country = countryLookup.Match("BE");
            Assert.That(countryLookup.Count(), Is.EqualTo(2));

            country = countryLookup.Match("US");
            Assert.That(countryLookup.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Match_IncrementalNonExistingKeys_ReturnValue()
        {
            var countryLookup = new CountryLookup();
            countryLookup.Load();
            var country = countryLookup.Match("XX");
            Assert.That(country, Is.EqualTo("Unknown"));

            country = countryLookup.Match("YY");
            Assert.That(country, Is.EqualTo("Unknown"));

            country = countryLookup.Match("ZZ");
            Assert.That(country, Is.EqualTo("Unknown"));
        }

        [Test]
        public void Match_IncrementalNonExistingKeys_IsReallyIncremental()
        {
            var countryLookup = new CountryLookup();
            countryLookup.Load();
            Assert.That(countryLookup.Count(), Is.EqualTo(0));

            var country = countryLookup.Match("US");
            Assert.That(countryLookup.Count(), Is.EqualTo(1));

            country = countryLookup.Match("XX");
            Assert.That(countryLookup.Count(), Is.EqualTo(1));

            country = countryLookup.Match("US");
            Assert.That(countryLookup.Count(), Is.EqualTo(1));
        }
    }
}
