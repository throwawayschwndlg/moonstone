using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface IUserService
    {
        User CreateUser(User user);

        User GetUerByEmail(string email);

        User GetUserById(Guid userId);

        void SetCulture(Guid userId, string culture);

        void SetCurrentGroup(Guid userId, Guid groupId);

        void SetTimeZone(Guid userId, string timeZoneId);

        void UpdateSettings(Guid userId, string timeZoneId, bool autoUpdateTimeZone);
    }
}