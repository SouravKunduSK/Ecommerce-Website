using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers.Customer
{
    public class CheckOutController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: CheckOut
        public ActionResult Index()
        {
            User usr = db.Users.Find(Session["uid"]);
            var data = this.GetDefaultData();
            var tuple = new Tuple<User, IEnumerable<OrderDetail>>(usr, data);
            ViewBag.PayMethod = new SelectList(db.PaymentTypes, "PaymentTypeID", "PaymentTypeName");
            
            return View(tuple);
        }
        public ActionResult PlaceOrder( FormCollection getCheckoutDetails)
        {
            int shpID = 1;
            if (db.ShippingDetails.Count() > 0)
            {
                shpID = db.ShippingDetails.Max(x => x.ShippingID) + 1;
            }
            int payID = 1;
            if (db.Payments.Count() > 0)
            {
                payID = db.Payments.Max(x => x.PaymentID) + 1;
            }
            int orderID = 1;
            if (db.Ordereds.Count() > 0)
            {
                orderID = db.Ordereds.Max(x => x.OrderedID) + 1;
            }
            var usr = db.Users.Find(Session["uid"]);
            //ViewBag.User = usr;
            //var user = new User();
            ShippingDetail shpDetails = new ShippingDetail();
            shpDetails.ShippingID = shpID;
            shpDetails.FirstName = usr.FirstName;
            shpDetails.LastName = usr.LastName;
            shpDetails.Email = usr.Email;
            shpDetails.Mobile = usr.Mobile;
            shpDetails.Address = usr.Address;
            //shpDetails.Province = getCheckoutDetails["Province"];
            //shpDetails.City = getCheckoutDetails["City"];
            //shpDetails.PostCode = getCheckoutDetails["PostCode"];
            shpDetails.PostalCode = usr.PostalCode;
            shpDetails.DistrictID = usr.DistrictID;
            db.ShippingDetails.Add(shpDetails);
            db.SaveChanges();

            Payment pay = new Payment();
            pay.PaymentID = payID;
            pay.PaymentType = Convert.ToInt32(getCheckoutDetails["PayMethod"]);
            db.Payments.Add(pay);
            db.SaveChanges();

            Ordered o = new Ordered();
            o.OrderedID = orderID;
            o.UserID = TempShpData.UserID;
            o.PaymentID = payID;
            o.ShippingID = shpID;
            o.Discount = Convert.ToInt32(getCheckoutDetails["discount"]);
            o.TotalAmount = ViewBag.TotalAmount;
            o.isCompleted = true;
            o.OrderDate = DateTime.Now;
            db.Ordereds.Add(o);
            db.SaveChanges();

            foreach (var OD in TempShpData.items)
            {
                OD.OrderedID = orderID;
                OD.Ordered = db.Ordereds.Find(orderID);
                OD.Product = db.Products.Find(OD.ProductID);
                db.OrderDetails.Add(OD);
                db.SaveChanges();
            }


            return RedirectToAction("Index", "ThankYou");
        }
    }
}