using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Product name should be between {2} and {1}")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Product quantity should be between {2} and {1}")]
        public string QuantityPerUnit { get; set; }

        [Required]
        [Range(5, 99999, ErrorMessage = "Product price should be between 5 and 99999")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Product category should be choosen")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Product supplier should be choosen")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Product discontinued status should be choosen")]
        public bool Discontinued { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }
    }
}
