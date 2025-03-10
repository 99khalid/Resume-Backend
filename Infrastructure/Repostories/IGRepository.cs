namespace Infrastructure.Repostories
{
    public interface IGRepository<T> where T : class
    {
        //Task DeleteRangeAsync(ICollection<T> entities);
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        //Task DeleteAsync(T entity);
        IQueryable<T> GetAllNoTracking();
        IQueryable<T> GetAllAsTracking();
        IQueryable<T> GetAllAsTracking(Expression<Func<T, bool>> where);
        IQueryable<T> GetAllAsTracking(Expression<Func<T, bool>> where, int pageIndex, int pageSize, ref ResponseDTO responseDto);
        IQueryable<T> GetAllAsTracking(int pageNumber, int pageSize, ref ResponseDTO responseDto);
        IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> where);
        IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> where, int pageIndex, int pageSize, ref ResponseDTO responseDto);
        IQueryable<T> GetAllAsNoTracking(int pageNumber, int pageSize, ref ResponseDTO responseDto);
    }
}
