using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers.Seller
{
    public class DistrictController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: District
        public ActionResult Index()
        {
            var query = db.Districts.ToList();
            return View(query);
        }

        #region Create
        public ActionResult Create()
        {

            return View();


        }

        [HttpPost]
        public ActionResult Create(District c)
        {

            var sd = db.Districts.Where(x => x.DistrictName == c.DistrictName).SingleOrDefault();
            if (sd != null)
            {
                TempData["msg"] = "Category Name has already been added! Try another...";
                return RedirectToAction("Create", "District");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    District cat = new District();
                    cat.DistrictName = c.DistrictName;
                    db.Districts.Add(cat);
                    db.SaveChanges();
                    return RedirectToAction("Index", "District");
                }
                else
                {
                    TempData["msg"] = "Failed Attempt!";
                    return RedirectToAction("Create", "District");
                }

            }
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {


            var query = db.Districts.Where(m => m.DistrictID == id).ToList().FirstOrDefault();
            return View(query);


        }

        [HttpPost]
        public ActionResult Edit(District c)
        {
            try
            {

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "District");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index", "District");
        }
        #endregion

        public ActionResult Delete(int? id)
        {
            var query = db.Districts.SingleOrDefault(m => m.DistrictID == id);
            db.Districts.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "District");
        }

        //Category Ends

    }
}