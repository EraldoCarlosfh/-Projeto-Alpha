using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.EventSourcing.Domains;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alpha.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AlphaDataContext _dbContext;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        private int _numberOfTransactions = 0;
        private int _numberOfCommits = 0;

        public UnitOfWork(
            AlphaDataContext dataContext,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            _dbContext = dataContext;
            _domainEventsDispatcher = domainEventsDispatcher;
        }

        public void BeginTransaction()
        {
            _numberOfTransactions++;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            _numberOfCommits++;

            if (_numberOfTransactions == _numberOfCommits)
            {
                var saveInt = await _dbContext.SaveChangesAsync();
                await _domainEventsDispatcher.DispatchEventsAsync(_dbContext);
                return saveInt;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> ForceCommitAsync(CancellationToken cancellationToken = default)
        {
            var saveInt = await _dbContext.SaveChangesAsync();
            await _domainEventsDispatcher.DispatchEventsAsync(_dbContext);
            return saveInt;
        }

        public void RollBack()
        {
            var changedEntries = _dbContext.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        } 

        public async Task<int> CommitAsyncForDbSet<TEntity>() where TEntity : class
        {
            _numberOfCommits++;

            var original = _dbContext.ChangeTracker.Entries()
                        .Where(x => !typeof(TEntity).IsAssignableFrom(x.Entity.GetType()) && x.State != EntityState.Unchanged)
                        .GroupBy(x => x.State)
                        .ToList();

            foreach (var entry in _dbContext.ChangeTracker.Entries().Where(x => !typeof(TEntity).IsAssignableFrom(x.Entity.GetType())))
            {
                entry.State = EntityState.Unchanged;
            }

            var rows = await _dbContext.SaveChangesAsync();

            foreach (var state in original)
            {
                foreach (var entry in state)
                {
                    entry.State = state.Key;
                }
            }

            return rows;
        }
    }
}

