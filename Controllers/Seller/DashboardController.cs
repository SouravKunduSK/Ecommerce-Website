using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers.Seller
{
    [Authorize]
    public class DashboardController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: Dashboard
        public ActionResult Index()
        {

            ViewBag.latestOrders = db.Ordereds.OrderByDescending(x => x.OrderedID).Take(10).ToList();
            ViewBag.NewOrders = db.Ordereds.Where(a => /*a.Dispatched == false &&*/(a.Shipped == false || a.Shipped == null) && (a.Deliver == false || a.Deliver == null) && (a.CancelOrder == false || a.CancelOrder == null)).Count();
            //ViewBag.DispatchedOrders = db.Ordereds.Where(a => a.Dispatched == true && a.Shipped == false && a.Deliver == false).Count();
            ViewBag.ShippedOrders = db.Ordereds.Where(a => /*a.Dispatched == true &&*/ a.Shipped == true && (a.Deliver == false || a.Deliver == null)).Count();
            ViewBag.DeliveredOrders = db.Ordereds.Where(a => /*a.Dispatched == true &&*/ a.Shipped == true && a.Deliver == true).Count();
            ViewBag.CancleOrders = db.Ordereds.Where(a => /*a.Dispatched == true &&*/ /*a.Shipped == true &&*/ a.CancelOrder == true).Count();
            return View();
        }

        
    }
}