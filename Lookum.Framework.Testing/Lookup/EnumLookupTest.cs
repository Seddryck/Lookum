using Lookum.Framework.Lookup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace Lookum.Framework.Testing.Lookup
{
    public class EnumLookupTest
    {
        public enum Cardinal
        {
            South = 0,
            North = 1,
            West = 2,
            East = 3
        }

        
        public class CardinalLookup : EnumLookup<Cardinal, int>
        {
            public CardinalLookup()
                : base("Unknown")
            {
                
            }
        }

        [Test]
        public void Match_ExistingKey_ReturnValue()
        {
            var cardinalLookup = new CardinalLookup();
            cardinalLookup.Load();
            var country = cardinalLookup.Match(1);

            Assert.That(country, Is.EqualTo("North"));
        }

        [Test]
        public void Match_NonExistingKey_ReturnDefault()
        {
            var cardinalLookup = new CardinalLookup();
            cardinalLookup.Load();
            var country = cardinalLookup.Match(5);

            Assert.That(country, Is.EqualTo("Unknown"));
        }

    }
}
