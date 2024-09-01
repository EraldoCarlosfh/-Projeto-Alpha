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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    RequestClassIdentification = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RequestPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityClassIdentification = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 128, nullable: true),
                    UserIdentification = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    MaskedDocumentNumber = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    EncryptedDocumentNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PhotorUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CellPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsCellPhoneVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsWhatsappVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResendCodesAllowed = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    PasswordUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordErrorsAllowed = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    EmailActivationCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    EmailActivationCodeExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WhatsappActivationCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    WhatsappActivationCodeExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CellPhoneActivationCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CellPhoneActivationCodeExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAccessActivated = table.Column<bool>(type: "bit", nullable: true),
                    IsTermsAccepted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastTermsAcceptanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivationIp = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ActivationBrowser = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ActivationOperationalSystem = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ActivationCodeUsed = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginPhoneCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    IsReceiveCodeActivationByEmail = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsReceiveCodeActivationBySMS = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsOnboardingCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsNewNotification = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
