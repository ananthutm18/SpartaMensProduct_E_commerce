using SpartaMensProduct.DAL;
using SpartaMensProduct.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpartaMensProduct.Controllers
{

    /// <summary>
    /// Cart controller
    /// </summary>
    public class CartController : Controller
    {

        private readonly cartDAL cartDAL = new cartDAL();

        private int? GetUserIdFromSession()
        {
            return Session["UserId"] as int?;
        }


        // GET: Cart
        [SessionAdminAuthorize]
        public ActionResult Index()
        {
            var userId = GetUserIdFromSession();
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            var cartItems = cartDAL.GetCartItems(userId.Value);
            return View(cartItems);
        }

        // GET: Cart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var userId = GetUserIdFromSession();
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            cartDAL.AddToCart(userId.Value, productId, quantity);
            return RedirectToAction("Index", "Cart");
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cart/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }



        // POST: Cart/ClearCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearCart()
        {
            var userId = GetUserIdFromSession();
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

            cartDAL.ClearCart(userId.Value); // Call method to clear cart items
            return RedirectToAction("Index", "Cart");
        }


        // POST: Cart/Delete/5
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
