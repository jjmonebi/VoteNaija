using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoteNaija;
using VoteNaija.Controllers;

namespace VoteNaija.Tests.Controllers
{
    [TestClass]
    public class VoteControllerTest
    {
        [TestMethod]
        public void Vote()
        {
            // Arrange
            VoteController controller = new VoteController();

            // Act
            ViewResult result = controller.Home() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Poll()
        {
            // Arrange
            VoteController controller = new VoteController();

            // Act
            ViewResult result = controller.Poll() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Article()
        {
            // Arrange
            VoteController controller = new VoteController();

            // Act
            ViewResult result = controller.Article() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
