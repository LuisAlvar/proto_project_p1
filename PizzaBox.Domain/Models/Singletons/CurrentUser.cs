using PizzaBox.Domain.Models.DbModels;
using System.Collections.Generic;

namespace PizzaBox.Domain.Models.Singletons{
  public class CurrentUser{
    private static DbModels.User _CurrentUserInfo;
    private static DbModels.Location _selectableLocations;

    public static DbModels.User Storage(){
      if(_CurrentUserInfo != null){
        return _CurrentUserInfo;
      }
      return null;
    }

    public static bool IsEmpty(){
      if(_CurrentUserInfo == null) return true;
      return false;
    }

    public static void SetStorage(DbModels.User authUser){
      _CurrentUserInfo = new PizzaBox.Domain.Models.DbModels.User();
      _CurrentUserInfo = authUser;
      _CurrentUserInfo.Messages = new PizzaBox.Domain.Models.Messages();
    }

    public static void SetLocationStorage(List<DbModels.Location> list){
      if(_CurrentUserInfo.NearestLocations == null){
        _CurrentUserInfo.NearestLocations = list;
      }
    }

    public static List<DbModels.Location> GetLocationStorage(){
      return _CurrentUserInfo.NearestLocations;
    }


    public static void DeleteStorage(){
      _CurrentUserInfo = null;
    }
    private CurrentUser(){}
  }
}