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

       
        [TestMethod()]        
        public void Can_LoadProdct_ForEdit()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Product result =  target.Edit(products[1].ProductID).Model as Product;

            //Assert
            Assert.AreEqual(2, result.ProductID);
        }

        [TestMethod]
        public void Cant_Edit_NonExistent_Product()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            AdminController target = new AdminController(mock.Object);

            //Act
            Product result = target.Edit(8).Model as Product;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create a product
            Product product = new Product { Name = "Test" };
            // Act - try to save the product
            ActionResult result = target.Edit(product);
            // Assert - check that the repository was called
            mock.Verify(m => m.SaveProduct(product), Times.Once());
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }


        [TestMethod]
        public void Cant_Save_InValid_Changes()
        {
            // Arrange - create mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);
            // Arrange - create a product
            Product product = new Product { Name = "Test" };
            target.ModelState.AddModelError("error", "error");
            // Act - try to save the product
            ActionResult result = target.Edit(product);

            // Assert - check that the repository was called
            //mock.Verify(m => m.SaveProduct(product), Times.Never());

            //if you dont want to pass product instance in above line use statement below
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            
            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act - delete the product
            target.Delete(products[1].ProductID);
            // Assert - ensure that the repository delete method was
            // called with the correct Product
            mock.Verify(m => m.DeleteProduct(products[1]), Times.Once());
        }

        [TestMethod]
        public void Cant_Delete_InValid_Products()
        {

            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());

            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object);

            // Act - delete the product
            target.Delete(100);
            // Assert - ensure that the repository delete method was
            // called with the correct Product
            mock.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never());
        }
    }
}
