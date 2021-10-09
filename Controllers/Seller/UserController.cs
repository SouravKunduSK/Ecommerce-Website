using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers.Seller
{
    public class UserController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: User
        public ActionResult Index()
        {
            var query = db.Users.Where(x=>x.RoleType == 2).ToList();
            return View(query);
        }
    }
}