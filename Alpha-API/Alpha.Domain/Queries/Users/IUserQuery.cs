using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Queries;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using System.Threading.Tasks;

namespace Alpha.Domain.Queries.Users
{
    public interface IUserQuery : IQuery<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByCellPhone(string cellPhone);
        Task<User> GetUserByLoginPhoneCode(string cellPhone);
        Task<User> GetUserByDocumentNumber(string documentNumber);
        Task<User> GetUserByEmailOrCellPhone(string value);
        Task<User> GetUserByEmailOrDocumentNumber(string value);
        Task<User> GetUserByEntityId(EntityId value);    
        Task<bool> ExistUserByEmailOrCellPhone(string value);
    }
}
