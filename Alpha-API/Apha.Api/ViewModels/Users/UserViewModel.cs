using Alpha.Framework.MediatR.EventSourcing.Entity;

namespace Alpha.Api.ViewModels.Users
{
    public class UserViewModel : EntityViewModel, IViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string MaskedDocumentNumber { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public string PhotorUrl { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsWhatsappVerified { get; set; }
        public string Whatsapp { get; set; }
        public string CellPhone { get; set; }
        public bool IsCellPhoneVerified { get; set; }
        public string SaltPassword { get; set; }
        public int PasswordErrorsAllowed { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int ResendCodesAllowed { get; set; }
        public string WhatsappPhoneActivationCode { get; set; }
        public string CellPhoneActivationCode { get; set; }
        public DateTime? EmailActivationCodeExpirationDate { get; set; }
        public DateTime? WhatsappActivationCodeExpirationDate { get; set; }
        public DateTime? CellPhoneActivationCodeExpirationDate { get; set; }
        public bool? IsAccessActivated { get; set; }
        public bool IsTermsAccepted { get; set; }
        public DateTime? LastCookiesAcceptanceDate { get; set; }
        public DateTime? LastTermsAcceptanceDate { get; set; }
        public bool? IsReceiveCodeActivationByEmail { get; set; }
        public bool? IsReceiveCodeActivationBySMS { get; set; }
        public bool IsOnboardingCompleted { get; set; }
        public bool IsNewNotification { get; set; }
    }
}