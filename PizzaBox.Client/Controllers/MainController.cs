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
        //This fetchs the most recent order that the user has been 
        var listOfUserOrders = _db.Orders.Where(o => o.UserId == CurrentUser.Storage().Id).Last(); 
        if(listOfUserOrders.UserId == CurrentUser.Storage().Id){
          const long TicksPerHour = 36000000000;
          DateTime projectedTime = (DateTime) listOfUserOrders.OrderCreated;
          projectedTime = projectedTime.AddTicks(TicksPerHour*2);
          if(DateTime.Now.Ticks < projectedTime.Ticks){
            CurrentUser.Storage().UserAbleToOrder = false;
            CurrentUser.Storage().Messages.MessageType = "UnableToOrder";
            CurrentUser.Storage().Messages.MessageToUser = "You can't create a new order due to time restrictions";
          }else{
            CurrentUser.Storage().UserAbleToOrder = true;
          }
        }
      }
      catch (System.Exception)
      {
        //catch gets execute if user has not order before -> new user
        CurrentUser.Storage().UserAbleToOrder = true;
      }

      if(CurrentUser.Storage().UserAbleToOrder){
        try
        {
          var locationList = _db.Locations.ToList();
          CurrentUser.SetLocationStorage(locationList);
        }
        catch (System.Exception)
        {
          CurrentUser.Storage().Messages.MessageType = "LocationListDbError";
          CurrentUser.Storage().Messages.FetchDbError = "Unable to read your location.";
        } 
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