using moonstone.core.exceptions;
using System;
using System.IO;

namespace moonstone.dbinit
{
    public class Script
    {
        public string Name { get; set; }

        public string Command { get; set; }

        public Version Version { get; set; }

        public bool UseTransaction { get; set; }

        public bool UseSpecifiedDatabase { get; set; }

        public Script(string name, string command, Version version, bool useSpecifiedDatabase, bool useTransaction)
        {
            this.Name = name;
            this.Command = command;
            this.Version = version;
            this.UseSpecifiedDatabase = useSpecifiedDatabase;
            this.UseTransaction = useTransaction;
        }

        public static Script FromFile(string name, string path, Version version, bool useSpecifiedDatabase, bool useTransaction)
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

            return new Script(name, content, version, useSpecifiedDatabase, useTransaction);
        }
    }
}