using SpartaMensProduct.DAL;
using SpartaMensProduct.Middleware;
using SpartaMensProduct.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SpartaMensProduct.Controllers
{
    public class AdminController : Controller
    {

       categoryDAL _categoryDal=new categoryDAL();  

        productDAL _productDal = new productDAL();
        orderDAL _orderDal = new orderDAL();
        userDAL _userDal = new userDAL();


       
        /// <summary>
        /// Model for the counts of products and orders
        /// </summary>

        public class DashboardViewModel
        {
            public int TotalProducts { get; set; }
            public int PendingOrders { get; set; }
            public decimal TotalSales { get; set; }
        }



        /// <summary>
        /// Admin dashboard get controller
        /// </summary>
        /// <returns></returns>

        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult Index()
        {
            try
            {

                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                var model = new DashboardViewModel
                {

                    TotalProducts = _productDal.GetProductCount(),
                    PendingOrders = _orderDal.GetOrderCount(),
                    TotalSales = 100,

                };

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }




        /// <summary>
        /// Admin dashboard listing product controller
        /// </summary>
        /// <returns></returns>

        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult ProductList()
        {
            var products = _productDal.GetAllProducts();
            return View(products);
        }


        //Get All users=================================================
        public ActionResult UserList()
        {
            try
            {
                var users = _userDal.GetAllUsers();
                return View(users);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }



        //delete user=================

        /// <summary>
        /// Admin controller for deleting user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        public ActionResult DeleteUser(int userId)
        {
            try
            {
                _userDal.DeleteUser(userId);
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }




        //===========================================================================================

        // GET: Admin/EditProduct
        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult EditProduct(int id)
        {
            try
            {
                var product = _productDal.GetProductById(id);
                var categories = _categoryDal.GetAllCategories();
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Categories = categories;
                return View(product);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        // POST: Admin/EditProduct
        [HttpPost]
        public ActionResult EditProduct( Product model, HttpPostedFileBase imageFile)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    //  model.PasswordHash = HashPassword(model.PasswordHash);

                    var product = new Product
                    {
                        ProductId = model.ProductId,
                        ProductName = model.ProductName,
                        Description = model.Description,
                        Price = model.Price,
                        Category = model.Category,
                        Brand = model.Brand,
                        StockQuantity = model.StockQuantity,
                        IsActive = model.IsActive,
                    };

                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(imageFile.InputStream))
                        {
                            product.ImageData = binaryReader.ReadBytes(imageFile.ContentLength);
                        }
                    }


                    _productDal.UpdateProduct(product);

                    return RedirectToAction("ProductList");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        // GET: Admin/DeleteProduct
        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _productDal.GetProductById(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int productId)
        {
            try
            {
                _productDal.DeleteProduct(productId);
                return RedirectToAction("ProductList");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        //=====================================================================

        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult OrdersList()
        {
            try
            {

                //var orders = _orderDal.GetAllOrders();
                var orders = _orderDal.GetAllOrdersLatest();

                return View(orders);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }

        }

        public ActionResult EditOrder(int id)
        {
            try
            {
                var order = _orderDal.GetAllOrders().FirstOrDefault(o => o.OrderId == id);
                if (order == null)
                {
                    return HttpNotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }


        public ActionResult UpdateOrderStatus(Order order)
        {
            try
            {
                _orderDal.UpdateOrderStatus(order.OrderId, order.Status);
                return RedirectToAction("OrdersList");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }

        // GET: Admin/Details/5
        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
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

        // GET: Admin/Edit/5

        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // GET: Admin/Delete/5
        [SessionAdminAuthorize(requireAdmin: true)]

        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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




        //Category managemnet=================
        public ActionResult Category()
        {
            var categories = _categoryDal.GetAllCategories();
            return View(categories);
        }


        public ActionResult CategoryCreate()
        {
            return View();
        }


        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryCreate(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryDal.AddCategory(category);
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }


        //get categor delete
        public ActionResult CategoryDelete(int id)
        {

            try
            {
                var category = _categoryDal.GetAllCategories().FirstOrDefault(c => c.CategoryId == id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }


        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryDeleteConfirmed(int CategoryId)
        {
            try
            {
                _categoryDal.DeleteCategory(CategoryId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }


        public ActionResult CategoryEdit()
        {

                return View();

        }

    }
}
