using System.ComponentModel.DataAnnotations;

namespace PizzaBox.Domain.Models.DbModels{
  public class Topping: AComponent{
    [Required]
    public string Type { get; set; }
  }
}