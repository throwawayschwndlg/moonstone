using Microsoft.AspNet.Identity;
using moonstone.authentication.models;
using System;
using System.Threading.Tasks;

namespace moonstone.authentication.stores
{
    public class UserStore<TUser> :
        IUserStore<TUser, Guid>
        where TUser : IdentityUser
    {
        public Task CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }
    }
}