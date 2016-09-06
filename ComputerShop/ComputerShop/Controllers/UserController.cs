using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Security;
using ComputerShop.Models;
using WebMatrix.WebData;
using ComputerShop.Filters;

namespace ComputerShop.Controllers
{
   [Authorize(Roles = "Seller")]
    [InitializeSimpleMembership]
    public class UserController : Controller
    {
        private UsersContext db = new UsersContext();

        public ActionResult Index()
        {
            return View(db.UserProfiles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegisterModel model, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                Roles.AddUserToRole(model.UserName, form["role"]);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var user = db.UserProfiles.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            var user = db.UserProfiles.Find(id);
            if (user != null)
            {
                Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                Membership.DeleteUser(user.UserName, true);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangeRole(int id = 0)
        {
            var user = db.UserProfiles.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult ChangeRole(FormCollection form, int id = 0)
        {
            var user = db.UserProfiles.Find(id);
            if (user != null)
            {
                Roles.RemoveUserFromRole(user.UserName, Roles.GetRolesForUser(user.UserName)[0]);
                string role = form["role"];
                Roles.AddUserToRole(user.UserName, role);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}