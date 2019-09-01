using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Data;
using PizzaBox.Domain.Models.DbModels;
using PizzaBox.Domain.Models.Singletons;

namespace  PizzBox.Client.Controllers
{
    public class AuthController : Controller{
      private PizzaBoxDbContext _db = new PizzaBoxDbContext();
      [HttpGet]
      public IActionResult SignIn(){
        var tempUser = new PizzaBox.Domain.Models.DbModels.User();
        tempUser.Messages =  new PizzaBox.Domain.Models.Messages();
        return View(tempUser);
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public IActionResult SignIn(PizzaBox.Domain.Models.DbModels.User potentUser){
        if( (potentUser.Username != null) && (potentUser.Password != null)){
          try
          {
              var userDb = _db.Users.First(u => u.Username == potentUser.Username && potentUser.Password == u.Password);
              CurrentUser.SetStorage(userDb);
              var CurrentUserStored = CurrentUser.Storage();
          }
          catch (System.Exception)
          {
            var errorUser = new PizzaBox.Domain.Models.DbModels.User();
            errorUser.Messages = new PizzaBox.Domain.Models.Messages();
            errorUser.Messages.MessageType = "LoginError";
            errorUser.Messages.FetchDbError = "Invalid Username or Password";
            return View(errorUser);
          }
          return RedirectToAction("Index", "Main");
        }
        return RedirectToAction("Index", "Home");
      }
      [HttpGet]
      public IActionResult SignUp(){
        return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public IActionResult SignUp(PizzaBox.Domain.Models.DbModels.User newUser){
        if(ModelState.IsValid){
          _db.Users.Add(newUser);
          _db.SaveChanges();
        }
        return RedirectToActionPermanent("Index", "Home");
      }

    }
}