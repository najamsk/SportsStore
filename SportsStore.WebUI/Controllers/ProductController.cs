using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        private IProductRepository repo;
        public int PageSize = 3;
        public ProductController(IProductRepository Repo) {
            repo = Repo;
        }
        public ViewResult List(string Category, int Page = 1) {
            ProductsListViewModel pvm = new ProductsListViewModel
            {
                Products = repo.Products
                            .Where(p=> Category == null || p.Category == Category)
                            .OrderBy(p => p.ProductID)
                            .Skip((Page - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo { 
                    CurrentPage = Page, 
                    ItemsPerPage = PageSize, 
                    TotalItems = (Category == null) ? repo.Products.Count() : repo.Products.Where(p=>p.Category == Category).Count()
                },
                CurrentCategory = Category

            };            
            return View(pvm);
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repo.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod != null && prod.ImageData != null && prod.ImageMimeType != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }
}
