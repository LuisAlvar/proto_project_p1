using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models.Singletons;
using PizzaBox.Domain.Models.DbModels;
using PizzaBox.Data;

namespace PizzaBox.Client.Controllers{
  public class MainController: Controller{
    private Data.PizzaBoxDbContext _db = new Data.PizzaBoxDbContext();
    public IActionResult Index(){ //redirect user to render the starter template _UserLayout.cshtml
      if(CurrentUser.IsEmpty()){
        return RedirectToAction("Index","Home");
      }
      try
      {
        var locationList = _db.Locations.ToList();
        CurrentUser.SetLocationStorage(locationList);
      }
      catch (System.Exception)
      {
        CurrentUser.Storage().FetchDbError = "Unable to read your location.";
      } 
      return View(CurrentUser.Storage());
    }

    [HttpGet]
    public IActionResult Signout(){
      CurrentUser.DeleteStorage();
      return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult SelectedLocation(){
      return View(CurrentUser.Storage());
    }

    [HttpPost]
    public IActionResult SelectedLocation(User a){
      var choosen = a.SelectedLocation;
      CurrentUser.Storage().SelectedLocation = a.SelectedLocation;

      return RedirectToAction("Index", "Order");
    }
  }
}