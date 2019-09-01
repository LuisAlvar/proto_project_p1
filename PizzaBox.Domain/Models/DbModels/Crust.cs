using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models.DbModels{
  public class Crust: AComponent{
    [NotMapped]
    public List<Crust> CrustList { get; set; }
  }
}