using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpartaMensProduct.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string ProductName { get; set; }


        [StringLength(100, ErrorMessage = "Product name cannot be longer than 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1.00, 10000.00, ErrorMessage = "Price must be between 1.00 and 10,000.00")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }


        [Required(ErrorMessage = "Category is required")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be a non-negative number")]
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }

        public byte[] ImageData { get; set; }  // Base64 string for image
    }
}