using SpartaMensProduct.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpartaMensProduct.Controllers
{


    public class MyproductsController : ApiController
    {

        productDAL _productDal = new productDAL();


        [HttpGet]
        public IHttpActionResult GetProducts()
        {
            var products = _productDal.GetAllProducts();
            return Ok(products);
        }
    }
}
