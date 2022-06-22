using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Category name should be between {2} and {1}")]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Category description should be between {2} and {1}")]
        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }
}
