using PizzaBox.Domain.Models.DbModels;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models.Singletons{
  public class UsersOrder{
    private static Order _Order;
    public static Order Storage(){
      if(_Order == null){
        _Order = new Order();
        _Order.User =  new User();
        _Order.Location = new Location();
        _Order.Pizzas = new List<Pizza>();
      }
      return _Order;
    }

    public static bool IsEmpty(){
      if(_Order == null) return true;
      return false;
    }

    public static void SetStorage(Order anOrder){
      if(_Order != null){
        _Order = anOrder;
      }
    }

    private UsersOrder(){}
  }
}