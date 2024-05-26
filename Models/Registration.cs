namespace Topcat_Cat_Hotel.Models
{
    public class Registration
    {
        public int registrationId { get; set; }
        public int ownerId { get; set; }
        public bool consentToVet { get; set; }
        public DateTime createdAt { get; set; }
    }
}
