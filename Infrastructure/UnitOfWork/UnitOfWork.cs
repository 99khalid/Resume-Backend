using Domain.Models;
using Infrastructure.Repostories;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        private IDbContextTransaction _transaction;
        public Guid _loggedInUserId;
        public UnitOfWork(AppDBContext context, IHttpContextAccessor _httpContextAccessor)
        {
            _context = context;
            //_loggedInUserId = LoggedInUserProvider.GetLoggedInUserId(_httpContextAccessor);
            repoistories = new Hashtable();

        }
        Hashtable repoistories;
        public IGRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity<Guid>
        {
            var key = typeof(TEntity).Name;
            if (!repoistories.ContainsKey(key))
            {
                var repository = new GRepository<TEntity>(_context);
                repoistories.Add(key, repository);
            }
            return repoistories[key] as IGRepository<TEntity>;


        }
        public IGRepository<TEntity> LK_Repository<TEntity>() where TEntity : BaseEntity<int>
        {
            var key = typeof(TEntity).Name;
            if (!repoistories.ContainsKey(key))
            {
                var repository = new GRepository<TEntity>(_context);
                repoistories.Add(key, repository);
            }
            return repoistories[key] as IGRepository<TEntity>;


        }
        #region Transaction
        public void BeginTransaction() => _transaction = _context.Database.BeginTransaction();
        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();
        public void Commit() => _transaction.Commit();
        public async Task CommitAsync() => await _transaction.CommitAsync();
        public void Rollback() => _transaction?.Rollback();
        public async Task RollbackAsync() => await _transaction.RollbackAsync();

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        public async Task DisposeAsync()
        {
            await _transaction.DisposeAsync();
            await _context.DisposeAsync();
        }
        #endregion


        #region SaveChanges
        public void SaveChanges()
        {
            try
            {
                var entries = _context.ChangeTracker.Entries()
                       .Where(e => (e.Entity is BaseEntity<Guid>) && (
                         e.State == EntityState.Added
                                  || e.State == EntityState.Modified));

                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Modified)
                    {
                        ((BaseEntity<Guid>)entityEntry.Entity).UpdatedOn = DateTime.Now;
                        ((BaseEntity<Guid>)entityEntry.Entity).UpdatedBy = _loggedInUserId;
                        entityEntry.Property("CreatedOn").IsModified = false;
                        entityEntry.Property("CreatedBy").IsModified = false;


                    }

                    if (entityEntry.State == EntityState.Added)
                    {
                        ((BaseEntity<Guid>)entityEntry.Entity).CreatedOn = DateTime.Now;
                        ((BaseEntity<Guid>)entityEntry.Entity).CreatedBy = _loggedInUserId;
                    }

                }
                if (_context.Database.CurrentTransaction == null)
                    BeginTransaction();
                _context.SaveChanges();
                //Commit();
            }
            catch (Exception)
            {
                Rollback();

            }

        }

        public async Task SaveChangesAsync()
        {
            try
            {

                var entries = _context.ChangeTracker.Entries()
.Where(e => (e.Entity is BaseEntity<Guid>) && (
     e.State == EntityState.Added
     || e.State == EntityState.Modified));

                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Modified)
                    {
                        ((BaseEntity<Guid>)entityEntry.Entity).UpdatedOn = DateTime.Now;
                        ((BaseEntity<Guid>)entityEntry.Entity).UpdatedBy = _loggedInUserId;
                        entityEntry.Property("CreatedOn").IsModified = false;
                        entityEntry.Property("CreatedBy").IsModified = false;


                    }

                    if (entityEntry.State == EntityState.Added)
                    {
                        ((BaseEntity<Guid>)entityEntry.Entity).CreatedOn = DateTime.Now;
                        ((BaseEntity<Guid>)entityEntry.Entity).CreatedBy = _loggedInUserId;
                    }
                }
                if (_context.Database.CurrentTransaction == null)
                    await BeginTransactionAsync();
                await _context.SaveChangesAsync();
                //Commit();
            }
            catch (Exception ex)
            {
                await RollbackAsync();

            }

        }
        #endregion

    }

}