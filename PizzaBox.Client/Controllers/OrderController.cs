using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models.Singletons;
using PizzaBox.Domain.Models.DbModels;
using Microsoft.EntityFrameworkCore;
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
      //we have the sizeid and crustid and a list of topping id
      //which the user selected

      //If the user reach this part that mean that 
      //user can make an order 
      if(UsersOrder.IsEmpty()){  
        UsersOrder.Storage().Location = _db.Locations.Single(l => l.Id == CurrentUser.Storage().SelectedLocation);
        // _order.Location.PhoneNumber = _db.Locations.Single(l => l.Id == CurrentUser.Storage().SelectedLocation).PhoneNumber;
        // _order.Location.Address = _db.Locations.Single(l => l.Id == a.SelectedLocation).Address;
        UsersOrder.Storage().OrderCreated = DateTime.Now;
        UsersOrder.Storage().User = CurrentUser.Storage();

        // _order.Pizzas =  new List<Pizza>();

        //place the first pizza in order
        // AddPizzaToUsersOrder(ref a);
      }
      // else{
        // AddPizzaToUsersOrder(ref a);
      // }
      //Check for max toppings 
      if(!CheckSelections(a)) return RedirectToAction("Index", "Order");
      AddPizzaToUsersOrder(a);
      CurrentUser.Storage().Messages.MessageType = "AddedToOrder";
      CurrentUser.Storage().Messages.MessageToUser = $"Added a {UsersOrder.Storage().Pizzas.Last().Size.Name} and {UsersOrder.Storage().Pizzas.Last().Crust.Name} pizza to your order!";
      return RedirectToAction("Index", "Order");
    }

    [HttpPost]
    public IActionResult Submit(){
      //the order is ready to be send to the database 
      //Phrase 1: select the order 
      var revisedOrder = new Order();
      revisedOrder.Id = 0;
      revisedOrder.Location = UsersOrder.Storage().Location;
      revisedOrder.OrderCreated = UsersOrder.Storage().OrderCreated;
      revisedOrder.User = UsersOrder.Storage().User;

      using(var transaction = _db.Database.BeginTransaction()){
        _db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].Orders ON");
        _db.Orders.Add(UsersOrder.Storage());
        _db.SaveChanges();
        _db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].Orders OFF");
        transaction.Commit();
      }

      var orderlistDB =  _db.Orders.ToList();
      //remove order infomation 
      return RedirectToAction("PrintReceipts", "Order");
    }

    public IActionResult PrintReceipts(){
      return RedirectToAction("Index", "Order");
    }

    public IActionResult Cart(){
      if(CurrentUser.Storage().Messages != null){
        CurrentUser.Storage().Messages.MessageType = "";
      }
      return View(UsersOrder.Storage());
    }

    private bool CheckSelections(User a){
      if(a.SelectedToppings.Count > 5){
        CurrentUser.Storage().Messages.MessageType = "PizzaProcessError";
        CurrentUser.Storage().Messages.MessageToUser = "The topping limit is 5.";
        return false;
      }else if(a.SelectedCrust == 0){
        CurrentUser.Storage().Messages.MessageType = "PizzaProcessError";
        CurrentUser.Storage().Messages.MessageToUser = "You forgot to select a crust.";
        return false;
      }else if(a.SelectedSize == 0){
        CurrentUser.Storage().Messages.MessageType = "PizzaProcessError";
        CurrentUser.Storage().Messages.MessageToUser = "You forgot to select a size.";
        return false;
      }
      return true;
    }

    private void AddPizzaToUsersOrder(User a){
        var newPizzaToPlace =  new Pizza();
        newPizzaToPlace.Crust = _db.Crusts.Single(c => c.Id == a.SelectedCrust);
        newPizzaToPlace.Cost += newPizzaToPlace.Crust.Price;
        
        newPizzaToPlace.Size = _db.Sizes.Single(s => s.Id == a.SelectedSize);
        newPizzaToPlace.Cost += newPizzaToPlace.Size.Price;

        newPizzaToPlace.Toppings = new List<Topping>();
        newPizzaToPlace.Description = "";

        foreach (var selectedToppingId in a.SelectedToppings)
        {
            var fetchTopping = _db.Toppings.Single(t => t.Id == selectedToppingId);
            newPizzaToPlace.Description += fetchTopping.Name + " ";

            newPizzaToPlace.Toppings.Add(fetchTopping);
            newPizzaToPlace.Cost += fetchTopping.Price;


        }
        UsersOrder.Storage().Pizzas.Add(newPizzaToPlace);
    }

  }
}