using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        
        // GET: /Nav/
        private IProductRepository repo;
        public NavController(IProductRepository Repo) {
            repo = Repo;
        }
        public PartialViewResult Menu(string Category)
        {
            CategoryListViewModel cvm = new CategoryListViewModel {
                Categories = repo.Products.Select(p => p.Category).Distinct().OrderBy(x => x).AsEnumerable(),
                CurrentCategory = Category
            };
            return PartialView(cvm);
        }

    }
}
