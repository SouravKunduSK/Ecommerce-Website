using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping.Controllers.Seller
{
    [Authorize]
    public class ProductController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: Product
        public ActionResult Index()
        {
            var query = db.Products.ToList();
            return View(query);
        }
        #region Create
        public ActionResult Create()
        {
            List<Category> CategoryList = db.Categories.ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product prod, HttpPostedFileBase Image)
        {
            List<Category> CategoryList = db.Categories.ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
            if (!ModelState.IsValid)
            {
                if(prod!=null && Image!=null)
                {
                    prod.CreatedDate = DateTime.Now;
                    prod.Picture1 = Image.FileName.ToString();

                    var folder = Server.MapPath("~/Uploads/");
                    Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                    db.Products.Add(prod);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
                else if (prod.ProductName != null && prod.CategoryID != null && Image == null)
                {
                    prod.CreatedDate = DateTime.Now;
                    prod.Picture1 = null;
                    db.Products.Add(prod);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    TempData["msg"] = "Please fill out the fields...";
                    return RedirectToAction("Create", "Product");
                }
               
            }
            
            return View();
        }
        #endregion
        #region Edit
        public ActionResult Edit(int? id)
        {

            var query = db.Products.Where(m => m.ProductID == id).ToList().SingleOrDefault();
            Session["pimage"] = query.Picture1;

            if (query == null)
            {
                return HttpNotFound();
            }
            List<Category> CategoryList = db.Categories.ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
            //GetViewBagData();
            return View(query);

        }

        //Post Edit
        [HttpPost]
        public ActionResult Edit(Product prod, HttpPostedFileBase Image)
        {
            //GetViewBagData();
            var p = new Product();
            List<Category> CategoryList = db.Categories.ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
            try
            {
                if(Image!=null)
                {
                    prod.ModifiedDate = DateTime.Now;
                    prod.Picture1 = Image.FileName.ToString();
                    var folder = Server.MapPath("~/Uploads/");
                    Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    prod.ModifiedDate = DateTime.Now;
                    prod.Picture1 = Session["pimage"].ToString();
                   
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }

               
            }
            catch
            {
                TempData["msg"] = "Product isn't updated!" +
                    "You must update the product Image..";
                return RedirectToAction("Edit", "Product");
            }
        }
        #endregion
        #region Delete
        public ActionResult Delete(int? id)
        {
            var query = db.Products.SingleOrDefault(m => m.ProductID == id);
            db.Products.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }

        #endregion
    }
}