﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Platform;
using Platform.Controllers;
using Moq;

namespace Platform.Tests.Controllers
{
    [TestClass]
    public class LoginControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            LoginController controller = new LoginController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            LoginController controller = new LoginController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            LoginController controller = new LoginController();

            // Act
            string output = controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            LoginController controller = new LoginController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
