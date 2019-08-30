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
    public static void SetStorage(DbModels.User authUser){
      if(_CurrentUserInfo == null){
        _CurrentUserInfo = authUser;
      }
    }
    private CurrentUser(){}
  }
}