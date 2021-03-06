﻿using Lookum.Framework.Lookup;
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
    public class DatabaseLookupTest
    {
        public class CountryLookup : DatabaseLookup<string, string>
        {
            public CountryLookup()
                : base(true)
            {
                ConnectionString=ConnectionStringReader.GetSqlClient();
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

        public class WrongCountryLookup : DatabaseLookup<string, int>
        {
            public WrongCountryLookup()
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

        public class DuplicateCountryLookup : CountryLookup
        {
            public DuplicateCountryLookup()
                : base(){}
           
            protected override IDbCommand BuildCommand()
            {
                var sql = "select [CategoryValue].[Key], [CategoryValueTranslation].[Value] from [CategoryType] inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id] inner join [CategoryValueTranslation] on [CategoryValueTranslation].[CategoryValueId] = [CategoryValue].[Id] inner join [IsoLanguage] on [IsoLanguage].[Id] = [CategoryValueTranslation].[IsoLanguageId] where [IsoLanguage].[IsDefault]=1 and [CategoryType].[Value]=@Category";
                sql = sql + " union all " + sql;
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

        public class CurrencyDefaultLookup : DatabaseLookup<string, string>
        {

            public CurrencyDefaultLookup()
                : base((k) => k.ToLower().Substring(0, Math.Min(k.Length, 2)))
            {
                ConnectionString = ConnectionStringReader.GetSqlClient();
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand()
            {
                var sql = "select [CategoryValue].[Key], [CategoryValueTranslation].[Value] from [CategoryType] inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id] inner join [CategoryValueTranslation] on [CategoryValueTranslation].[CategoryValueId] = [CategoryValue].[Id] inner join [IsoLanguage] on [IsoLanguage].[Id] = [CategoryValueTranslation].[IsoLanguageId] where [IsoLanguage].[Code]=@IsoLanguage and [CategoryType].[Value]=@Category";
                var command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("Category", "Currency"));
                command.Parameters.Add(new SqlParameter("IsoLanguage", "fr"));
                return command;
            }
        }

        [Test]
        public void Match_ExistingKey_ReturnValue()
        {
            var countryLookup = new CountryLookup();
            countryLookup.Load();
            var country = countryLookup.Match("US");

            Assert.That(country, Is.EqualTo("United States of America"));
        }

        [Test]
        public void Match_NonExistingKey_ThrowException()
        {
            var countryLookup = new CountryLookup();
            countryLookup.Load();

            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => countryLookup.Match("XX"));
        }

        [Test]
        public void Match_ExistingKeyWithoutLanguageDefined_ReturnValue()
        {
            var currencyLookup = new CurrencyLookup();
            currencyLookup.Load();
            var currency = currencyLookup.Match("USD");

            Assert.That(currency, Is.EqualTo("US Dollar"));
        }

        [Test]
        public void Match_ExistingKeyWithLanguageDefined_ReturnValue()
        {
            var currencyLookup = new CurrencyLookup("fr");
            currencyLookup.Load();
            var currency = currencyLookup.Match("USD");

            Assert.That(currency, Is.EqualTo("Dollar américain"));
        }

        [Test]
        public void Match_NonExistingKeyWithLanguageDefined_ReturnUnknown()
        {
            var currencyLookup = new CurrencyLookup("fr");
            currencyLookup.Load();
            var currency = currencyLookup.Match("XXX");

            Assert.That(currency, Is.EqualTo("Unknown"));
        }

        [Test]
        public void Match_NonExistingKeyWithDefaultEqualToKeyToLower_ReturnKeyToLower()
        {
            var currencyLookup = new CurrencyDefaultLookup();
            currencyLookup.Load();
            var currency = currencyLookup.Match("XXX");

            Assert.That(currency, Is.EqualTo("xx"));
        }

        [Test]
        public void Load_WrongTypeForPrimitive_ThrowCorrectException()
        {
            var countryLookup = new WrongCountryLookup();
            var ex = Assert.Throws<InvalidCastException>(delegate { countryLookup.Load(); });
            Assert.That(ex.Message, Is.StringContaining("Int32"));
            Assert.That(ex.Message, Is.StringContaining("String"));
            Assert.That(ex.Message, Is.StringContaining("value"));
            Console.WriteLine(ex.Message);
        }

        [Test]
        public void Load_DuplicateKey_ThrowCorrectException()
        {
            var countryLookup = new DuplicateCountryLookup();
            var ex = Assert.Throws<InvalidOperationException>(delegate { countryLookup.Load(); });
            var countries = new [] {"US", "FR", "BE", "NL", "DE"};
            Assert.That(countries.Any(c => ex.Message.Contains("'" + c + "'")));
            Assert.That(ex.Message, Is.StringContaining(String.Format("{0} elements.", countries.Length)));
            Console.WriteLine(ex.Message);
        }
    }
}
