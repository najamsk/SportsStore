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

namespace SportsStore.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for AdminControllerTest and is intended
    ///to contain all AdminControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdminControllerTest
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

        
        [TestMethod()]       
        public void AdminController_IndexAction_Retuns_Products()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());
            AdminController target = new AdminController(mock.Object);

            //Act
            Product[] result = ((IEnumerable<Product>)target.Index().Model).ToArray();  
            
            //Assert
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual(1, result[0].ProductID);
        }
    }
}
