using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }
        
        [Required(ErrorMessage="Please enter a Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage="Please enter product description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage="Please enter product price")]
        [Range(0.01, double.MaxValue,ErrorMessage="Please enter positive price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage="Please enter product category")]
        public string Category { get; set; }
    }
}
