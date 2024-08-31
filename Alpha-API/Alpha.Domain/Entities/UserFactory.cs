using Alpha.Domain.Commands.Users;
using Alpha.Framework.MediatR.EventSourcing.Entity;
using Alpha.Framework.MediatR.Resources.Extensions;

namespace Alpha.Domain.Entities
{
    public partial class User
    {
        public static User Create(string firstName,
                                  string lastName,
                                  string maskedDocumentNumber,
                                  string encryptedDocumentNumber,
                                  string photorUrl,
                                  string userName,
                                  string email,
                                  bool isEmailVerified,
                                  string cellPhone,
                                  bool isCellPhoneVerified,
                                  int passwordErrorsAllowed,
                                  DateTime? lastLoginDate,
                                  int resendCodesAllowed,
                                  string emailActivationCode,
                                  DateTime? emailActivationCodeExpirationDate,
                                  string cellPhoneActivationCode,
                                  DateTime? cellPhoneActivationCodeExpirationDate,
                                  bool? isAccessActivated,
                                  bool isTermsAccepted,
                                  DateTime? lastTermsAcceptanceDate,
                                  string activationIp,
                                  string activationBrowser,
                                  string activationOperationalSystem,
                                  string activationCodeUsed,
                                  DateTime? activationDate,
                                  bool isCookiesAccepted,
                                  DateTime? lastCookiesAcceptanceDate,
                                  string id = null)
        {
            return new User
            {
                Id = id == null ? new EntityId(Guid.NewGuid().ToString()) : id.ToEntityId(),
                FirstName = firstName,
                LastName = lastName,
                MaskedDocumentNumber = maskedDocumentNumber,
                EncryptedDocumentNumber = encryptedDocumentNumber,
                Birthdate = null,
                PhotorUrl = photorUrl,
                UserName = userName,
                Email = email.ToLower().Trim(),
                IsEmailVerified = isEmailVerified,
                CellPhone = cellPhone,
                IsCellPhoneVerified = isCellPhoneVerified,
                PasswordErrorsAllowed = passwordErrorsAllowed,
                LastLoginDate = lastLoginDate,
                ResendCodesAllowed = resendCodesAllowed,
                EmailActivationCode = emailActivationCode,
                EmailActivationCodeExpirationDate = emailActivationCodeExpirationDate,
                CellPhoneActivationCode = cellPhoneActivationCode,
                CellPhoneActivationCodeExpirationDate = cellPhoneActivationCodeExpirationDate,
                IsAccessActivated = isAccessActivated,
                IsTermsAccepted = isTermsAccepted,
                LastTermsAcceptanceDate = lastTermsAcceptanceDate,
                ActivationIp = activationIp,
                ActivationBrowser = activationBrowser,
                ActivationOperationalSystem = activationOperationalSystem,
                ActivationCodeUsed = activationCodeUsed,
                ActivationDate = activationDate,
                IsActive = true,
                IsNewNotification = true
            };
        }

        public static User Create(
            CreateNewUserCommand command,
            string userEncryptedPassword,
            string emailActivationCode,
            DateTime? emailActivationCodeExpirationDate)
        {
            return new User
            {
                FirstName = GetFistName(command.FullName),
                LastName = GetLastName(command.FullName),
                Birthdate = null,
                FullName = command.FullName,
                Email = command.Email.ToLower().Trim(),
                Password = userEncryptedPassword,
                EmailActivationCode = emailActivationCode,
                IsNewNotification = true,
                EmailActivationCodeExpirationDate = emailActivationCodeExpirationDate
            };
        }

        public void Activate(ActivateUserCommand request, string userEncryptedPassword, string encryptedDocumentNumber)
        {
            EncryptedDocumentNumber = EncryptedDocumentNumber ?? encryptedDocumentNumber;
            Birthdate = Birthdate ?? request.Birthdate;
            Password = userEncryptedPassword;
            PasswordUpdatedDate = DateTime.UtcNow.ToLocalTimeZone();
            IsAccessActivated = true;
            IsActive = true;
            ActivationIp = request.ActivationIp;
            ActivationOperationalSystem = request.ActivationOperationalSystem;
            ActivationBrowser = request.ActivationBrowser;
            ActivationCodeUsed = request.ActivationCode;
            ActivationDate = DateTime.UtcNow.ToLocalTimeZone();
            IsTermsAccepted = true;
            LastTermsAcceptanceDate = ActivationDate;
        }

        public void SetCellphone(string cellphone)
        {
            CellPhone = cellphone;
        }

        public void ChangePassword(string userEncryptedPassword)
        {
            Password = userEncryptedPassword;
            PasswordUpdatedDate = DateTime.UtcNow.ToLocalTimeZone();
            PasswordErrorsAllowed = 5;
        }
        public void UpdateNewNotification(bool isNewNotification)
        {
            IsNewNotification = isNewNotification;
        }

        public void SetOnboardingCompleted()
        {
            IsOnboardingCompleted = true;
        }


        public void AcceptNewTerms(AcceptNewUserTermsCommand request)
        {
            IsTermsAccepted = true;
            LastTermsAcceptanceDate = DateTime.UtcNow.ToLocalTimeZone();
            ActivationIp = request.ActivationIp;
            ActivationOperationalSystem = request.ActivationOperationalSystem;
            ActivationBrowser = request.ActivationBrowser;
        }

        public void ForgotPasswordByEmail(string emailVerificationCode, DateTime emailVerificationCodeExpirationDate)
        {
            ResendCodesAllowed = 5;
            EmailActivationCode = emailVerificationCode;
            EmailActivationCodeExpirationDate = emailVerificationCodeExpirationDate.ToLocalTimeZone();
        }

        public void ForgotPasswordByCellPhone(string cellPhoneVerificationCode, DateTime cellPhoneVerificationCodeExpirationDate)
        {
            ResendCodesAllowed = 5;
            CellPhoneActivationCode = cellPhoneVerificationCode;
            CellPhoneActivationCodeExpirationDate = cellPhoneVerificationCodeExpirationDate.ToLocalTimeZone();
        }

        public void HandlePasswordError()
        {
            PasswordErrorsAllowed--;
            if (PasswordErrorsAllowed == 0) IsAccessActivated = false;
        }

        public void ResendActivationCodeByEmail(string emailVerificationCode, DateTime emailVerificationCodeExpirationDate, bool isDecreaseOperation = true)
        {
            if (isDecreaseOperation)
                ResendCodesAllowed--;

            EmailActivationCode = emailVerificationCode;
            EmailActivationCodeExpirationDate = emailVerificationCodeExpirationDate.ToLocalTimeZone();
        }

        public void ResendActivationCodeByCellPhone(string cellPhoneVerificationCode, DateTime cellPhoneVerificationCodeExpirationDate, bool isDecreaseOperation = true)
        {
            if (isDecreaseOperation)
                ResendCodesAllowed--;

            CellPhoneActivationCode = cellPhoneVerificationCode;
            CellPhoneActivationCodeExpirationDate = cellPhoneVerificationCodeExpirationDate.ToLocalTimeZone();
        }

        public void ResendActivationCodeByWhatsapp(string whatsappVerificationCode, DateTime whatsappVerificationCodeExpirationDate, bool isDecreaseOperation = true)
        {
            if (isDecreaseOperation)
                ResendCodesAllowed--;

            WhatsappActivationCode = whatsappVerificationCode;
            WhatsappActivationCodeExpirationDate = whatsappVerificationCodeExpirationDate.ToLocalTimeZone();
        }

        public void ResetPasswordByEmail(string userEncryptedPassword)
        {
            Password = userEncryptedPassword;
            PasswordUpdatedDate = DateTime.UtcNow.ToLocalTimeZone();
            PasswordErrorsAllowed = 5;
            ResendCodesAllowed = 5;
            EmailActivationCode = null;
            EmailActivationCodeExpirationDate = null;
            IsAccessActivated = true;
        }

        public void ResetPasswordByCellPhone(string userEncryptedPassword)
        {
            Password = userEncryptedPassword;
            PasswordUpdatedDate = DateTime.UtcNow.ToLocalTimeZone();
            PasswordErrorsAllowed = 5;
            ResendCodesAllowed = 5;
            CellPhoneActivationCode = null;
            CellPhoneActivationCodeExpirationDate = null;
            IsAccessActivated = true;
        }

        public void ResetUserTermsAcceptance()
        {
            IsTermsAccepted = false;
            LastTermsAcceptanceDate = null;
        }

        public void UpdateAccess(bool activationStatus)
        {
            IsAccessActivated = activationStatus;
            PasswordErrorsAllowed = 5;
            ResendCodesAllowed = 5;
        }

        public void UpdateActivated(bool activated)
        {
            IsActive = activated;
        }

        public void UpdateFullname(string fullName)
        {
            FirstName = fullName.GetFirstName();
            LastName = fullName.GetLastName();
        }

        public void Update(string cellphone, DateTime? birthdate, string email)
        {
            CellPhone = cellphone;
            Birthdate = birthdate;
            Email = email;
        }


        public void UpdateDocumentNumber(string documentNumber, string encryptedDocumentNumber)
        {
            MaskedDocumentNumber = documentNumber.HideCPF();
            EncryptedDocumentNumber = encryptedDocumentNumber;
        }

        public void UpdateEmail(string email, string emailVerificationCode, DateTime emailVerificationCodeExpirationDate)
        {
            Email = email.ToLower().Trim();
            IsEmailVerified = false;
            EmailActivationCode = emailVerificationCode;
            EmailActivationCodeExpirationDate = emailVerificationCodeExpirationDate.ToLocalTimeZone();
        }
        public void UpdateCellPhone(string cellPhone, string cellPhoneVerificationCode, DateTime cellPhoneVerificationCodeExpirationDate)
        {
            CellPhone = cellPhone;
            IsCellPhoneVerified = false;
            CellPhoneActivationCode = cellPhoneVerificationCode;
            CellPhoneActivationCodeExpirationDate = cellPhoneVerificationCodeExpirationDate.ToLocalTimeZone();
        }

        public void Login()
        {
            LastLoginDate = DateTime.UtcNow.ToLocalTimeZone();
            PasswordErrorsAllowed = 5;
            ResendCodesAllowed = 5;
        }

        public void VerifyCellphone()
        {
            IsCellPhoneVerified = true;
        }

        public void VerifyEmail()
        {
            IsEmailVerified = true;
        }

        public void VerifyWhatsapp()
        {
            IsWhatsappVerified = true;
        }

        public string GenerateLoginPhoneCode()
        {
            Random generator = new Random();
            var code = generator.Next(0, 1000000).ToString("D6");
            this.LoginPhoneCode = code;
            return code;
        }

        public void RemoveLoginPhoneCode()
        {
            LoginPhoneCode = null;
        }

        private static string GetFistName(string fullName)
        {
            var fullNameSplit = fullName.Split(' ');
            if (fullNameSplit.Length > 1)
                return fullNameSplit[0];

            return string.Empty;
        }

        private static string GetLastName(string fullName)
        {
            var fullNameSplit = fullName.Split(' ').ToList();
            if (fullNameSplit.Count > 1)
            {
                fullNameSplit.RemoveAt(0);
                return string.Join(' ', fullNameSplit);
            }

            return string.Empty;
        }
    }
}
