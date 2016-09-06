using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ComputerShop.Models;
using WebMatrix.WebData;
using ComputerShop.Filters;
using ComputerShop.DAO;
namespace ComputerShop.Controllers
{
    [InitializeSimpleMembership]
    public class OrderController : Controller
    {
        private DeliveryDAO deliveryDAO = new DeliveryDAO();
        private ComputerShopDBEntities db = new ComputerShopDBEntities();

        [Authorize(Roles = "Seller, Client")]
        public ActionResult Index()
        {
            int userID = WebSecurity.CurrentUserId;
            List<order> orders;
            if (User.IsInRole("Seller"))
            {
                orders = db.order.ToList();
            }
            else
            {
                orders = db.order.Where(order => order.userorder_id == userID).ToList();
            }
            return View(orders);
        }


        [Authorize(Roles = "Client")]
        public ActionResult Create()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null || cart.IsEmpty())
            {
                return View("Empty");
            }
            int cost = 0;
            cart.Content.Keys.ToList().ForEach(key => cost += (int)db.product.Find(key).price * cart.Content[key]);
            ViewBag.cost = cost;
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            string paymethod = formCollection["method"];
            string deliveryType = formCollection["deliveryType"];
            string cost = formCollection["cost"];
            order order = new order();
            order.userorder_id = WebSecurity.CurrentUserId;
            order.order_date = DateTime.Now;
            order.payment_method = paymethod;
            order.tax_sum = Convert.ToInt32(cost);
            order.orders_status = "Оплачен";
            db.order.Add(order);
            db.SaveChanges();
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            ((Cart)Session["Cart"]).Clear();
            if (deliveryType.Equals("доставка на дом"))
            {
                return RedirectToAction("Create","Delivery");
            }
            else
            {
                return View("Thanks");
            }
        }

        [Authorize(Roles = "Seller, Client")]
        public ActionResult Delete(int id = 0)
        {
            order order = db.order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Client") && WebSecurity.CurrentUserId != order.userorder_id)
            {
                return View("Error");
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            bool flag = deliveryDAO.deleteDelivery(id);
            if (flag)
            {
                order order = db.order.Find(id);
                db.order.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Error");
}

       


        [HttpGet]
        [Authorize(Roles = "Seller")]
        public ActionResult Submit(int id = 0)
        {
            order order = db.order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public ActionResult SubmitToDelivery(int id = 0)
        {
            order order = db.order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        [HttpPost, ActionName("SubmitToDelivery")]
        public ActionResult SubmitToDeliveryConfirmed(int id = 0)
        {
            bool flag = deliveryDAO.confirmToDelivery(id);
            if (flag==true)
            return RedirectToAction("Index");
            else 
            return RedirectToAction("Error");
        }

        [HttpPost, ActionName("Submit")]
        public ActionResult SubmitConfirmed(int id = 0)
        {
            order order = db.order.Find(id);
            order.orders_status = "Подтвержден";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}