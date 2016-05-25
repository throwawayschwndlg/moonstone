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
    public class InstalledVersionTest
    {
        [Test]
        public void Can_Compare()
        {
            //var now = DateTime.UtcNow;
            //var tomorrow = DateTime.UtcNow.AddDays(1);

            //var low = new InstalledVersion(0, 0, 9, now);
            //var high = new InstalledVersion(1, 1, 1, now);

            //low.Should().BeLessThan(high);
            //high.Should().BeGreaterOrEqualTo(low);

            //low = new InstalledVersion(2, 0, 0, now);
            //high = new InstalledVersion(2, 0, 0, now);

            //low.ShouldBeEquivalentTo(high);

            //low = new InstalledVersion(3, 0, 0, now);
            //high = new InstalledVersion(3, 0, 0, tomorrow);

            //low.Should().BeLessThan(high);
            //high.Should().BeGreaterThan(low);
        }

        [Test]
        public void Can_Return_Version()
        {
            var installed = new InstalledVersion(1, 2, 3, DateTime.UtcNow);
            var expected = new Version(1, 2, 3);

            var actual = installed.GetVersion();

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}