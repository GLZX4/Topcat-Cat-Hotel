using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Topcat_Cat_Hotel.Models.Enums;
using Topcat_Cat_Hotel.Models;

public class Registration
{
    [Key]
    public int RegistrationId { get; set; }
    public int OwnerId { get; set; }
    public bool ConsentToContactVet { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

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
