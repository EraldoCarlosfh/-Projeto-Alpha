namespace Alpha.Domain.Responses.Users
{
    public class CodeOperationResponse
    {
        public string MaskedDocumentNumber { get; set; }
        public string ContactValue { get; set; }
        public DateTime CodeExpirationDate { get; set; }
        public int RemaingResendAttempts { get; set; }
        public string UserId { get; set; }
        public bool? IsAccessActivated { get; set; }
    }
}
