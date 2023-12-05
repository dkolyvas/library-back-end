namespace library_app.Repositories
{
    public interface IRepository<T>
    {
        public  Task<List<T>> GetAll();
        public Task<T?> GetById(int id);
        public Task<T?> Insert(T entity);
        public Task<T?> Update(T entity, int id);
        public Task<bool> Delete(int id);

    }
}
