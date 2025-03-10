using Domain.Enums;

namespace Infrastructure.Repostories
{
    public class GRepository<T> : IGRepository<T> where T : class
    {
        #region Vars / Props

        protected readonly AppDBContext _dbContext;

        #endregion

        #region Constructor(s)
        public GRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion




        #region Actions
        public virtual async Task<T> GetByIdAsync(Guid id)
        {

            return await _dbContext.Set<T>().FindAsync(id);
        }


        public virtual IQueryable<T> GetAllNoTracking()
        {
            return _dbContext.Set<T>().AsNoTracking().AsQueryable();
        }


        public virtual async Task AddRangeAsync(ICollection<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }
        public virtual async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            //_dbContext.Entry<T>(entity).State = EntityState.Detached;
            _dbContext.Set<T>().Update(entity);
        }

        //public virtual async Task DeleteAsync(T entity)
        //{
        //    //_dbContext.Set<T>().Remove(entity);
        //    _dbContext.Set<T>().Update(entity);
        //    await _dbContext.SaveChangesAsync();
        //}
        //public virtual async Task DeleteRangeAsync(ICollection<T> entities)
        //{
        //    //foreach (var entity in entities)
        //    //{
        //    //    _dbContext.Entry(entity).State = EntityState.Deleted;
        //    //}
        //    _dbContext.Set<T>().UpdateRange(entities);
        //    await _dbContext.SaveChangesAsync();
        //}
        public virtual IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().AsNoTracking()
                .Where(where).AsQueryable();
        }
        public virtual IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> where, int pageNumber, int pageSize, ref ResponseDTO responseDto)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var data = _dbContext.Set<T>().AsNoTracking()
                .Where(where).AsQueryable();
            var items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
            var count = data.Count();

            responseDto.PageIndex = pageNumber;
            responseDto.PageSize = pageSize;
            responseDto.TotalItems = count;
            responseDto.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            responseDto.StatusEnum = StatusEnum.Success;

            return items;
        }
        public virtual IQueryable<T> GetAllAsNoTracking(int pageNumber, int pageSize, ref ResponseDTO responseDto)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var data = _dbContext.Set<T>().AsNoTracking().AsQueryable();
            var items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
            var count = data.Count();

            responseDto.PageIndex = pageNumber;
            responseDto.PageSize = pageSize;
            responseDto.TotalItems = count;
            responseDto.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            responseDto.StatusEnum = StatusEnum.Success;

            return items;
        }
        public virtual IQueryable<T> GetAllAsTracking(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().Where(where).AsQueryable();
        }
        public virtual IQueryable<T> GetAllAsTracking(Expression<Func<T, bool>> where, int pageNumber, int pageSize, ref ResponseDTO responseDto)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var data = _dbContext.Set<T>()
                .Where(where).AsQueryable();
            var items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
            var count = data.Count();

            responseDto.PageIndex = pageNumber;
            responseDto.PageSize = pageSize;
            responseDto.TotalItems = count;
            responseDto.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            responseDto.StatusEnum = StatusEnum.Success;

            return items;
        }
        public virtual IQueryable<T> GetAllAsTracking(int pageNumber, int pageSize, ref ResponseDTO responseDto)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var data = _dbContext.Set<T>().AsQueryable();
            var items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsQueryable();
            var count = data.Count();

            responseDto.PageIndex = pageNumber;
            responseDto.PageSize = pageSize;
            responseDto.TotalItems = count;
            responseDto.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            responseDto.StatusEnum = StatusEnum.Success;

            return items;
        }
        public virtual IQueryable<T> GetAllAsTracking()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public virtual async Task UpdateRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
