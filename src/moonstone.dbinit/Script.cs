using moonstone.core.exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit
{
    public class Script
    {
        public string Command { get; set; }

        public Version Version { get; set; }

        public Script(string command, Version version)
        {
            this.Command = command;
            this.Version = version;
        }

        public static Script FromFile(string path, Version version)
        {
            string content = null;

            try
            {
                content = File.ReadAllText(path);
            }
            catch(Exception e)
            {
                throw new ReadFileException(
                    $"Failed to read contents from {path}", e);
            }

            return new Script(content, version);
        }
    }
}
