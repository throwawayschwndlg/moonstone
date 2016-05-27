using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.sql.context;
using System;

namespace moonstone.sql.repositories
{
    public class SqlUserRepository : SqlBaseRepository, IUserRepository
    {
        public SqlUserRepository(SqlContext context)
            : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}