using moonstone.core.exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit.tests
{
    [TestFixture]
    public class ContextTest
    {
        const string SERVER_ADDRESS = ".";
        const bool INTEGRATED_SECURITY = true;
        const string SERVER_CONNECTION_STRING = "Data Source=.;Integrated Security=True;";

        const string DATABASE_NAME = "moonstone_tests";

        public Context GetValidContext()
        {
            return new Context(DATABASE_NAME, SERVER_ADDRESS, INTEGRATED_SECURITY);
        }

        [SetUp]
        public void Setup()
        {
            var context = this.GetValidContext();
            if(context.Exists())
            {
                context.Drop();
            }
            context.Create();
        }

        [TearDown]
        public void Teardown()
        {
            var context = this.GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }
        }

        [Test]
        public void Can_Open_Valid_Connection()
        {
            var context = this.GetValidContext();

            var connection = context.OpenConnection();

            Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
        }

        [Test]
        public void Can_Read_Server_Version()
        {
            var context = this.GetValidContext();

            string version = context.ServerVersion();

            Assert.That(version.Contains("Microsoft SQL Server"));
        }

        [Test]
        public void Can_Find_DB()
        {
            var context = this.GetValidContext();

            Assert.IsTrue(context.Exists());
        }

        [Test]
        public void Can_Run_Multiple_Queries()
        {
            var context = this.GetValidContext();

            try
            {
                context.Exists();
                context.ServerVersion();
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Cannot_Find_DB()
        {
            var context = this.GetValidContext();
            if(context.Exists())
            {
                context.Drop();
            }

            Assert.IsFalse(context.Exists());
        }

        [Test]
        public void Can_Drop_DB()
        {
            var context = this.GetValidContext();
            try
            {
                context.Drop();
                Assert.IsFalse(context.Exists());
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Can_Create_DB()
        {
            var context = this.GetValidContext();
            if(context.Exists())
            {
                context.Drop();
            }

            try
            {
                context.Create();
                Assert.IsTrue(context.Exists());
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Returns_False_If_Version_Table_Does_Not_Exist()
        {
            var context = this.GetValidContext();

            bool exists = context.VersionTableExists();

            Assert.IsFalse(exists);
        }

        [Test]
        public void Can_Create_Version_Table()
        {
            var context = this.GetValidContext();

            context.CreateVersionTable();
            bool exists = context.VersionTableExists();

            Assert.IsTrue(exists);
        }

        [Test]
        public void Returns_True_If_Version_Table_Exists()
        {
            var context = this.GetValidContext();
            context.CreateVersionTable();

            bool exists = context.VersionTableExists();

            Assert.True(exists);
        }

        [Test]
        public void Can_Build_Command_Master()
        {
            var context = this.GetValidContext();
            string command = "SELECT 42";
            string expected = $"USE master; {command}";

            string result = context.BuildCommand(command, false);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Can_Build_Command_Specific()
        {
            var context = this.GetValidContext();
            string command = "SELECT 42";
            var expected = $"USE {DATABASE_NAME}; {command}";

            string result = context.BuildCommand(command, true);

            Assert.AreEqual(expected, result);
        }
    }
}
