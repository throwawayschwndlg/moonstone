using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit
{
    public class Version : IComparable<Version>
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }

        public Version()
        {
            this.Major = 0;
            this.Minor = 0;
            this.Revision = 0;
        }

        public Version(int major, int minor, int revision)
        {
            this.Major = major;
            this.Minor = minor;
            this.Revision = revision;
        }

        public int CompareTo(Version other)
        {
            if (this.Major > other.Major)
            {
                return 1;
            }
            else if (this.Major < other.Major)
            {
                return -1;
            }
            else
            {
                if (this.Minor > other.Minor)
                {
                    return 1;
                }
                else if (this.Minor < other.Minor)
                {
                    return -1;
                }
                else
                {
                    if (this.Revision > other.Revision)
                    {
                        return 1;
                    }
                    else if (this.Revision < other.Revision)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}