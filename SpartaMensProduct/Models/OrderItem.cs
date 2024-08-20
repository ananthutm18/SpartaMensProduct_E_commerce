using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpartaMensProduct.Models
{
    public class OrderItem
    {

        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}