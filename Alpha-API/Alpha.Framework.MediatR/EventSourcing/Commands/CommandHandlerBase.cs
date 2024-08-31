using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.SecurityService.Models;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alpha.Framework.MediatR.EventSourcing.Commands
{
    public abstract class CommandHandlerBase<TEntity, TRequest, TResult, TAggregate>
        where TRequest : IRequest<TResult>
        where TEntity : Entity<TEntity>
        where TResult : class
    {
        protected readonly AuthenticatedUserModel _authenticatedUser;
        private readonly IUnitOfWork _unitOfWork;

        public CommandHandlerBase(IUnitOfWork unitOfWork, AuthenticatedUserModel authenticatedUser)
        {
            _authenticatedUser = authenticatedUser;
            _unitOfWork = unitOfWork;
        }       

        protected async Task<int> CommitPendingChanges()
        {
            return await _unitOfWork.ForceCommitAsync();
        }

        private EntityId GetEntityId(TResult result)
        {
            if (result == null) return null;
            var properties = result.GetType().GetProperties();
            var idProperty = properties.FirstOrDefault(c => c.Name == "Id");

            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(result).ToString();

                return new EntityId(idValue);
            }

            return null;
        }

        protected abstract Task<TResult> HandleRequest(TRequest request, TAggregate aggregate, CancellationToken cancellationToken);
    }


    public abstract class CommandHandlerBase<TEntity, TRequest, TResult> : OctaNotifiable
       where TRequest : IRequest<TResult>
       where TEntity : Entity<TEntity>
       where TResult : class
    {
        protected readonly AuthenticatedUserModel _authenticatedUser;
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandlerBase(IUnitOfWork unitOfWork, AuthenticatedUserModel authenticatedUser)
        {
            _authenticatedUser = authenticatedUser;
            _unitOfWork = unitOfWork;
        }        

        protected async Task<int> CommitPendingChanges()
        {
            return await _unitOfWork.ForceCommitAsync();
        }

        private EntityId GetEntityId(TResult result)
        {
            if (result == null) return null;
            var properties = result.GetType().GetProperties();
            var idProperty = properties.FirstOrDefault(c => c.Name == "Id");

            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(result).ToString();

                return new EntityId(idValue);
            }

            return null;
        }

        protected abstract Task<TResult> HandleRequest(TRequest request, CancellationToken cancellationToken);

    }
}
