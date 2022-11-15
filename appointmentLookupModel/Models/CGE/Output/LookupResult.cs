using System.ComponentModel.DataAnnotations;
namespace appointmentLookupModel;

public class LookupResult
{
    [Key]
    public int Id { get; set; } //Primary key
    public DateTime LookupTime {get;set;}
    public bool StillAuthenticated {get;set;}
    public bool FoundAppointment  { get; set; }
    public string? Raw {get;set;}
}