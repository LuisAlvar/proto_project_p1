
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models.DbModels{
  public class Size: AComponent{
    [NotMapped]
    public List<Size> SizeList { get; set; }
  }
}