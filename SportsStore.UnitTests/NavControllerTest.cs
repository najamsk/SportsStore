using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SportsStore.Domain.Abstract;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using Moq;
using System.Linq;
using System.Collections.Generic;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for NavControllerTest and is intended
    ///to contain all NavControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NavControllerTest
    {
        public Product[] products;
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

        [TestMethod()]        
        public void NavController_MenuAction_ReturningDistinctCategories_FromProductsCollection()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());           
            NavController target = new NavController(mock.Object); // TODO: Initialize to an appropriate value

            //Action
            CategoryListViewModel result = (CategoryListViewModel)target.Menu(null).Model;
            string[] cats = result.Categories.ToArray();

            //Assert
            Assert.IsTrue(cats.Length == 4);
            Assert.AreEqual(cats[0], "Accounting");
            Assert.AreEqual(cats[1], "Computers");
            Assert.AreEqual(cats[2], "Fruit");
        }
    }
}
