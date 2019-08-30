using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Data;
using PizzaBox.Domain.Models.DbModels;

namespace  PizzBox.Client.Controllers
{
    public class AuthController : Controller{
      private PizzaBoxDbContext _db = new PizzaBoxDbContext();
      public IActionResult SignIn(){
        return View();
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
        }

        return RedirectToAction("SignUp");
      }

      public IActionResult CheckSignIn(){

        return RedirectToAction("SignIn");
      }
    }
}