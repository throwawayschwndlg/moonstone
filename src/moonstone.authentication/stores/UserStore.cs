using Microsoft.AspNet.Identity;
using moonstone.core.models;
using moonstone.core.repositories;
using System;
using System.Threading.Tasks;

namespace moonstone.authentication.stores
{
    public class UserStore : IUserStore<User, Guid>
    {
        public IUserRepository UserRepository { get; set; }

        public UserStore(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }

        public Task CreateAsync(User user)
        {
            try
            {
                this.UserRepository.Create(user);

                return Task.FromResult(0);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            try
            {
                return Task.FromResult(this.UserRepository.GetById(userId));
            }
            catch (Exception e)
            {
                return Task.FromException<User>(e);
            }
        }

        public Task<User> FindByNameAsync(string userName)
        {
            try
            {
                return Task.FromResult(this.UserRepository.GetByEmail(userName));
            }
            catch (Exception e)
            {
                return Task.FromException<User>(e);
            }
        }

        public Task UpdateAsync(User user)
        {
            try
            {
                this.UserRepository.Update(user);

                return Task.FromResult(0);
            }
            catch (Exception e)
            {
                return Task.FromException(e);
                throw;
            }
        }
    }
}