using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Framework.MediatR.Data.UnitOfWork;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using Alpha.Framework.MediatR.EventSourcing.Responses;
using Alpha.Framework.MediatR.EventSourcing.Validators;
using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Models;
using MediatR;

namespace Alpha.Domain.Commands
{
    public abstract class CommandHandlerBase<TEntity, TRequest, TResult> : Alpha.Framework.MediatR.EventSourcing.Commands.CommandHandlerBase<TEntity, TRequest, TResult>
         where TRequest : IRequest<TResult>
         where TEntity : Entity<TEntity>
         where TResult : class
    {  
        protected readonly IUserQuery _userQuery;
        private User _loggedUser;

        protected EntityId LoggerUserId { get { return (_authenticatedUser != null ? _authenticatedUser.Id.ToEntityId() : null); } }

        protected User LoggedUser
        {
            get
            {
                if (_loggedUser == null)
                    // Informações do usuário ainda não foram recuperadas.
                    _loggedUser = GetLoggedUserInformation();

                return _loggedUser;
            }
        }

        public CommandHandlerBase(
            IUnitOfWork unitOfWork,
            IUserQuery userQuery,
            AuthenticatedUserModel authenticatedUser) : base(unitOfWork, authenticatedUser)
        {
            _userQuery = userQuery;
        }

        private User GetLoggedUserInformation()
        {
            User loggedUser = null;

            // Se existir informações do usuário logado, buscar dados do usuário na base.
            if (_authenticatedUser != null)
            {
                // Recuperar informações do usuário logado (AuthenticatedUser).
                var userId = _authenticatedUser.Id!.ToEntityId();
                if (userId != null)
                {
                    loggedUser = _userQuery.GetById(userId).Result;
                    if (loggedUser == null)
                        throw new BusinessException($"Não foi possível recuperar as informações do usuário logado.");
                }
            }
            return loggedUser;
        }

        public CommandResult<TEntity> ValidationErrors(IReadOnlyCollection<Notification> notifications)
        {
            return new CommandResult<TEntity>(false, "Errors", notifications);
        }

        public CommandResult<List<TEntity>> ValidationListErrors(IReadOnlyCollection<Notification> notifications)
        {
            return new CommandResult<List<TEntity>>(false, "Errors", notifications);
        }

        public CommandResult<TEntity> Success(TEntity data)
        {
            return new CommandResult<TEntity>(true, "Success", data);
        }

        public CommandResult<List<TEntity>> Success(List<TEntity> data)
        {
            return new CommandResult<List<TEntity>>(true, "Success", data);
        }

        public CommandResult<EmptyResponse> ValidationErrorsForEmptyResponse(IReadOnlyCollection<Notification> notifications)
        {
            return new CommandResult<EmptyResponse>(false, "Errors", notifications);
        }

        public CommandResult<EmptyResponse> Success()
        {
            return new CommandResult<EmptyResponse>(true, "Success", new EmptyResponse());
        }

        public CommandResult<TEntity> NotFound()
        {
            return new CommandResult<TEntity>(false, "Errors", new List<Notification> { new Notification("Erro", "Aconteceu um erro inesperado.") });
        }

        public CommandResult<TEntity> Erro(string errorMessage)
        {
            return new CommandResult<TEntity>(false, "Errors", new List<Notification> { new Notification("Erro", errorMessage) });
        }

    }
}
