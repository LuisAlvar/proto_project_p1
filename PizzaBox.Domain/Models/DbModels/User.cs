using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models.DbModels{
  public class User{
    public int Id { get; set; }
    [Required(ErrorMessage="Need a Username")]
    public string Username { get; set; }
    [Required(ErrorMessage="Need a First Name")]
    public string FirstName { get; set; }
    [Required(ErrorMessage="Need a Last Name")]
    public string LastName { get; set; }
    [Required(ErrorMessage="Need your Address")]
    public string Address { get; set; }
    public string Address2 { get; set; }
    [Required(ErrorMessage="Need your City")]
    public string City { get; set; }
    [Required(ErrorMessage="Need your State")]
    public string State {get; set;}
    [Required(ErrorMessage="Need your ZipCode")]
    public string ZipCode { get; set; }
    [Required(ErrorMessage="Need an Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required(ErrorMessage="Need a Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required(ErrorMessage="Need your Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [NotMapped]
    public string FetchDbError {get; set;}
    [Required(ErrorMessage="Need to Confirm Password")]
    [NotMapped]
    public string ConfirmPassword { get; set; }
  }
}