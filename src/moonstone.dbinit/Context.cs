using moonstone.core.exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Diagnostics;
using System.Data;

namespace moonstone.dbinit
{
    public class Context
    {
        const string VERSION_TABLE = "db_version";

        /// <summary>
        /// Holds the current SqlConnection.
        /// </summary>
        protected SqlConnection CurrentConnection { get; set; }

        /// <summary>
        /// The connection string the the server
        /// </summary>
        protected string ServerAddress { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        protected string DatabaseName { get; set; }

        /// <summary>
        /// Controls if integrated security should be used
        /// </summary>
        protected bool IntegratedSecurity { get; set; }


        /// <summary>
        /// Initializes a new context
        /// </summary>
        /// <param name="serverConnectionString">Connection String to the Server, without the database name</param>
        /// <param name="databaseName">The name of the database</param>
        public Context(string databaseName, string serverAddress, bool integratedSecurity)
        {
            this.DatabaseName = databaseName;
            this.ServerAddress = serverAddress;
            this.IntegratedSecurity = integratedSecurity;
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
                    this.CurrentConnection = new SqlConnection()
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

            if (this.CurrentConnection.State != System.Data.ConnectionState.Open)
            {
                this.CurrentConnection.Open();
            }

            return this.CurrentConnection;
        }

        /// <summary>
        /// Returns the @@VERSION of the SQL Server
        /// </summary>
        /// <returns>Server Version, eg. Microsoft SQL Server 2014 - 12.0.2269.0 (X64)   Jun 10 2015 03:35:45   Copyright (c) Microsoft Corporation  Express Edition (64-bit) on Windows NT 6.3 <X64> (Build 10586: )</returns>
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
        /// Attempts to create the version table.
        /// </summary>
        public void CreateVersionTable()
        {
            if(this.VersionTableExists())
            {
                throw new VersionTableAlreadyExistsException(
                    $"The version table '{VERSION_TABLE}' already exists in the database.");
            }

            var command = this.BuildCommand(
                $@"CREATE TABLE {VERSION_TABLE} (
                        major INT NOT NULL,
                        minor INT NOT NULL,
                        revision INT NOT NULL,
                        install_date DATETIME2 NOT NULL,
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
            catch(Exception e)
            {
                throw new CreateVersionTableException(
                    $"Failed to create version table '{VERSION_TABLE}'.", e);
            }
        }

        /// <summary>
        /// Checks if the version table exists
        /// </summary>
        /// <returns>True if exists, otherwise false</returns>
        public bool VersionTableExists()
        {
            var command = this.BuildCommand(
                $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{VERSION_TABLE}';",
                true);

            using (var connection = this.OpenConnection())
            {
                var result = connection.Query<int>(command).Single();

                return result == 1;
            }
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
    }
}
