using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers.Seller
{
    [Authorize]
    public class CategoriesController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: Categories
      
        public ActionResult Index()
        {
            var query = db.Categories.ToList();
            return View(query);
        }

        #region Create
        public ActionResult Create()
        {

            return View();


        }

        [HttpPost]
        public ActionResult Create(Category c)
        {

            var sd = db.Categories.Where(x => x.CategoryName == c.CategoryName).SingleOrDefault();
            if (sd != null)
            {
                TempData["msg"] = "Category Name has already been added! Try another...";
                return RedirectToAction("Create", "Categories");
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    Category cat = new Category();
                    cat.CategoryName = c.CategoryName;
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Categories");
                }
                else
                {
                    TempData["msg"] = "Failed Attempt!";
                    return RedirectToAction("Create", "Categories");
                }

            }
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {


            var query = db.Categories.Where(m => m.CategoryID == id).ToList().FirstOrDefault();
            return View(query);


        }

        [HttpPost]
        public ActionResult Edit(Category c)
        {
            try
            {

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Categories");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index", "Categories");
        }
        #endregion

        public ActionResult Delete(int? id)
        {
            var query = db.Categories.SingleOrDefault(m => m.CategoryID == id);
            db.Categories.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }

        //Category Ends

       
    }
}