using SportsStore.WebUI.HtmlHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for PagingHelpersTest and is intended
    ///to contain all PagingHelpersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PagingHelpersTest
    {

        [TestMethod()]
        public void PagingHelpers_Can_Genrate_PagingLinks()
        {
            //Arrange 
            HtmlHelper myhelper = null; // TODO: Initialize to an appropriate value
            PagingInfo pagingInfo = new PagingInfo { CurrentPage = 2, TotalItems = 30, ItemsPerPage = 10 };
            Func<int, string> pageUrlDelegate = (x=>"Page"+x); // TODO: Initialize to an appropriate value
            
            //Act
            MvcHtmlString result = myhelper.PageLinks(pagingInfo, pageUrlDelegate);
            
            //Assert
            string expected = @"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>";
            Assert.AreEqual(result.ToString(), expected);
            
        }
    }
}
