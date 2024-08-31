using Alpha.Framework.MediatR.Auditorship.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task<int> ForceCommitAsync(CancellationToken cancellationToken = default);
        void RollBack();
        Task AddAudit(Audit audit);
        Task<int> CommitAsyncForDbSet<TEntity>() where TEntity : class;
    }
}
