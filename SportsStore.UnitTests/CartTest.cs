using SportsStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
namespace SportsStore.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for CartTest and is intended
    ///to contain all CartTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CartTest
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
        public void AddItemTest()
        {
            //Arrange
            Cart target = new Cart(); // TODO: Initialize to an appropriate value
            
            //Act
            target.AddItem(products[0], 10);
            target.AddItem(products[2], 5);
            target.AddItem(products[0], 3);
            CartLine[] results = target.Lines.ToArray();

            //Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product.ProductID, products[0].ProductID);
            Assert.AreEqual(results[1].Product.ProductID, products[2].ProductID);
            Assert.AreEqual(results[0].Quantity, 13);
            
        }

        /// <summary>
        ///A test for ComputeTotalValue
        ///</summary>
        [TestMethod()]
        public void ComputeTotalValueTest()
        {
            //Arrange
            Cart target = new Cart(); // TODO: Initialize to an appropriate value

            target.AddItem(products[0], 5);
            target.AddItem(products[1], 2);
            target.AddItem(products[2], 3);

            //Act
            decimal total = target.ComputeTotalValue();

            //Assert
            Assert.AreEqual(total, 139M);
            
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        public void ClearTest()
        {

            Cart target = new Cart(); // TODO: Initialize to an appropriate value

            target.AddItem(products[0], 5);
            target.AddItem(products[1], 2);
            target.AddItem(products[2], 3);

            target.Clear();

            Assert.IsTrue( target.Lines.Count() == 0);
        }

        /// <summary>
        ///A test for RemoveLine
        ///</summary>
        [TestMethod()]
        public void RemoveLineTest()
        {
            Cart target = new Cart(); // TODO: Initialize to an appropriate value

            target.AddItem(products[0], 5);
            target.AddItem(products[1], 2);
            target.AddItem(products[2], 3);
            target.AddItem(products[0], 5);

            target.RemoveLine(products[0]);
            Product[] results = target.Lines.Select(e=>e.Product).ToArray();

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0], products[1]);
            Assert.AreEqual(results[1], products[2]);
            
        }
    }
}
