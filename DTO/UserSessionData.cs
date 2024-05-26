using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.DTO
{
    public class userSessionData
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string Role { get; set; }

        public Role UserRole
        {
            get => Enum.Parse<Role>(Role);
            set => Role = value.ToString();
        }
    }
}
