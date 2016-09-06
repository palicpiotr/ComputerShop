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
    public class DeliveryController : Controller
    {
        private ComputerShopDBEntities db = new ComputerShopDBEntities();
        private DeliveryDAO deliveryDAO = new DeliveryDAO();
        //
        // GET: /Delivery/
        [Authorize(Roles = "Client")]
        public ActionResult Index()
        {
          
            var delivery = deliveryDAO.getDelivery(WebSecurity.CurrentUserId);
            ViewData.Model = delivery;
            return View();
           
        }

        [Authorize(Roles = "Client")]
        public ActionResult Details(int id = 0)
        {
            delivery delivery = db.delivery.Find(id);
            return View(delivery);
        }

        //
        // GET: /Delivery/Create

        public ActionResult Create()
        {
            
            return View();
        }

        //
        // POST: /Delivery/Create
        [Authorize(Roles = "Client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(delivery delivery)
        {
                delivery.orderdelivery_id = deliveryDAO.getDeliveryForCreate(WebSecurity.CurrentUserId);
                delivery.cost = 200;
                delivery.delivery_type = "доставка на дом";
                delivery.delivery_status = "в магазине";
                db.delivery.Add(delivery);
                db.SaveChanges();
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}