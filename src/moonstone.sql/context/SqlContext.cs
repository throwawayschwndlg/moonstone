using Dapper;
using moonstone.core.exceptions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace moonstone.sql.context
{
    public class SqlContext
    {
        private const string VERSION_TABLE = "db_version";

        /// <summary>
        /// Initializes a new context
        /// </summary>
        /// <param name="serverAddress">Connection String to the Server, without the database name</param>
        /// <param name="databaseName">The name of the database</param>
        public SqlContext(string databaseName, string serverAddress, bool integratedSecurity)
        {
            this.DatabaseName = databaseName;
            this.ServerAddress = serverAddress;
            this.IntegratedSecurity = integratedSecurity;
        }

        /// <summary>
        /// Holds the current SqlConnection.
        /// </summary>
        protected SqlConnection CurrentConnection { get; set; }

        /// <summary>
        /// Holds the current SqlTransaction
        /// </summary>
        protected SqlTransaction CurrentTransaction { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string DatabaseName { get; protected set; }

        /// <summary>
        /// Controls if integrated security should be used
        /// </summary>
        public bool IntegratedSecurity { get; protected set; }

        /// <summary>
        /// The connection string the the server
        /// </summary>
        public string ServerAddress { get; protected set; }

        /// <summary>
        /// Adds a record to the version table
        /// </summary>
        /// <param name="version">InstalledVersion to add</param>
        public void AddInstalledVersion(SqlInstalledVersion version)
        {
            this.CheckVersion(version);

            var command = this.BuildCommand(
                $@"INSERT INTO {VERSION_TABLE} (major, minor, revision, installDateUtc)
                    VALUES (@major, @minor, @revision, @installDateUtc);", true);

            using (var connection = this.OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Query(
                            sql: command,
                            transaction: transaction,
                            param: new
                            {
                                major = version.Major,
                                minor = version.Minor,
                                revision = version.Revision,
                                installDateUtc = version.InstallDateUtc
                            });
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new AddInstalledVersionException(
                            $"Failed to add installed version record", e);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the 'USE XYZ;' part to the command.
        /// </summary>
        /// <param name="command">The command without the 'USE XYZ' part</param>
        /// <param name="useSpecifiedDatabase">If true, the DatabaseName will be used, otherwise 'master'</param>
        /// <returns></returns>
        public string BuildCommand(string command, bool useSpecifiedDatabase)
        {
            string dbToUse = useSpecifiedDatabase ? this.DatabaseName : "master";

            return $"USE {dbToUse}; {command}";
        }

        /// <summary>
        /// Returns the connection string used to connect to the database
        /// </summary>
        /// <returns></returns>
        public string ConnectionString()
        {
            var connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = this.ServerAddress;
            connectionString.IntegratedSecurity = this.IntegratedSecurity;

            var c = connectionString.ToString();
            return connectionString.ToString();
        }

        /// <summary>
        /// Creates the database
        /// </summary>
        public void Create()
        {
            var command = this.BuildCommand(
                $"CREATE DATABASE {this.DatabaseName}",
                false);
            using (var connection = this.OpenConnection())
            {
                try
                {
                    var result = connection.Query(command);
                }
                catch (Exception e)
                {
                    throw new CreateDatabaseException(
                        $"Failed to create database", e);
                }
            }
        }

        /// <summary>
        /// Attempts to create the version table.
        /// </summary>
        public void CreateVersionTable()
        {
            if (this.VersionTableExists())
            {
                throw new VersionTableAlreadyExistsException(
                    $"The version table '{VERSION_TABLE}' already exists in the database.");
            }

            var command = this.BuildCommand(
                $@"CREATE TABLE {VERSION_TABLE} (
                        major INT NOT NULL,
                        minor INT NOT NULL,
                        revision INT NOT NULL,
                        installDateUtc DATETIME2 NOT NULL,
                        PRIMARY KEY (major, minor, revision)
                    );", true);

            try
            {
                using (var connection = this.OpenConnection())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            connection.Execute(sql: command, transaction: transaction);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new CreateVersionTableException(
                    $"Failed to create version table '{VERSION_TABLE}'.", e);
            }
        }

        /// <summary>
        /// Drops the database
        /// </summary>
        public void Drop()
        {
            var command = this.BuildCommand($"DROP DATABASE {this.DatabaseName}", false);
            using (var connection = this.OpenConnection())
            {
                try
                {
                    connection.Query(command);
                }
                catch (Exception e)
                {
                    throw new DropDatabaseException(
                        $"Failed to drop database", e);
                }
            }
        }

        /// <summary>
        /// Executes the specified script on the database. If successfull, the installed version will be added
        /// </summary>
        /// <param name="script"></param>
        public void ExecuteScript(SqlScript script)
        {
            var version = new SqlInstalledVersion(
                script.Version.Major,
                script.Version.Minor,
                script.Version.Revision,
                DateTime.UtcNow);
            this.CheckVersion(version);

            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = this.OpenConnection();

                transaction = script.UseTransaction ?
                    connection.BeginTransaction() :
                    null;

                var command = this.BuildCommand(script.Command, script.UseSpecifiedDatabase);
                connection.Query(sql: command, transaction: transaction);
                if (script.UseTransaction)
                {
                    transaction.Commit();
                }

                this.AddInstalledVersion(version);
            }
            catch (Exception e)
            {
                if (script.UseTransaction)
                {
                    transaction.Rollback();
                }

                throw new ExecuteScriptException(
                    $"Failed to execute script {script.Name}.", e);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
                if (script.UseTransaction && transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// Checks if the database exists
        /// </summary>
        /// <returns>True if exists, otherwise false</returns>
        public bool Exists()
        {
            using (var connection = this.OpenConnection())
            {
                var command = this.BuildCommand($"SELECT DB_ID('{this.DatabaseName}')", false);
                var result = connection.Query<int?>(command).SingleOrDefault();
                return result.HasValue;
            }
        }

        /// <summary>
        /// Returns the currently installed version
        /// </summary>
        /// <returns></returns>
        public SqlInstalledVersion GetInstalledVersion()
        {
            if (this.VersionTableExists())
            {
                var command = this.BuildCommand(
                    $"SELECT TOP(1) * FROM {VERSION_TABLE} ORDER BY major DESC, minor DESC, revision DESC", true);

                using (var connection = this.OpenConnection())
                {
                    try
                    {
                        var result = connection.Query<SqlInstalledVersion>(command).SingleOrDefault();

                        return result;
                    }
                    catch (Exception e)
                    {
                        throw new RetreiveInstalledVersionException(
                            $"Failed to retreive the installed version", e);
                    }
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Initializes the database and sets current version to 0.0.0
        /// </summary>
        public void Init()
        {
            try
            {
                if (!this.Exists())
                {
                    this.Create();
                }

                if (!this.VersionTableExists())
                {
                    this.CreateVersionTable();
                    this.AddInstalledVersion(new SqlInstalledVersion(0, 0, 0, DateTime.UtcNow));
                }
            }
            catch (Exception e)
            {
                throw new InitializeDatabaseException(
                    $"Failed to initialize the database", e);
            }
        }

        /// <summary>
        /// Returns an open SqlConnection
        /// </summary>
        /// <returns></returns>
        public SqlConnection OpenConnection()
        {
            if (this.CurrentConnection == null || this.CurrentConnection?.State == ConnectionState.Closed)
            {
                try
                {
                    this.CurrentConnection = new SqlConnection
                    {
                        ConnectionString = this.ConnectionString()
                    };
                }
                catch (Exception e)
                {
                    throw new InitializeSqlConnectionException(
                        $"Failed to initialize sql connection", e);
                }
            }

            try
            {
                if (this.CurrentConnection.State != System.Data.ConnectionState.Open)
                {
                    this.CurrentConnection.Open();
                }

                return this.CurrentConnection;
            }
            catch (Exception e)
            {
                throw new OpenConnectionException(
                    $"Failed to open connection.", e);
            }
        }

        /// <summary>
        /// Returns the @@VERSION of the SQL Server
        /// </summary>
        /// <returns>Server Version, eg. Microsoft SQL Server 2014 - 12.0.2269.0 (X64)   Jun 10 2015 03:35:45   Copyright (c) Microsoft Corporation  Express Edition (64-bit) on Windows NT 6.3 ...</returns>
        public string ServerVersion()
        {
            var command = this.BuildCommand($"SELECT @@VERSION", false);

            using (var connection = this.OpenConnection())
            {
                try
                {
                    var result = connection.Query<string>(command).SingleOrDefault();
                    return result;
                }
                catch (Exception e)
                {
                    throw new ReadDatabaseVerionException(
                        $"Failed to get database version", e);
                }
            }
        }

        /// <summary>
        /// Checks if a table exists on the server.
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="useSpecifiedDatabase">If true, query uses the Database specified in the contructor.
        /// If false, master database will be used.</param>
        /// <returns>True if table was found. Otherwise false.</returns>
        public bool TableExists(string table, bool useSpecifiedDatabase)
        {
            var command = this.BuildCommand(
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}';",
                useSpecifiedDatabase);

            try
            {
                using (var connection = this.OpenConnection())
                {
                    var res = connection.Query<int>(sql: command).SingleOrDefault();
                    return res == 1;
                }
            }
            catch (Exception e)
            {
                throw new MetaQueryException(
                    $"Failed to check for existence of table '{table}'.", e);
            }
        }

        /// <summary>
        /// Checks if the version table exists
        /// </summary>
        /// <returns>True if exists, otherwise false</returns>
        public bool VersionTableExists()
        {
            if (this.Exists())
            {
                return this.TableExists(VERSION_TABLE, useSpecifiedDatabase: true);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if there is a newer version installed. Throws AddLowerVersionException if
        /// equal or higher version was found.
        /// </summary>
        /// <param name="installedVersionToCheck"></param>
        protected void CheckVersion(SqlInstalledVersion installedVersionToCheck)
        {
            var currentInstalledVersion = this.GetInstalledVersion();
            if (currentInstalledVersion != null)
            {
                var currentVersion = currentInstalledVersion.GetVersion();
                var versionToCheck = installedVersionToCheck.GetVersion();

                if (currentVersion != null && currentVersion.CompareTo(versionToCheck) >= 0)
                {
                    throw new LowerOrEqualVersionException(
                        $"Attempt to add lower version. Current version is {currentVersion}");
                }
            }
        }

        /// <summary>
        /// Drops the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <param name="useSpecifiedDatabase">If true, the specified database will be used. Otherwise master.</param>
        public void DropTable(string tableName, bool useSpecifiedDatabase)
        {
            var command = this.BuildCommand($"DROP TABLE {tableName}", useSpecifiedDatabase: useSpecifiedDatabase);

            using (var connection = this.OpenConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Query(sql: command, transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new DropTableException(
                            $"Failed to drop table {tableName}", e);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the name of the version table.
        /// </summary>
        /// <returns></returns>
        public string VersionTableName()
        {
            return VERSION_TABLE;
        }

        /// <summary>
        /// Checks if the connection can be opened.
        /// </summary>
        /// <returns>True if connection can be opened, otherwise false</returns>
        public bool CanConnect()
        {
            try
            {
                this.OpenConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Begins a new transaction on the current connection
        /// </summary>
        public void BeginTransaction()
        {
            if (this.CurrentTransaction != null)
            {
                throw new TransactionAlreadyInitializedException(
                    $"Transaction was already initialized");
            }

            this.CurrentTransaction = this.CurrentConnection.BeginTransaction();
        }

        /// <summary>
        /// Commits the current transaction. Rolls back if commit failed.
        /// </summary>
        public void CommitTransaction()
        {
            if (this.CurrentTransaction == null)
            {
                throw new TransactionNotInitializedException(
                    $"The transaction is not initialized");
            }

            try
            {
                this.CurrentTransaction.Commit();
            }
            catch (Exception e)
            {
                this.CurrentTransaction.Rollback();

                throw new CommitTransactionException(
                    $"Failed to commit the transaction", e);
            }
            finally
            {
                this.CurrentTransaction.Dispose();
                this.CurrentTransaction = null;
            }
        }
    }
}