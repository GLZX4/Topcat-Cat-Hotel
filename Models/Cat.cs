using Topcat_Cat_Hotel.Models.Enums;

public class Cat
{
    public int catId { get; set; }
    public int registrationId { get; set; }
    public string name { get; set; }
    public string? breed { get; set; }
    public string? colour { get; set; }
    public int? age { get; set; }
    public Gender sex { get; set; }
    public bool microchipped { get; set; }
    public string? microchipNumber { get; set; }
    public bool? insured { get; set; }
    public string? insuranceCompany { get; set; }
    public string? insurancePolicyNumber { get; set; }
    public string? generalHealthDetails { get; set; }
    public string? vetDetails { get; set; }
    public DateTime createdAt { get; set; }
}
