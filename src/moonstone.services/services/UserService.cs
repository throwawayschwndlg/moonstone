using moonstone.core.exceptions.serviceExceptions;
using moonstone.core.i18n;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class UserService : BaseService, IUserService
    {
        protected IGroupService GroupService { get; set; }

        public UserService(RepositoryHub repoHub, IGroupService groupService)
            : base(repoHub)
        {
            this.GroupService = groupService;
        }

        public User CreateUser(User user)
        {
            var userId = this.Repositories.UserRepository.Create(user);
            return this.Repositories.UserRepository.GetById(userId);
        }

        public User GetUerByEmail(string email)
        {
            return this.Repositories.UserRepository.GetByEmail(email);
        }

        public User GetUserById(Guid userId)
        {
            return this.Repositories.UserRepository.GetById(userId);
        }

        public void SetCulture(Guid userId, string culture)
        {
            var user = this.GetUserById(userId);
            user.Culture = culture;
            this.UpdateUser(user);
        }

        public void SetCurrentGroup(Guid userId, Guid groupId)
        {
            var user = this.GetUserById(userId);
            if (user != null)
            {
                if (this.GroupService.IsUserInGroup(userId, groupId))
                {
                    try
                    {
                        user.CurrentGroupId = groupId;
                        this.UpdateUser(user);
                    }
                    catch (Exception e)
                    {
                        throw new UpdateUserException(
                            $"Failed to update user with id {userId}.", e);
                    }
                }
                else
                {
                    throw new UserNotInGroupException(
                        $"Current group will not be set since user with id {userId} is not in group with id {groupId}.");
                }
            }
            else
            {
                throw new UserNotFoundException(
                    $"User with id {userId} was not found.");
            }
        }

        public void SetTimeZone(Guid userId, string timeZoneId)
        {
            if (!TimeZoneUtils.GetAvailableTimeZones().Contains(timeZoneId, StringComparer.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException($"TimeZone \"{timeZoneId}\" is not registered.");
            }

            var user = this.GetUserById(userId);
            user.TzdbTimeZoneId = timeZoneId;
            this.UpdateUser(user);
        }

        public void UpdateSettings(Guid userId, string timeZoneId, bool autoUpdateTimeZone)
        {
            var user = this.GetUserById(userId);
            user.TzdbTimeZoneId = timeZoneId;
            user.AutoUpdateTimeZone = autoUpdateTimeZone;

            this.UpdateUser(user);
        }

        protected void UpdateUser(User user)
        {
            this.Repositories.UserRepository.Update(user);
        }
    }
}