using Alpha.Domain.Entities;
using Alpha.Framework.MediatR.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alpha.Data.Configurations.Users
{
    public class UserConfiguration : EntityConfigurationBase<User>
    {
        public override void ConfigureEntityFields(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(c => c.FirstName).HasMaxLength(128).IsRequired();
            builder.Property(c => c.LastName).HasMaxLength(128).IsRequired();
            builder.Property(c => c.FullName).HasMaxLength(256).IsRequired(false);
            builder.Property(c => c.MaskedDocumentNumber).IsRequired(false).HasMaxLength(14);
            builder.Property(c => c.EncryptedDocumentNumber).IsRequired(false).HasMaxLength(128);
            builder.Property(c => c.Birthdate).IsRequired(false);
            builder.Property(c => c.PhotorUrl).IsRequired(false).HasMaxLength(1024);
            builder.Property(c => c.UserName).IsRequired(false).HasMaxLength(16);
            builder.Property(c => c.Email).IsRequired(false).HasMaxLength(64);
            builder.Property(c => c.IsEmailVerified).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.CellPhone).IsRequired(false).HasMaxLength(20);
            builder.Property(c => c.IsCellPhoneVerified).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.IsWhatsappVerified).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.Password).IsRequired(false).HasMaxLength(1024);
            builder.Property(c => c.PasswordUpdatedDate).IsRequired(false);
            builder.Property(c => c.PasswordErrorsAllowed).IsRequired().HasDefaultValue(3);
            builder.Property(c => c.LastLoginDate).IsRequired(false);
            builder.Property(c => c.ResendCodesAllowed).IsRequired().HasDefaultValue(3);
            builder.Property(c => c.EmailActivationCode).IsRequired(false).HasMaxLength(24);
            builder.Property(c => c.EmailActivationCodeExpirationDate).IsRequired(false);
            builder.Property(c => c.WhatsappActivationCode).IsRequired(false).HasMaxLength(24);
            builder.Property(c => c.WhatsappActivationCodeExpirationDate).IsRequired(false);
            builder.Property(c => c.CellPhoneActivationCode).IsRequired(false).HasMaxLength(24);
            builder.Property(c => c.CellPhoneActivationCodeExpirationDate).IsRequired(false);
            builder.Property(c => c.IsAccessActivated).IsRequired(false).HasDefaultValue(null);
            builder.Property(c => c.ActivationIp).IsRequired(false).HasMaxLength(24);
            builder.Property(c => c.ActivationBrowser).IsRequired(false).HasMaxLength(128);
            builder.Property(c => c.ActivationOperationalSystem).IsRequired(false).HasMaxLength(128);
            builder.Property(c => c.ActivationCodeUsed).IsRequired(false).HasMaxLength(24);
            builder.Property(c => c.ActivationDate).IsRequired(false);
            builder.Property(c => c.IsTermsAccepted).IsRequired(true).HasDefaultValue(false);
            builder.Property(c => c.LastTermsAcceptanceDate).IsRequired(false);
            builder.Property(c => c.LoginPhoneCode).IsRequired(false).HasMaxLength(6);
            builder.Property(c => c.IsReceiveCodeActivationByEmail).IsRequired(false).HasDefaultValue(false);
            builder.Property(c => c.IsReceiveCodeActivationBySMS).IsRequired(false).HasDefaultValue(false);
            builder.Property(c => c.IsOnboardingCompleted).IsRequired();
            builder.Property(c => c.IsNewNotification).IsRequired().HasDefaultValue(false);
        }
    }
}