using Alpha.Api.ViewModels.Users;
using Alpha.Domain.Entities;
using Alpha.Domain.Responses.Users;
using Alpha.Integrations.Encryption;
using AutoMapper;
using System;

namespace Alpha.Api.Mappings.Users
{
    public class UserMappingProfile : Profile
    {
        private readonly IEncryptionService _encryptionService;

        public UserMappingProfile(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;

            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(c => c.Id.ToString()))
                .ForMember(dest => dest.SaltPassword, opt => opt.MapFrom(c => Decrypt(c.Password)))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(c => Decrypt(c.EncryptedDocumentNumber)));

            CreateMap<LoginUserResponse, LoginResponseViewModel>()
                .ConstructUsing(CreateLoginResponseViewModel)
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<CodeOperationResponse, CodeOperationResponseViewModel>();
            CreateMap<LoginPhoneGenerateCodeResponse, LoginPhoneGenerateCodeResponseViewModel>();
        }

        private string Decrypt(string encryptedDocument)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedDocument)) return string.Empty;

                return _encryptionService.Decrypt(encryptedDocument);
            }
            catch (Exception ex)
            {
                return encryptedDocument;
            }
        }

        private readonly Func<LoginUserResponse, ResolutionContext, LoginResponseViewModel>
            CreateLoginResponseViewModel = (src, ctx) =>
            {
                var viewModel = new LoginResponseViewModel();
                viewModel.ExpirationDate = src.ExpirationDate;
                viewModel.Token = src.Token;
                viewModel.User = ctx.Mapper.Map<UserViewModel>(src.User);
                viewModel.PasswordErrorsAllowed = src.PasswordErrorsAllowed;
                viewModel.UserId = src.UserId;
                viewModel.IsAccessActivated = src.IsAccessActivated;
                viewModel.IsApproved = src.IsApproved;
                viewModel.UserExists = src.UserExists;
                viewModel.IsApprovedNotificationViewed = src.IsApprovedNotificationViewed;
                viewModel.IsNewNotification = src.IsNewNotification;
                viewModel.IsExpirationDate = src.IsExpirationDate;

                return viewModel;
            };
    }
}
