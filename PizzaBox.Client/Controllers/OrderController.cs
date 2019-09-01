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
  public class OrderController: Controller{
    private PizzaBoxDbContext _db = new PizzaBoxDbContext();

    public IActionResult Index(){
      if(CurrentUser.IsEmpty()){
        return RedirectToAction("Index", "Home");
      }
      //load crust, sizes, toppings 
      ViewBag.CrustList = _db.Crusts.ToList();
      ViewBag.SizeList = _db.Sizes.ToList();
      ViewBag.ToppingList = _db.Toppings.ToList();

      return View(CurrentUser.Storage());
    }

    [HttpGet]
    public IActionResult AddPizzaToOrder(){
      return View(CurrentUser.Storage());
    }

    [HttpPost]
    public IActionResult AddPizzaToOrder(User a){

      return RedirectToAction("Index", "Order");
    }
  }
}