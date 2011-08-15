using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Web.Mvc;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Ninject;
using SportsStore.Domain.Concrete;
using SportsStore.WebUI.Models;
namespace SportsStore.UnitTests
{
    
    /// <summary>
    ///This is a test class for ProductControllerTest and is intended
    ///to contain all ProductControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProductControllerTest
    {
        private Product[] products;
        [TestInitialize]
        public void PrepareTest()
        {
            products = new Product[] { 
                new Product{ProductID =1, Name ="Mangos", Category="Fruit", Description="Summer gift", Price=12M},
                new Product{ProductID =2, Name ="Apples", Category="Fruit", Description="spring gift", Price=20M},
                new Product{ProductID =3, Name ="Nike Joggers", Category="Sports", Description="football fever", Price=13M},
                new Product{ProductID =4, Name ="Calculator", Category="Accounting", Description="japaniiii", Price=52M},
                new Product{ProductID =5, Name ="PC", Category="Computers", Description="I am PC", Price=92M},
                new Product{ProductID =6, Name ="MAC", Category="Computers", Description="I am  Mac", Price=120M}
            };
        }
        
        [TestMethod]
        public void ProductController_Action_List_CanPaginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m=>m.Products).Returns(products.AsQueryable());
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Act
            ProductsListViewModel result = (ProductsListViewModel) controller.List(null,2).Model;

            //Assert
            Product[] filtered = result.Products.ToArray();
            Assert.IsTrue(filtered.Length == 3);
            Assert.AreEqual(products[3].Name, "Calculator");
            Assert.AreEqual(products[4].Name, "PC");
            Assert.AreEqual(products[5].Name, "MAC");
        }

        [TestMethod]
        public void ProductController_Can_Send_Pagination_View_Model()
        {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 6);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void ProductController_Can_Send_Pagination_ViewModel_ProductsFilteredByCategory()
        {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            Product[] result = ((ProductsListViewModel)controller.List("Computers", 1).Model).Products.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "PC" && result[0].Category == "Computers");
            Assert.IsTrue(result[1].Name == "MAC" && result[1].Category == "Computers");
            
        }

        [TestMethod]
        public void ProductController_Can_Send_Pagination_ViewModel_ProductsFilteredByCategory_PagingInfoRefelctFilteration()
        {

            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            ProductsListViewModel result = (ProductsListViewModel)controller.List("Computers", 1).Model;

            // Assert
            Assert.AreEqual(result.Products.Count(), 2);
            Assert.IsTrue(result.PagingInfo.CurrentPage == 1);

            //this result failed because ProductsListViewModel contains prodcuts filtered by category
            //but paginginfo don't have totalItems property updated by the category filteration
            Assert.IsTrue(result.PagingInfo.TotalItems == result.Products.Count());

        }
    }
}
