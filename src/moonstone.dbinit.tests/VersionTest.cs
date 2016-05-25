using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit.tests
{
    [TestFixture]
    public class VersionTest
    {
        [Test]
        public void Can_Compare()
        {
            var low = new Version(0, 0, 9);
            var high = new Version(1, 1, 1);

            low.Should().BeLessThan(high);
        }
    }
}