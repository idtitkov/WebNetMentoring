using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.ViewModels
{
    public class ProductViewModel
    {
        public int? ProductId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name should be between {2} and {1}")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Quantity should be between {2} and {1}")]
        public string QuantityPerUnit { get; set; }

        [Required]
        [Range(5, 99999, ErrorMessage = "Price should be between 5 and 99999")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Choose category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Choose supplier")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Choose discontinued status")]
        public bool Discontinued { get; set; }
    }
}
