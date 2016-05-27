using FluentAssertions;
using moonstone.core.exceptions;
using moonstone.sql.context;
using NUnit.Framework;
using System;

namespace moonstone.sql.tests
{
    [TestFixture]
    public class SqlContextTest
    {
        private const string SERVER_ADDRESS = ".";
        private const bool INTEGRATED_SECURITY = true;
        private const string SERVER_CONNECTION_STRING = "Data Source=.;Integrated Security=True;";

        private const string DATABASE_NAME = "moonstone_tests";

        public static SqlContext GetValidContext()
        {
            return new SqlContext(DATABASE_NAME, SERVER_ADDRESS, INTEGRATED_SECURITY);
        }

        public static SqlContext GetInitializedContext()
        {
            var context = GetValidContext();
            context.Init();

            return context;
        }

        [SetUp]
        public void Setup()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }
            context.Create();
        }

        [TearDown]
        public void Teardown()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }
        }

        [Test]
        public void Can_Open_Valid_Connection()
        {
            var context = GetValidContext();

            var connection = context.OpenConnection();

            Assert.IsTrue(connection.State == System.Data.ConnectionState.Open);
        }

        [Test]
        public void Can_Read_Server_Version()
        {
            var context = GetValidContext();

            string version = context.ServerVersion();

            Assert.That(version.Contains("Microsoft SQL Server"));
        }

        [Test]
        public void Can_Find_DB()
        {
            var context = GetValidContext();

            Assert.IsTrue(context.Exists());
        }

        [Test]
        public void Can_Run_Multiple_Queries()
        {
            var context = GetValidContext();

            try
            {
                context.Exists();
                context.ServerVersion();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Cannot_Find_DB()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }

            Assert.IsFalse(context.Exists());
        }

        [Test]
        public void Can_Drop_DB()
        {
            var context = GetValidContext();
            try
            {
                context.Drop();
                Assert.IsFalse(context.Exists());
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Can_Create_DB()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }

            try
            {
                context.Create();
                Assert.IsTrue(context.Exists());
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void Returns_False_If_Version_Table_Does_Not_Exist()
        {
            var context = GetValidContext();

            bool exists = context.VersionTableExists();

            Assert.IsFalse(exists);
        }

        [Test]
        public void Returns_False_If_Database_Does_Not_Exist()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }

            Assert.IsFalse(context.Exists());

            Assert.IsFalse(context.VersionTableExists());
        }

        [Test]
        public void Can_Create_Version_Table()
        {
            var context = GetValidContext();

            context.CreateVersionTable();
            bool exists = context.VersionTableExists();

            Assert.IsTrue(exists);
        }

        [Test]
        public void Returns_True_If_Version_Table_Exists()
        {
            var context = GetValidContext();
            context.CreateVersionTable();

            bool exists = context.VersionTableExists();

            Assert.True(exists);
        }

        [Test]
        public void Can_Build_Command_Master()
        {
            var context = GetValidContext();
            string command = "SELECT 42";
            string expected = $"USE master; {command}";

            string result = context.BuildCommand(command, false);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Can_Build_Command_Specific()
        {
            var context = GetValidContext();
            string command = "SELECT 42";
            var expected = $"USE {DATABASE_NAME}; {command}";

            string result = context.BuildCommand(command, true);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Returns_Null_If_No_Version_Installed()
        {
            var context = GetValidContext();
            context.SqlVersion version = null;

            version = context.GetInstalledVersion();

            Assert.IsNull(version);
        }

        [Test]
        public void Can_Get_Latest_Version()
        {
            var context = GetInitializedContext();
            var ver1 = new SqlInstalledVersion(1, 0, 0, DateTime.Now);
            var ver2 = new SqlInstalledVersion(1, 2, 3, DateTime.Now);
            var ver3 = new SqlInstalledVersion(2, 0, 0, DateTime.Now);

            context.AddInstalledVersion(ver1);
            context.AddInstalledVersion(ver2);
            context.AddInstalledVersion(ver3);

            var installed = context.GetInstalledVersion();

            installed.ShouldBeEquivalentTo(ver3, options =>
                options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(ver3.InstallDateUtc)).WhenTypeIs<DateTime>());
        }

        [Test]
        public void Init_Creates_Database()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }

            Assert.IsFalse(context.Exists());

            context.Init();

            Assert.IsTrue(context.Exists());
        }

        [Test]
        public void Init_Creates_Version_Table()
        {
            var context = GetValidContext();
            if (context.Exists())
            {
                context.Drop();
            }

            Assert.IsFalse(context.VersionTableExists());

            context.Init();

            Assert.IsTrue(context.VersionTableExists());
        }

        [Test]
        public void Can_Add_Version()
        {
            var context = GetInitializedContext();
            var toAdd = new SqlInstalledVersion(1, 2, 3, DateTime.UtcNow);

            context.AddInstalledVersion(toAdd);

            var installed = context.GetInstalledVersion();

            installed.ShouldBeEquivalentTo(toAdd, options =>
                options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(toAdd.InstallDateUtc)).WhenTypeIs<DateTime>());
        }

        [Test]
        public void Can_Not_Add_Lower_Version()
        {
            var context = GetInitializedContext();
            var ver1 = new SqlInstalledVersion(2, 0, 0, DateTime.UtcNow);
            var ver2 = new SqlInstalledVersion(1, 9, 9, DateTime.UtcNow);
            context.AddInstalledVersion(ver1);

            Assert.Throws<LowerOrEqualVersionException>(() => context.AddInstalledVersion(ver2));
        }

        [Test]
        public void Can_Find_Specified_Table()
        {
            var context = GetInitializedContext();

            bool exists = context.TableExists(context.VersionTableName(), true);

            Assert.IsTrue(exists);
        }

        [Test]
        public void Can_Not_Find_Specified_Table()
        {
            var context = GetInitializedContext();

            bool exists = context.TableExists("somenonexistenttable", true);

            Assert.IsFalse(exists);
        }

        [Test]
        public void Can_Find_Unspecified_Table()
        {
            var context = GetInitializedContext();

            bool exists = context.TableExists("spt_monitor", false);

            Assert.IsTrue(exists);
        }

        [Test]
        public void Can_Not_Find_Unspecified_Table()
        {
            var context = GetInitializedContext();

            bool exists = context.TableExists("something", false);

            Assert.IsFalse(exists);
        }

        [Test]
        public void Can_Drop_Table_On_Specified()
        {
            var context = GetInitializedContext();

            bool existsBeforeDrop;
            bool existsAfterDrop;

            existsBeforeDrop = context.VersionTableExists();
            context.DropTable(context.VersionTableName(), useSpecifiedDatabase: true);
            existsAfterDrop = context.VersionTableExists();

            Assert.IsTrue(existsBeforeDrop);
            Assert.IsFalse(existsAfterDrop);
        }

        [Test]
        public void Can_Drop_Table_On_Unspecified()
        {
            string tableName = "hurr";
            var context = GetInitializedContext();

            bool existsBeforeDrop;
            bool existsAfterDrop;

            var createTableScript = new SqlScript("hurr_create",
                $@"CREATE TABLE {tableName} (
                    id INT NOT NULL,
                    [name] NVARCHAR(128) NOT NULL,
                    PRIMARY KEY(id)
                );",
                new context.SqlVersion(1, 1, 1),
                useSpecifiedDatabase: false,
                useTransaction: false);

            if (!context.TableExists(tableName, useSpecifiedDatabase: false))
            {
                context.ExecuteScript(createTableScript);
            }

            existsBeforeDrop = context.TableExists(tableName, useSpecifiedDatabase: false);
            context.DropTable(tableName, useSpecifiedDatabase: false);
            existsAfterDrop = context.TableExists(tableName, useSpecifiedDatabase: false);

            Assert.IsTrue(existsBeforeDrop);
            Assert.IsFalse(existsAfterDrop);
        }

        [Test]
        public void Can_Execute_On_Specified()
        {
            var context = SqlContextTest.GetInitializedContext();

            bool tableExistsBeforeExecution;
            bool tableExistsAfterExecution;

            string tableName = "TEST";

            var script = new SqlScript("ttt",
                $@"CREATE TABLE {tableName} (
                        id int not null,
                        [name] nvarchar(128) not null
                        PRIMARY KEY(id)
                    );

                    INSERT INTO {tableName} (id, [name]) values (1, 'test');",
                new context.SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            tableExistsBeforeExecution = context.TableExists(tableName, useSpecifiedDatabase: true);
            context.ExecuteScript(script);
            tableExistsAfterExecution = context.TableExists(tableName, useSpecifiedDatabase: true);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Execute_On_Unspecified()
        {
            var context = GetInitializedContext();

            bool tableExistsBeforeExecution;
            bool tableExistsAfterExecution;

            string tableName = "test";

            var script = new SqlScript("ttt",
                $@"CREATE TABLE {tableName} (
                        id int not null,
                        [name] nvarchar(128) not null
                        PRIMARY KEY(id)
                    );

                INSERT INTO {tableName} (id, [name]) values (1, 'test');",
                new context.SqlVersion(1, 0, 0),
                useSpecifiedDatabase: false,
                useTransaction: false);

            if (context.TableExists(tableName, useSpecifiedDatabase: false))
            {
                context.DropTable(tableName, useSpecifiedDatabase: false);
            }

            tableExistsBeforeExecution = context.TableExists(tableName, useSpecifiedDatabase: false);
            context.ExecuteScript(script);
            tableExistsAfterExecution = context.TableExists(tableName, useSpecifiedDatabase: false);
            context.DropTable(tableName, useSpecifiedDatabase: false);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Execute_With_Transaction()
        {
            string tableName = "animals";
            bool tableExistsBeforeExecution = false;
            bool tableExistsAfterExecution = false;
            var context = GetInitializedContext();
            var script = new SqlScript(
                "animals",
                $@"CREATE TABLE {tableName}(
                    id int not null,
                    [name] nvarchar(128) not null,
                    PRIMARY KEY (id)
                );

                INSERT INTO {tableName} (id, [name]) VALUES(1, 'dog');",
                version: new context.SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: true);

            tableExistsBeforeExecution = context.TableExists(tableName, true);
            context.ExecuteScript(script);
            tableExistsAfterExecution = context.TableExists(tableName, true);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Can_Execute_Without_Transaction()
        {
            string tableName = "animals";
            bool tableExistsBeforeExecution = false;
            bool tableExistsAfterExecution = false;
            var context = GetInitializedContext();
            var script = new SqlScript(
                "animals",
                $@"CREATE TABLE {tableName}(
                    id int not null,
                    [name] nvarchar(128) not null,
                    PRIMARY KEY (id)
                );

                INSERT INTO {tableName} (id, [name]) VALUES(1, 'dog');",
                version: new context.SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            tableExistsBeforeExecution = context.TableExists(tableName, true);
            context.ExecuteScript(script);
            tableExistsAfterExecution = context.TableExists(tableName, true);

            Assert.IsFalse(tableExistsBeforeExecution);
            Assert.IsTrue(tableExistsAfterExecution);
        }

        [Test]
        public void Cannot_Execute_Multiple_With_Same_Version()
        {
            var context = GetInitializedContext();

            var script1 = new SqlScript("script_1",
                $@"SELECT 1;",
                new context.SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            var script2 = new SqlScript("script_2",
                $@"SELECT 2;",
                new context.SqlVersion(1, 0, 0),
                useSpecifiedDatabase: true,
                useTransaction: false);

            context.ExecuteScript(script1);
            Assert.Throws<LowerOrEqualVersionException>(() => context.ExecuteScript(script2));
        }

        [Test]
        public void Can_Return_Version_Table_Name()
        {
            var context = GetInitializedContext();
            var versionTableName = context.VersionTableName();

            Assert.AreEqual("db_version", versionTableName);
        }

        [Test]
        public void Can_Add_Installed_Version_Without_Transaction()
        {
            var context = GetInitializedContext();
            var versionToInstall = new context.SqlVersion(1, 2, 3);
            var script = new SqlScript(
                "add_version",
                "select 42;",
                versionToInstall,
                useSpecifiedDatabase: true,
                useTransaction: false);

            context.ExecuteScript(script);

            var installedVersion = context.GetInstalledVersion().GetVersion();

            installedVersion.ShouldBeEquivalentTo(versionToInstall);
        }

        [Test]
        public void Can_Add_Installed_Version_With_Transaction()
        {
            var context = GetInitializedContext();
            var versionToInstall = new context.SqlVersion(1, 2, 3);
            var script = new SqlScript(
                "add_version",
                "select 42;",
                versionToInstall,
                useSpecifiedDatabase: true,
                useTransaction: true);

            context.ExecuteScript(script);

            var installedVersion = context.GetInstalledVersion().GetVersion();

            installedVersion.ShouldBeEquivalentTo(versionToInstall);
        }
    }
}