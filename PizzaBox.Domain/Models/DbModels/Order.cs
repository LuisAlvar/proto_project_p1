using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaBox.Domain.Models.DbModels{
  public class Order{
    public int Id { get; set; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTime OrderCreated {get; set;}
    [Required]
    public Location Location { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    public List<Pizza> Pizzas { get; set; }
  }
}