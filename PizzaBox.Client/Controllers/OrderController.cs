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
      if(CurrentUser.IsEmpty()){
        return RedirectToAction("Index", "Home");
      }
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
      using(var transaction = _db.Database.BeginTransaction()){

        _db.Orders.Add(new Order(){
          OrderCreated = UsersOrder.Storage().OrderCreated,
          LocationId = UsersOrder.Storage().Location.Id,
          UserId = CurrentUser.Storage().Id
        });

        _db.SaveChanges();
        var fetchOrderId = _db.Orders.Single(o => o.OrderCreated == UsersOrder.Storage().OrderCreated).Id;
        foreach (var PizzaObject in UsersOrder.Storage().Pizzas)
        {
          _db.Pizzas.Add(new Pizza(){
            CrustId = PizzaObject.Crust.Id,
            SizeId = PizzaObject.Size.Id,
            Cost = PizzaObject.Cost,
            Description = PizzaObject.Description,
            OrderId = fetchOrderId
          });
        }
        _db.SaveChanges();
        transaction.Commit();
      }
      CurrentUser.Storage().UserAbleToOrder = false;
      UsersOrder.DeleteStorage(); 
      //remove order infomation 
      return RedirectToAction("Receipt", "Order");
    }
    
    public IActionResult History(){
      CurrentUser.Storage().OrderHistory = new List<Order>();
      CurrentUser.Storage().OrderHistory = _db.Orders.Where(o => o.UserId == CurrentUser.Storage().Id).ToList();
      foreach(var orderObject in CurrentUser.Storage().OrderHistory){
        orderObject.Pizzas = new List<Pizza>();
        orderObject.Pizzas = _db.Pizzas.Where(p => p.OrderId == orderObject.Id).ToList();
        decimal orderTotalCost = 0;
        foreach(var pizzaObject in orderObject.Pizzas){
          var sizeDb = _db.Sizes.Single(s => s.Id == pizzaObject.SizeId);
          pizzaObject.Size = new Size();
          pizzaObject.Size.Id = sizeDb.Id;
          pizzaObject.Size.Name = sizeDb.Name;
          pizzaObject.Size.Price = sizeDb.Price;
          var crustDb = _db.Crusts.Single(c => c.Id == pizzaObject.CrustId);
          pizzaObject.Crust = new Crust();
          pizzaObject.Crust.Id = crustDb.Id;
          pizzaObject.Crust.Name = crustDb.Name;
          pizzaObject.Crust.Price = crustDb.Price;

          string[] toppingsArr = pizzaObject.Description.Split(' ');
          pizzaObject.Toppings = new List<Topping>();
          for (var i = 0; i < toppingsArr.Count()-1; ++i)
          {
            pizzaObject.Toppings.Add(_db.Toppings.Single(t => t.Name == toppingsArr[i]));
          }
          orderTotalCost += pizzaObject.Cost;
        }
        orderObject.Cost = orderTotalCost;
      }

      return View(CurrentUser.Storage());
    }

    public IActionResult Cart(){
      if(CurrentUser.Storage().Messages != null){
        CurrentUser.Storage().Messages.MessageType = "";
      }
      //if the user is wants to re-access the cart after a order he will be 
      //sent back to the main index
      if(!CurrentUser.Storage().UserAbleToOrder){
        return RedirectToAction("Index", "Main");
      }
      return View(UsersOrder.Storage());
    }

    public IActionResult Receipt(){
      CurrentUser.Storage().Messages.MessageType = "OrderSuccessfulCommited";
      CurrentUser.Storage().Messages.MessageToUser = "Your order was successfully sent.";
      return View(CurrentUser.Storage());
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