using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        public ActionResult Index()
        {
            this.GetDefaultData();
            ViewBag.LatestProducts = db.Products.OrderByDescending(x => x.ProductID).ToList();
            ViewBag.RecentViewsProducts = RecentViewProducts();
            ViewBag.AllProducts = db.Products.ToList();
          
            return View();
        }



       

        //RECENT VIEWS PRODUCTS
        public IEnumerable<Product> RecentViewProducts()
        {
            if (TempShpData.UserID > 0)
            {
                var top3Products = (from recent in db.RecentlyViews
                                    where recent.UserID == TempShpData.UserID
                                    orderby recent.ViewDate descending
                                    select recent.ProductID).ToList();

                var recentViewProd = db.Products.Where(x => top3Products.Contains(x.ProductID));
                return recentViewProd;
            }
            else
            {
                var prod = (from p in db.Products
                            select p).OrderByDescending(x => x.UnitPrice).ToList();
                return prod;
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}