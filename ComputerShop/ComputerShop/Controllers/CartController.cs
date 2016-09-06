using ComputerShop.Filters;
using ComputerShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComputerShop.Controllers
{
  //  [Authorize(Roles = "Client")]
    [InitializeSimpleMembership]
    public class CartController : Controller
    {

        private ComputerShopDBEntities db = new ComputerShopDBEntities();

        public ActionResult Index()
        {
            Cart cart = getCart();
            List<product> products = new List<product>();
            cart.Content.Keys.ToList().ForEach(key => products.Add(db.product.Find(key)));
            ViewBag.products = products;
            return View(cart);
        }

        public ActionResult AddToCart(int id = 0)
        {
            product product = db.product.Find(id);
            if (product != null)
            {
                getCart().AddProduct(id);
            }
            else
            {
                return HttpNotFound();
            }
            return PartialView(product);
        }

        public ActionResult Remove(int id = 0)
        {
            product product = db.product.Find(id);
            if (product != null)
            {
                getCart().RemoveProduct(id);
            }
            else
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Clear()
        {
            return View();
        }

        [HttpPost, ActionName("Clear")]
        public ActionResult ClearConfirmed()
        {
            ((Cart)Session["Cart"]).Clear();
            return RedirectToAction("Index");
        }

        private Cart getCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}
