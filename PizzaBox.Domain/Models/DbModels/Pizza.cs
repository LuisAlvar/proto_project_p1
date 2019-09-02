using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models.DbModels{
  public class Pizza{
    public int Id { get; set; }
    [Required]
    public Crust Crust { get; set; }
    [Required]
    public Size Size { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    public decimal Cost {get; set;}
    [Required]
    public string Description {get; set;}
    [NotMapped]
    public List<Topping> Toppings { get; set; }
  }
}