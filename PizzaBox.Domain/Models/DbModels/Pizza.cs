using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models.DbModels{
  public class Pizza{
    public int Id { get; set; }
   
    [Required]
    [ForeignKey("Crust")]
    public int CrustId {get; set;}

    [NotMapped]
    public Crust Crust { get; set; }

    [Required]
    [ForeignKey("Size")]
    public int SizeId { get; set; }

    [NotMapped]
    public Size Size { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Cost {get; set;}
    [Required]
    public string Description {get; set;}
    [NotMapped]
    public List<Topping> Toppings { get; set; }

    [Required]
    [ForeignKey("Order")]
    public int OrderId { get; set; }
  }
}