namespace moonstone.core.data
{
    public interface IUnitOfWork
    {
        void Begin();

        void Commit();
    }
}