using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.ViewModels
{
    public class CategoryImageViewModel
    {
        public int CategoryId { get; set; }

        public byte[] Picture { get; set; }
    }
}
