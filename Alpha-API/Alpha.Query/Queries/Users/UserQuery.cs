using Alpha.Data;
using Alpha.Domain.Entities;
using Alpha.Domain.Queries.Users;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using Alpha.Framework.MediatR.Resources.Extensions;
using Alpha.Framework.MediatR.SecurityService.Models;
using Alpha.Integrations.Encryption;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Query.Queries.Users
{
    namespace OctaTech.Query.Queries.Users
    {
        public class UserQuery : IUserQuery
        {
            protected AlphaDataContext _dataContext;
            private readonly IEncryptionService _encryptionService;
            private readonly AuthenticatedUserModel _autheticatadUser;

            public UserQuery(
                AlphaDataContext dbContext,
                IEncryptionService encryptionService,
                AuthenticatedUserModel autheticatadUser)
            {
                _dataContext = dbContext;
                _autheticatadUser = autheticatadUser;
                _encryptionService = encryptionService;
            }

            #region Base

            public void Dispose()
            {
                _dataContext.Dispose();
            }

            public async Task<User> GetById(EntityId id)
            {
                return await _dataContext.Users
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            public async Task<List<User>> GetAll()
            {
                return await _dataContext.Users
                    .ToListAsync();
            }

            public async Task<List<User>> GetActives()
            {
                return await _dataContext.Users
                    .Where(c => c.IsActive).ToListAsync();
            }

            #endregion

            public async Task<User> GetUserByEntityId(EntityId id)
            {
                return await _dataContext.Users
                    .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
            }

            public async Task<User> GetUserByEmail(string email)
            {
                return await _dataContext.Users
                    .FirstOrDefaultAsync(c => c.Email == email && c.IsActive);
            }

            public async Task<User> GetUserByDocumentNumber(string documentNumber)
            {
                string encryptedDocument = _encryptionService.Encrypt(documentNumber.ClearNumberMask());

                return await _dataContext.Users
                    .FirstOrDefaultAsync(c => c.EncryptedDocumentNumber == encryptedDocument);
            }

            public async Task<User> GetUserByCellPhone(string cellPhone)
            {
                var clearedCellPhone = cellPhone.ClearNumberMask();

                return await _dataContext.Users
                    .FirstOrDefaultAsync(c => c.CellPhone == clearedCellPhone && c.IsActive);
            }

            public async Task<User> GetUserByLoginPhoneCode(string loginPhoneCode)
            {
                return await _dataContext.Users
                    .FirstOrDefaultAsync(c => c.LoginPhoneCode == loginPhoneCode && c.IsActive);
            }

            public async Task<User> GetUserByEmailOrDocumentNumber(string value)
            {
                if (value.Contains("@"))
                    return await GetUserByEmail(value);

                return await GetUserByDocumentNumber(value);
            }

            public async Task<User> GetUserByEmailOrCellPhone(string value)
            {
                if (value.Contains("@"))
                    return await GetUserByEmail(value);

                return await GetUserByCellPhone(value);
            }

            public async Task<bool> ExistUserByEmailOrCellPhone(string value)
            {
                if (value.Contains("@"))
                {
                    return await _dataContext.Users.AnyAsync(x => x.Email == value);
                }
                else
                {
                    var clearedCellPhone = value.ClearNumberMask();
                    return await _dataContext.Users.AnyAsync(x => x.CellPhone == value);
                }
            }
        }
    }
}