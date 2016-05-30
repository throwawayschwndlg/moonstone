using moonstone.core.data;
using moonstone.core.repositories;
using moonstone.sql.context;
using System;

namespace moonstone.sql.context
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }

        protected SqlContext Context
        {
            get; set;
        }

        public SqlUnitOfWork(SqlContext context, IUserRepository userRepo)
        {
            this.Context = context;
            this.UserRepository = userRepo;
        }

        public void Begin()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }
    }
}