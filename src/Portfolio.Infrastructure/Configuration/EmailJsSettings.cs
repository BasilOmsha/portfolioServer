namespace Portfolio.Infrastructure.Configuration
{
    public class EmailJsSettings
    {
        public required string ServiceId { get; set; }
        public required string TemplateId { get; set; }
        public required string PublicKey { get; set; }
        public required string PrivateKey { get; set; }
    }
}
