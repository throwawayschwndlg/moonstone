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
    public class ContextTests
    {
        const string SERVER_CONNECTION_STRING = "Data Source=.;Integrated Security=True;";

        const string DATABASE_NAME = "moonstone";

        [Test]
        public void Open_Throws_On_Invalid_ConnectionString()
        {
            var connectionString = "Invalid";
            Context context = new Context(connectionString, DATABASE_NAME);

            Assert.Throws<InitializeSqlConnectionException>(() => context.OpenConnection());
        }

        [Test]
        public void Can_Open_Valid_Connection()
        {
            var context = new Context(SERVER_CONNECTION_STRING, DATABASE_NAME);

            var connection = context.OpenConnection();

            Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
        }

        [Test]
        public void Can_Read_Server_Version()
        {
            var context = new Context(SERVER_CONNECTION_STRING, DATABASE_NAME);

            string version = context.Version();

            Assert.That(version.Contains("Microsoft SQL Server"));
        }

        [Test]
        public void Can_Find_DB()
        {
            var context = new Context(SERVER_CONNECTION_STRING, "master");

            Assert.IsTrue(context.Exists());
        }

        [Test]
        public void Can_Run_Multiple_Queries()
        {
            var context = new Context(SERVER_CONNECTION_STRING, DATABASE_NAME);

            try
            {
                context.Exists();
                context.Version();
            }
            catch(Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Cannot_Find_DB()
        {
            var context = new Context(SERVER_CONNECTION_STRING, "nonexistent");

            Assert.IsFalse(context.Exists());
        }

        [Test]
        public void Can_Drop_DB()
        {
            var context = new Context(SERVER_CONNECTION_STRING, DATABASE_NAME);
            if(!context.Exists())
            {
                context.Create();
            }

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
            var context = new Context(SERVER_CONNECTION_STRING, DATABASE_NAME);
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
    }
}
