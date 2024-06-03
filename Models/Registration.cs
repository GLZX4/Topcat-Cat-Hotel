using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.Models.Enums;

public class Registration
{
    [Key]
    public int RegistrationId { get; set; }
    public int OwnerId { get; set; }
    public bool ConsentToContactVet { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }

    [NotMapped]
    public RegistrationStatus regStatus
    {
        get => Enum.Parse<RegistrationStatus>(Status);
        set => Status = value.ToString();
    }

    // Navigation properties
    public Owner Owner { get; set; }
    public ICollection<Cat> Cats { get; set; }
}