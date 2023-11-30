namespace NetCore6APIDataTransfer.Requests.Authentication
{
    public class TokenRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FingerPrint { get; set; }
    }
}
