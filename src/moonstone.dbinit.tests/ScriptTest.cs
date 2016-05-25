using NUnit.Framework;
using System.IO;

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

            script = dbinit.Script.FromFile("test", path, version, useSpecifiedDatabase: true, useTransaction: false);

            Assert.AreEqual(command, script.Command);
            Assert.AreEqual(version, script.Version);
        }
    }
}