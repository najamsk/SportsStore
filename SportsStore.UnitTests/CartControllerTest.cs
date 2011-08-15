using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Web.Mvc;
using Moq;
using System.Linq;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for CartControllerTest and is intended
    ///to contain all CartControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CartControllerTest
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
        public void Can_Add_To_Cart() {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());
            CartController target = new CartController(mock.Object, null); // TODO: Initialize to an appropriate value
            Cart cart = new Cart(); // TODO: Initialize to an appropriate value

            //Act           
            RedirectToRouteResult result = target.AddToCart(cart, 1, null);
            
            //Assert
            Assert.AreSame(cart, cart);
            Assert.AreEqual(1, cart.Lines.ToArray().Length);
            Assert.AreEqual(1, cart.Lines.ToArray()[0].Product.ProductID);
            
        }

        [TestMethod]       
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());
            CartController target = new CartController(mock.Object, null); // TODO: Initialize to an appropriate value
            Cart cart = new Cart(); // TODO: Initialize to an appropriate value

            //Act           
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"]);
            
        }

        [TestMethod]
        public void Can_Delete_Product_From_Cart()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());
            CartController target = new CartController(mock.Object, null); // TODO: Initialize to an appropriate value
            Cart cart = new Cart(); // TODO: Initialize to an appropriate value
            target.AddToCart(cart, 1, "myUrl");
            //Act           
            RedirectToRouteResult result = target.RemoveFromCart(cart, 1, "myUrl");

            //Assert
            Assert.AreSame(cart, cart);
            Assert.AreEqual(0, cart.Lines.ToArray().Length);

        }

        [TestMethod]
        public void Deleting_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(products.AsQueryable());
            CartController target = new CartController(mock.Object, null); // TODO: Initialize to an appropriate value
            Cart cart = new Cart(); // TODO: Initialize to an appropriate value

            //Act           
            RedirectToRouteResult result = target.RemoveFromCart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("myUrl", result.RouteValues["returnUrl"]);

        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            Cart cart = new Cart();
            // Arrange - create the controller
            CartController target = new CartController(null, null);
            // Act - call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
            Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {

            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);
            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());
            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that we are passing an valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
        
    }
}
