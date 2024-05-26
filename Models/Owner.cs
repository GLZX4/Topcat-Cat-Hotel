namespace Topcat_Cat_Hotel.Models
{
    public class Owner
    {
        public int ownerId { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string mobile { get; set; }
        public DateTime createdAt { get; set; }
    }
}
