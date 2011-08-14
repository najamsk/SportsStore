using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.WebUI.Models
{
    public class CategoryListViewModel
    {
        public IEnumerable<string> Categories {get; set;}
        public string CurrentCategory { get; set; }
       
    }
}