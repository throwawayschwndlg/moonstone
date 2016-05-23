using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit
{
    public class Version
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }

        public Version(int major, int minor, int revision)
        {
            this.Major = major;
            this.Minor = minor;
            this.Revision = revision;
        }
    }
}
