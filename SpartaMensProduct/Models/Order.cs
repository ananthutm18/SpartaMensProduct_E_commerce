using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpartaMensProduct.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        
    }
}