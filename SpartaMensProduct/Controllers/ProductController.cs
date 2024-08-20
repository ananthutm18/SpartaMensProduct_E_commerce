using SpartaMensProduct.DAL;
using SpartaMensProduct.Middleware;
using SpartaMensProduct.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SpartaMensProduct.Controllers
{
    public class ProductController : Controller
    {

        productDAL _productDal = new productDAL();
        categoryDAL _categoryDal = new categoryDAL();

        // GET: Product
        public ActionResult Index()
        {


            try
            {
                var products = _productDal.GetAllProducts();
                return View(products);

            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }

            //=======================This code is the API integration code its working fine=====================================

            //List <Product> products = null;
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://localhost:59382/api/");
            //    var responseTask = client.GetAsync("myproducts");
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<List<Product>>();
            //        readTask.Wait();
            //        products = readTask.Result;
            //    }
            //    else
            //    {
            //       // products = Enumerable.Empty<Product>();
            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            //return View(products);


            //=======================API integration code  working fine=====================================

        }

        public ActionResult ByCategory(string Category)
        {

            try
            {
                var products = _productDal.GetProductsByCategory(Category);
                return View(products);

            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }

        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {


            try
            {
                Product product = _productDal.GetProductById(id);
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

        // GET: Product/Create

        [SessionAdminAuthorize(requireAdmin: true)]  // Requires user to be logged in and be an admin

        public ActionResult Create()
        {
            var categories = _categoryDal.GetAllCategories();

            ViewBag.Categories = categories;
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product model, HttpPostedFileBase imageFile)
        {

            try
            {



                if (ModelState.IsValid)
                {
                    //  model.PasswordHash = HashPassword(model.PasswordHash);

                    var product = new Product
                    {
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


                    _productDal.InsertProduct(product);

                    return RedirectToAction("Create");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }

        }



        // GET: Product/Edit/5
        [SessionAdminAuthorize(requireAdmin: true)]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
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



        // GET: Product/Delete/5
        [SessionAdminAuthorize(requireAdmin: true)]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occurred in SomeAction", ex);
                return View("Error");
            }
        }


        //serch ===========

        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var products = _productDal.SearchProducts(keyword);
            return View(products);
        }
    }
}
