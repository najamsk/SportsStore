using SportsStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;
using System.Web.Mvc;
using Moq;

namespace SportsStore.UnitTests
{
    
    
    /// <summary>
    ///This is a test class for AccountControllerTest and is intended
    ///to contain all AccountControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AccountControllerTest
    {

        [TestMethod()]
        public void Can_LogOn_With_Valid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m=>m.Authenticate("admin", "admin")).Returns(true);
            LogOnViewModel logonModel = new LogOnViewModel { UserName = "admin", Password = "admin" };
            AccountController target = new AccountController(mock.Object);

            ActionResult result = target.LogOn(logonModel, "/myUrl");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/myUrl", ((RedirectResult)result).Url);
        }

        [TestMethod()]
        public void Cant_LogOn_With_InValid_Credentials()
        {
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("gora", "bad")).Returns(false);
            LogOnViewModel logonModel = new LogOnViewModel { UserName = "gora", Password = "bad" };
            AccountController target = new AccountController(mock.Object);

            ActionResult result = target.LogOn(logonModel, "/myUrl");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
