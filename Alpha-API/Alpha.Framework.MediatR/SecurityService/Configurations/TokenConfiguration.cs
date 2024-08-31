namespace Alpha.Framework.MediatR.SecurityService.Configurations
{
    public class TokenConfiguration
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
        public int VerificationCodeTopSize { get; set; }
        public int VerificationCodeStep { get; set; }
        public int VerificationCodeExpirationInHours { get; set; }
    }
}
