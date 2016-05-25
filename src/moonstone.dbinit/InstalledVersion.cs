using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.dbinit
{
    public class InstalledVersion : Version, IComparable<InstalledVersion>
    {
        public DateTime InstallDateUtc { get; set; }

        public InstalledVersion()
            : base()
        {
            this.InstallDateUtc = DateTime.MinValue;
        }

        public InstalledVersion(int major, int minor, int revision, DateTime installDateUtc)
            : base(major, minor, revision)
        {
            this.InstallDateUtc = installDateUtc;
        }

        public int CompareTo(InstalledVersion other)
        {
            int baseResult = base.CompareTo(other);

            if (baseResult == 0)
            {
                if (this.InstallDateUtc > other.InstallDateUtc)
                {
                    return 1;
                }
                else if (this.InstallDateUtc < other.InstallDateUtc)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return baseResult;
            }
        }

        public Version GetVersion()
        {
            return new Version(this.Major, this.Minor, this.Revision);
        }
    }
}