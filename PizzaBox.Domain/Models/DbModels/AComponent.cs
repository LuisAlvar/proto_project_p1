using System.ComponentModel.DataAnnotations;

namespace PizzaBox.Domain.Models.DbModels{
  public abstract class AComponent{
    public int Id { get; set; } 
    [Required]
    public string Name { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [Required]
    public bool Active { get; set; }
  }
}