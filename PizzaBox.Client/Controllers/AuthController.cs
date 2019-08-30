using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;

namespace  PizzBox.Client.Controllers
{
    public class AuthController : Controller{
      public IActionResult SignIn(){
        return View();
      }

      public IActionResult SignUp(){
        return View();
      }

      public IActionResult CheckSignIn(){
        
        return RedirectToAction("SignIn");
      }
    }
}