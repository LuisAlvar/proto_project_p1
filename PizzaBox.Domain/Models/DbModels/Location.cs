using System.ComponentModel.DataAnnotations;

namespace PizzaBox.Domain.Models.DbModels{
  public class Location{
    public int Id { get; set; }
    [Required]
    public string Address {get; set;}
    [Required]
    public string PhoneNumber {get; set;}
  }
}