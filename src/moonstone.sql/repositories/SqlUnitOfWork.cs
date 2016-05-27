using moonstone.core.data;
using moonstone.core.repositories;
using moonstone.sql.context;

namespace moonstone.sql.repositories
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        protected SqlContext Context
        {
            get; set;
        }

        public IUserRepository UserRepository { get; set; }

        public SqlUnitOfWork(SqlContext context, IUserRepository userRepo)
        {
            this.Context = context;
            this.UserRepository = userRepo;
        }

        public void Begin()
        {
            this.Context.BeginTransaction();
        }

        public void Commit()
        {
            this.Context.CommitTransaction();
        }
    }
}