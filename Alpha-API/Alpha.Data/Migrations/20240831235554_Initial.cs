using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alpha.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationTypeId = table.Column<int>(type: "integer", nullable: false),
                    RequestClassIdentification = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RequestPayload = table.Column<string>(type: "text", nullable: false),
                    EntityClassIdentification = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", maxLength: 128, nullable: true),
                    UserIdentification = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    BarCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Image = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    UserName = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    MaskedDocumentNumber = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    EncryptedDocumentNumber = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    PhotorUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CellPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsCellPhoneVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsWhatsappVerified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ResendCodesAllowed = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    PasswordUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PasswordErrorsAllowed = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    EmailActivationCode = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    EmailActivationCodeExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    WhatsappActivationCode = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    WhatsappActivationCodeExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CellPhoneActivationCode = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    CellPhoneActivationCodeExpirationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsAccessActivated = table.Column<bool>(type: "boolean", nullable: true),
                    IsTermsAccepted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastTermsAcceptanceDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActivationIp = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    ActivationBrowser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ActivationOperationalSystem = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ActivationCodeUsed = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LoginPhoneCode = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    IsReceiveCodeActivationByEmail = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    IsReceiveCodeActivationBySMS = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    IsOnboardingCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsNewNotification = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__Audits");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
