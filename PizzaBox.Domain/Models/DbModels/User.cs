using System.ComponentModel.DataAnnotations;

namespace PizzaBox.Domain.Models.DbModels{
  public class User{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Address { get; set; }
    public string Address2 { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string State {get; set;}
    [Required]
    public string ZipCode { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
  }
}