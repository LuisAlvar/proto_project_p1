using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models.Singletons;


namespace PizzaBox.Client.Controllers{
  public class MainController: Controller{
    public IActionResult Index(){ //redirect user to render the starter template _UserLayout.cshtml
      if(CurrentUser.IsEmpty()){
        return RedirectToAction("Index","Home");
      }
      return View(CurrentUser.Storage());
    }

    [HttpGet]
    public IActionResult Signout(){
      CurrentUser.DeleteStorage();
      return RedirectToAction("Index", "Home");
    }

  }
}