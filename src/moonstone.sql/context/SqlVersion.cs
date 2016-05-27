using System;

namespace moonstone.sql.context
{
    public class SqlVersion : IComparable<SqlVersion>
    {
        public int Major { get; set; }

        public int Minor { get; set; }

        public int Revision { get; set; }

        public SqlVersion()
        {
            this.Major = 0;
            this.Minor = 0;
            this.Revision = 0;
        }

        public SqlVersion(int major, int minor, int revision)
        {
            this.Major = major;
            this.Minor = minor;
            this.Revision = revision;
        }

        public int CompareTo(SqlVersion other)
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