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
        /// <summary>
        /// Holds the current SqlConnection.
        /// </summary>
        protected SqlConnection CurrentConnection { get; set; }

        /// <summary>
        /// The connection string the the server
        /// </summary>
        protected string ConnectionString { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        protected string DatabaseName { get; set; }


        /// <summary>
        /// Initializes a new context
        /// </summary>
        /// <param name="connectionString">Connection String to the Server, without the database name</param>
        /// <param name="dbName">The name of the database</param>
        public Context(string connectionString, string dbName)
        {
            this.ConnectionString = connectionString;
            this.DatabaseName = dbName;
        }

        /// <summary>
        /// Returns an open SqlConnection
        /// </summary>
        /// <returns></returns>
        public SqlConnection OpenConnection()
        {
            if (this.CurrentConnection == null || this.CurrentConnection.State == ConnectionState.Closed)
            {
                try
                {
                    this.CurrentConnection = new SqlConnection()
                    {
                        ConnectionString = this.ConnectionString
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
        public string Version()
        {
            var command = $"SELECT @@VERSION";

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
                var command = $"SELECT DB_ID('{this.DatabaseName}')";
                var result = connection.Query<int?>(command).SingleOrDefault();
                return result.HasValue;
            }
        }

        /// <summary>
        /// Drops the database
        /// </summary>
        public void Drop()
        {
            var command = $"DROP DATABASE {this.DatabaseName}";
            using (var connection = this.OpenConnection())
            {
                try
                {
                    connection.Query(command);
                }
                catch(Exception e)
                {
                    throw new DropDatabaseException(
                        $"Failed to drop database", e);
                }
            }
        }

        /// <summary>
        /// Creates the database
        /// </summary>
        public void Create()
        {
            var command = $"CREATE DATABASE {this.DatabaseName}";
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
    }
}
