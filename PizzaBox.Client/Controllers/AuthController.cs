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
        return View();
      }
      [HttpPost]
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
            return RedirectToAction("SignIn");
          }
          return RedirectToActionPermanent("Index", "Home");
        }
        return RedirectToAction("SignIn");
      }
      [HttpGet]
      public IActionResult SignUp(){
        return View();
      }

      [HttpPost]
      public IActionResult SignUp(PizzaBox.Domain.Models.DbModels.User newUser){
        if(ModelState.IsValid){
          _db.Users.Add(newUser);
          _db.SaveChanges();
          return RedirectToActionPermanent("Index","Home");
        }
        return RedirectToAction("SignUp");
      }

      public IActionResult CheckSignIn(){

        return RedirectToAction("SignIn");
      }
    }
}