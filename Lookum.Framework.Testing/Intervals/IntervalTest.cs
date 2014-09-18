using Lookum.Framework.Intervals;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lookum.Framework.Testing.Intervals
{
    public class IntervalTest
    {
        public class MyObject
        {
            public DateTime StartLife { get; set; }
            public DateTime EndLife { get; set; }
            public DateTime StartSecondLife { get; set; }
            public DateTime EndSecondLife { get; set; }

        }

        [Test]
        public void Intersect_TwoIntervals_CorrectResult()
        {
            var myObject = new MyObject()
            {
                StartLife = new DateTime(2014, 1, 1),
                EndLife = new DateTime(2015, 1, 1),
                StartSecondLife = new DateTime(2014, 6, 1),
                EndSecondLife = new DateTime(2015, 6, 1),
            };

            var result = Interval<DateTime>.Intersect
                (
                    new List<DateTime>() {myObject.StartLife, myObject.StartSecondLife},
                    new List<DateTime>() {myObject.EndLife, myObject.EndSecondLife}
                );

            Assert.That(result.Start, Is.EqualTo(new DateTime(2014, 6, 1)));
            Assert.That(result.End, Is.EqualTo(new DateTime(2015, 1, 1)));
        }
    }
}
