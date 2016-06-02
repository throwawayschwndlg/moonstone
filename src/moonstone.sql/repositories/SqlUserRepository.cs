using moonstone.core.exceptions;
using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.sql.context;
using System;
using System.Linq;

namespace moonstone.sql.repositories
{
    public class SqlUserRepository : SqlBaseRepository, IUserRepository
    {
        public SqlUserRepository(SqlContext context)
            : base(context)
        {
        }

        public void Create(User user)
        {
            try
            {
                var command =
                    $"INSERT INTO {this.Context.GetUsersTableName()}";
            }
            catch (Exception e)
            {
                throw new CreateUserException(
                    $"Failed to create user.", e);
            }
        }

        public User GetByEmail(string email)
        {
            try
            {
                var command =
                    $"SELECT * FROM {this.Context.GetUsersTableName()} WHERE email = @email;";

                var result = this.Context.RunCommand<User>(
                    command: command,
                    param: new { email = email },
                    mode: CommandMode.Read);

                return result.SingleOrDefault();
            }
            catch (Exception e)
            {
                throw new QueryUserException(
                    $"Failed to get user by email.", e);
            }
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}