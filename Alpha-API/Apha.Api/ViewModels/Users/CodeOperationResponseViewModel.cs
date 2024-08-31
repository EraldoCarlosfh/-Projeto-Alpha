namespace Alpha.Api.ViewModels.Users
{
    public class CodeOperationResponseViewModel : IViewModel
    {
        public string MaskedDocumentNumber { get; set; }
        public string ContactValue { get; set; }
        public DateTime CodeExpirationDate { get; set; }
        public int RemaingResendAttempts { get; set; }
        public string UserId { get; set; }
        public bool? IsAccessActivated { get; set; }
    }
}
