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
    public class EnumDescritpionLookupTest
    {
        
        public enum Status : short
        {
            [System.ComponentModel.Description("Currently waiting that you do something")]
            Waiting = 0,
            [System.ComponentModel.Description("Currently in action")]
            InAction = 1
        }

        public class StatusLookup : EnumDescriptionLookup<Status, short>
        {
            public StatusLookup()
                : base("Unknown")
            {

            }
        }

        [Test]
        public void Match_AdvancedExistingKey_ReturnValue()
        {
            var statusLookup = new StatusLookup();
            statusLookup.Load();
            var status = statusLookup.Match(0);

            Assert.That(status, Is.EqualTo("Currently waiting that you do something"));
        }
    }
}
