using SpartaMensProduct.DAL;
using SpartaMensProduct.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpartaMensProduct.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly cartDAL cartDAL = new cartDAL();
        private readonly orderDAL orderDAL = new orderDAL();


        private int? GetUserIdFromSession()
        {
            return Session["UserId"] as int?;
        }
        // GET: Checkout
        [SessionAdminAuthorize]
        public ActionResult Index()
        {
            try
            {
                var userId = GetUserIdFromSession();
                if (userId == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var cartItems = cartDAL.GetCartItems(userId.Value);
                return View(cartItems);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        // GET: Checkout/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Checkout/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Checkout/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }




        // POST: Checkout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder()
        {
            try
            {
                var userId = GetUserIdFromSession();
                if (userId == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var cartItems = cartDAL.GetCartItems(userId.Value);
                if (!cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);
                int orderId = orderDAL.CreateOrder(userId.Value, totalAmount, "Cash on Delivery");

                foreach (var item in cartItems)
                {
                    orderDAL.AddOrderItem(orderId, item.ProductId, item.Quantity, item.Price);
                }

                // Clear the cart after placing the order
                cartDAL.ClearCart(userId.Value);

                return RedirectToAction("OrderConfirmation", new { id = orderId });
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }



        [SessionAdminAuthorize]
        public ActionResult OrderConfirmation(int id)
        {
            try
            {
                ViewBag.OrderId = id;
                return View();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }





        // GET: Checkout/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Checkout/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        // GET: Checkout/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Checkout/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
