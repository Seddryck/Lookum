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
    public class DatabaseLookupWithClassTest
    {
        public class CurrencyLanguage
        {
            public string English { get; set; }
            public string French { get; set; }

            private static CurrencyLanguage unknown = new CurrencyLanguage() { English = "Unknown", French = "Inconnu" };
            public static CurrencyLanguage Unknown
            {
                get { return unknown; }
            }
        }

        public class CurrencyLookup : DatabaseLookup<string, CurrencyLanguage>
        {
            public CurrencyLookup()
                : base(CurrencyLanguage.Unknown)
            {
                ConnectionString = ConnectionStringReader.GetSqlClient();
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand()
            {
                var sql = "select [CategoryValue].[Key], [CategoryValueTranslationEnglish].[Value], [CategoryValueTranslationFrench].[Value] from [CategoryType] "
                    + " inner join [CategoryValue] on [CategoryValue].[CategoryTypeId] = [CategoryType].[Id]"
                    + " inner join [CategoryValueTranslation] [CategoryValueTranslationEnglish] on [CategoryValueTranslationEnglish].[CategoryValueId] = [CategoryValue].[Id]"
                    + " inner join [IsoLanguage] [IsoLanguageEnglish] on [IsoLanguageEnglish].[Id] = [CategoryValueTranslationEnglish].[IsoLanguageId] and [IsoLanguageEnglish].[Code]='en'"
                    + " inner join [CategoryValueTranslation] [CategoryValueTranslationFrench] on [CategoryValueTranslationFrench].[CategoryValueId] = [CategoryValue].[Id]"
                    + " inner join [IsoLanguage] [IsoLanguageFrench] on [IsoLanguageFrench].[Id] = [CategoryValueTranslationFrench].[IsoLanguageId] and [IsoLanguageFrench].[Code]='fr'" 
                    + " where [CategoryType].[Value]=@Category";
                var command = new SqlCommand();
                command.CommandText = sql;
                command.Parameters.Add(new SqlParameter("Category", "Currency"));
                return command;
            }

            protected override CurrencyLanguage BuildValue(object[] items)
            {
                return new CurrencyLanguage()
                {
                    English = (string)items[1],
                    French = (string)items[2]
                };
            }
        }


        public class ProductKey
        {
            public string @Class { get; set; }
            public string Subclass { get; set; }

            public override int GetHashCode()
            {
                return @Class.GetHashCode() ^ 67 * Subclass.GetHashCode();
            }

            public override bool Equals(object obj)
            {
 	             return 
                 (
                    this.Class.Equals((obj as ProductKey).Class)
                    && this.Subclass.Equals((obj as ProductKey).Subclass)
                 );
            }
            
        }

        public class ProductValue
        {
            public string Label { get; set; }
            public int Hours { get; set; }
        }

        public class ProductLookup : DatabaseLookup<ProductKey, ProductValue>
        {
            public ProductLookup()
                : base(true)
            {
                ConnectionString = ConnectionStringReader.GetSqlClient();
                CommandTimeOut = 300;
            }

            protected override IDbCommand BuildCommand()
            {
                var sql = "select [Class], [SubClass], [Label], [Quantity] from [MultiKeyValue]";
                var command = new SqlCommand();
                command.CommandText = sql;
                return command;
            }

            protected override ProductKey BuildKey(object[] items)
            {
                return new ProductKey()
                {
                    @Class = (string)items[0],
                    Subclass = (string)items[1]
                };
            }

            protected override ProductValue BuildValue(object[] items)
            {
                return new ProductValue()
                {
                    Label = (string)items[2],
                    Hours = (int)items[3]
                };
            }
        }

        [Test]
        public void Match_ExistingKey_ReturnValue()
        {
            var currencyLookup = new CurrencyLookup();
            currencyLookup.Load();
            var currency = currencyLookup.Match("USD");

            Assert.That(currency.English, Is.EqualTo("US Dollar"));
            Assert.That(currency.French, Is.EqualTo("Dollar américain"));
        }

        
        [Test]
        public void Match_NonExistingKey_ReturnUnknown()
        {
            var currencyLookup = new CurrencyLookup();
            currencyLookup.Load();
            var currency = currencyLookup.Match("XXX");

            Assert.That(currency.English, Is.EqualTo("Unknown"));
            Assert.That(currency.French, Is.EqualTo("Inconnu"));
        }

        [Test]
        public void Match_NonExistingKeyTwice_ReturnTwoUnknownWithSameReference()
        {
            var currencyLookup = new CurrencyLookup();
            currencyLookup.Load();
            var firstCurrency = currencyLookup.Match("XXX");
            var secondCurrency = currencyLookup.Match("YYY");

            Assert.That(firstCurrency, Is.EqualTo(secondCurrency));
        }

        [Test]
        public void Match_ExistingCompositeKey_ReturnValue()
        {
            var productLookup = new ProductLookup();
            productLookup.Load();
            var product = productLookup.Match(new ProductKey {@Class="Training", Subclass="SQL"});

            Assert.That(product.Label, Is.EqualTo("SQL training"));
            Assert.That(product.Hours, Is.EqualTo(40));
        }

        [Test]
        public void Match_NonExistingCompositeKey_ThrowException()
        {
            var productLookup = new ProductLookup();
            productLookup.Load();
            var product =

            Assert.Throws<KeyNotFoundException>(delegate { productLookup.Match(new ProductKey { @Class = "Training", Subclass = "SPARQL" }); });
        }
    }
}
