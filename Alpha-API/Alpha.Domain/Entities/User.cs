using Alpha.Domain.Enums;
using Alpha.Framework.MediatR.EventSourcing.Entity;

namespace Alpha.Domain.Entities
{
    public partial class User : Entity<User>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string MaskedDocumentNumber { get; private set; }
        public string EncryptedDocumentNumber { get; private set; }
        public string PhotorUrl { get; private set; }
        public DateTime? Birthdate { get; private set; }
        public string CellPhone { get; private set; }
        public bool IsCellPhoneVerified { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool IsWhatsappVerified { get; private set; }
        public DateTime? LastLoginDate { get; private set; }
        public int ResendCodesAllowed { get; private set; }
        public DateTime? PasswordUpdatedDate { get; set; }
        public int PasswordErrorsAllowed { get; set; }
        public string EmailActivationCode { get; private set; }
        public DateTime? EmailActivationCodeExpirationDate { get; private set; }
        public string WhatsappActivationCode { get; private set; }
        public DateTime? WhatsappActivationCodeExpirationDate { get; private set; }
        public string CellPhoneActivationCode { get; private set; }
        public DateTime? CellPhoneActivationCodeExpirationDate { get; private set; }
        public bool? IsAccessActivated { get; private set; }
        public bool IsTermsAccepted { get; private set; }
        public DateTime? LastTermsAcceptanceDate { get; private set; }
        public string ActivationIp { get; private set; }
        public string ActivationBrowser { get; private set; }
        public string ActivationOperationalSystem { get; private set; }
        public string ActivationCodeUsed { get; private set; }
        public DateTime? ActivationDate { get; private set; }
        public string LoginPhoneCode { get; private set; }
        public bool? IsReceiveCodeActivationByEmail { get; private set; }
        public bool? IsReceiveCodeActivationBySMS { get; private set; }
        public bool IsOnboardingCompleted { get; private set; }
        public bool IsNewNotification { get; private set; }

        public void AddCellPhone(string cellPhone)
        {
            CellPhone = cellPhone;
        }

        public void AddFullName(string firstName, string lastName)
        {
            FullName = firstName + " " + lastName;
        }

        public void AddUserName(string userName)
        {
            UserName = userName;
        }

        public void OnboardingCompleted()
        {
            IsOnboardingCompleted = true;
        }

        public void Activate(string signerKey)
        {
            IsActive = true;
        }

        public void TermsAccepted(bool? isTermsAccepted)
        {
            IsTermsAccepted = (bool)isTermsAccepted;
        }

        public void ReceivedCodeActivation(CommunicationChannelsEnum communicationChannels)
        {
            if (communicationChannels == CommunicationChannelsEnum.Email)
                IsReceiveCodeActivationByEmail = true;
            else
                IsReceiveCodeActivationBySMS = true;
        }

        public void Delete()
        {
            IsActive = false;
        }

        public string FormatedName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
