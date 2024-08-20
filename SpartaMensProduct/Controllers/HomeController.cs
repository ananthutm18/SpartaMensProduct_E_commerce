using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpartaMensProduct.DAL;
using System.Net.Http;

using SpartaMensProduct.Middleware;
using SpartaMensProduct.Models;
namespace SpartaMensProduct.Controllers
{
    public class HomeController : Controller
    {
        productDAL _productDal = new productDAL();

        public ActionResult Index()
        {




            IEnumerable<Product> products = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59382/api/");
                var responseTask = client.GetAsync("myproducts");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Product>>();
                    readTask.Wait();
                    products = readTask.Result;
                }
                else
                {
                    products = Enumerable.Empty<Product>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(products);



            //try
            //{
            //    var products = _productDal.GetAllProducts();
            //    return View(products);

            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError("Error occurred in SomeAction", ex);
            //    return View("Error");
            //}

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}