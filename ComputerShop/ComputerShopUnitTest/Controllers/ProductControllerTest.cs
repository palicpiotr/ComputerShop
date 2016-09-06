using System;
using ComputerShop.Controllers;
using ComputerShop.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Web.Mvc;
namespace ComputerShop.Test.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void Index()
        {
            ProductController productController = new ProductController();
            //ActionResult result = productController.Index();
            //Assert.IsNotNull(result);
        }
    }
}
