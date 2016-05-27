using moonstone.sql.context;

namespace moonstone.sql.repositories
{
    public abstract class SqlBaseRepository
    {
        protected SqlContext Context { get; set; }

        protected SqlBaseRepository(SqlContext context)
        {
            this.Context = context;
        }
    }
}