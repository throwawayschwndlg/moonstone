using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit.tests
{
    [TestFixture]
    public class ScriptTest
    {
        [Test]
        public void Can_Init_From_File()
        {
            var path = "test_init_from_file.sql";
            var command = "SELECT 42";
            var version = new Version(1, 0, 0);

            File.WriteAllText(path, command);
            dbinit.Script script = null;

            script = dbinit.Script.FromFile(path, version);

            Assert.AreEqual(command, script.Command);
            Assert.AreEqual(version, script.Version);
        }
    }
}
