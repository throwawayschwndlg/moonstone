using moonstone.core.exceptions;
using System;
using System.IO;

namespace moonstone.sql.context
{
    public class SqlScript
    {
        public string Name { get; set; }

        public string Command { get; set; }

        public SqlVersion Version { get; set; }

        public bool UseTransaction { get; set; }

        public bool UseSpecifiedDatabase { get; set; }

        public SqlScript(string name, string command, SqlVersion version, bool useSpecifiedDatabase, bool useTransaction)
        {
            this.Name = name;
            this.Command = command;
            this.Version = version;
            this.UseSpecifiedDatabase = useSpecifiedDatabase;
            this.UseTransaction = useTransaction;
        }

        public static SqlScript FromFile(string name, string path, SqlVersion version, bool useSpecifiedDatabase, bool useTransaction)
        {
            string content = null;

            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                throw new ReadFileException(
                    $"Failed to read contents from {path}", e);
            }

            return new SqlScript(name, content, version, useSpecifiedDatabase, useTransaction);
        }
    }
}