namespace Topcat_Cat_Hotel.Models
{
    public class RegistrationCode
    {
        public required int codeId { get; set; }
        public required string registrationCode { get; set; }
        public bool isUsed { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime expiresAt { get; set; }
    }
}
