using PizzaBox.Domain.Models.DbModels;

namespace PizzaBox.Domain.Models.Singletons{
  public class CurrentUser{
    private static DbModels.User _CurrentUserInfo;

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
    }
    public static void DeleteStorage(){
      _CurrentUserInfo = null;
    }
    private CurrentUser(){}
  }
}