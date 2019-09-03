using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models.DbModels{
  public class Order{
    public int Id { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime OrderCreated {get; set;}
    [Required]
    [ForeignKey("Location")]
    public int LocationId {get; set;}
    [NotMapped]
    public Location Location { get; set; }
    [Required]
    [ForeignKey("User")]
    public int UserId { get; set;}
    [NotMapped]
    public User User { get; set; }
    [NotMapped]
    public List<Pizza> Pizzas { get; set; }
    [NotMapped]
    public decimal Cost { get; set;}
  }
}