using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Shopping.Controllers.Customer
{
    public class AccountController : Controller
    {
        ShoppingEntities db = new ShoppingEntities();
        // GET: Account
        #region User Register
        public ActionResult Register()
        {
            List<Role> RoleList = db.Roles.ToList();
            ViewBag.RoleList = new SelectList(RoleList, "RoleID", "RoleName");
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            List<Role> RoleList = db.Roles.ToList();
            ViewBag.RoleList = new SelectList(RoleList, "RoleID", "RoleName");
            var isEmailExist = db.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if(isEmailExist == null)
            {
                if(user.RoleID == 1)
                {
                    var roleExist = db.Users.Where(x => x.RoleType == 1).Count();
                    if (roleExist > 0)
                    {
                        ViewBag.Message = "Seller Register is Not Possible!!";
                    }
                    else
                    {
                        User u = new User();
                        u.Pass = user.Password;
                        u.FirstName = user.FirstName;
                        u.LastName = user.LastName;
                        u.Email = user.Email;
                        u.Password = Crypto.Hash(user.Password);
                        u.RoleType = user.RoleID;
                        u.Created = DateTime.Now;
                        db.Users.Add(u);
                        db.SaveChanges();
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    User u = new User();
                    u.Pass = user.Password;
                    u.FirstName = user.FirstName;
                    u.LastName = user.LastName;
                    u.Email = user.Email;
                    u.Password = Crypto.Hash(user.Password);
                    u.RoleType = user.RoleID;
                    u.Created = DateTime.Now;
                    db.Users.Add(u);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }


            }
            else
            {
                ViewBag.Message = "Email has already been Register!!";
            }
            return View();
        }
        #endregion

        #region User Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {
            var cust = db.Users.SingleOrDefault(x => x.Email == user.Email);
            if(cust!=null && cust.RoleType == 2)
            {
                if(string.Compare(Crypto.Hash(user.Password), cust.Password) == 0)
                {
                    Session["uid"] = cust.UserID;
                    Session["username"] = cust.FirstName + " " + cust.LastName;
                    FormsAuthentication.SetAuthCookie(cust.Email, false);
                    TempShpData.UserID = cust.UserID;
                   
                    cust.RoleType = 2;
                    cust.LastLogin = DateTime.Now;
                    db.Entry(cust).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Email or Password Error!";
                }
            }
            else if(cust != null && cust.RoleType == 1)
            {
                if (string.Compare(Crypto.Hash(user.Password), cust.Password) == 0)
                {
                    Session["sid"] = cust.UserID;
                    Session["sellername"] = cust.FirstName + " " + cust.LastName;
                    FormsAuthentication.SetAuthCookie(cust.Email, false);
                   
                    cust.RoleType = 1;
                    cust.LastLogin = DateTime.Now;
                    db.Entry(cust).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.Message = "Email or Password Error!";
                }

            }
            else
            {
                ViewBag.Message = "Email or Password Error!";
            }
            return View();
        }
        #endregion
        #region Logout
        public ActionResult Signout()
        {
            TempShpData.UserID = 0;
            TempShpData.items = null;
           
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region MyAccount
        [Authorize]
        public ActionResult Index()
        {
            this.GetDefaultData();
            return View();
        }
        public ActionResult MyProfile()
        {
            this.GetDefaultData();
            var usr = db.Users.Find(Session["uid"]);
            return View(usr);
        }
        public ActionResult Edit(int? id)
        {
             this.GetDefaultData();

                var usr = db.Users.Find(id);
                Session["image"] = usr.Picture;
                return View(usr);
           
        }
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase Image, User user)
        {

            try
            {
                if (Image != null)
                {
                    user.Modified = DateTime.Now;
                    user.Picture = Image.FileName.ToString();
                    var folder = Server.MapPath("~/Uploads/");
                    Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyProfile", "Account");
                }
                else
                {
                    if(Image == null && Session["image"] == null)
                    {
                        user.Picture = null;
                    }
                    else
                    {
                        user.Picture = Session["image"].ToString();
                    }
                    user.Modified = DateTime.Now;
                    
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyProfile", "Account");
                }


            }
            catch
            {
                TempData["msg"] = "Profile isn't updated!";
                return RedirectToAction("Edit", "Account");
            }


        }



        public ActionResult ChangePass()
        {
            this.GetDefaultData();

            
            return View();

        }
        [HttpPost]
        public ActionResult ChangePass(UserViewModel user)
        {
            
            

            try
            {

                var cust = db.Users.Find(Session["uid"]);
                if (string.Compare(Crypto.Hash(user.OldPassword), cust.Password) == 0)
                {
                    cust.UserID = cust.UserID;
                    cust.FirstName = cust.FirstName;
                    cust.LastName = cust.LastName;
                    cust.Email = cust.Email;
                    cust.Mobile = cust.Mobile;
                    cust.Picture = cust.Picture;
                    cust.RoleType = cust.RoleType;
                    cust.DistrictID = cust.DistrictID;
                    cust.PostalCode = cust.PostalCode;
                    cust.Address = cust.Address;
                    cust.Created = cust.Created;
                    cust.LastLogin = cust.LastLogin;
                    cust.Modified = DateTime.Now;
                    cust.Pass = user.Password;
                    cust.Password = Crypto.Hash(user.Password);
                    db.Entry(cust).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("MyProfile", "Account");
                }
                else
                {
                    TempData["msg"] = "Current Password does't match!";
                    return RedirectToAction("ChangePass", "Account");
                }
              
                

            }
            catch
            {
                TempData["msg"] = "Password isn't updated!";
                return RedirectToAction("ChangePass", "Account");
            }

            
        }

        public ActionResult Address()
        {
            this.GetDefaultData();
            var usr = db.Users.Find(Session["uid"]);
            return View(usr);
        }

        public ActionResult EditAddress(int?id)
        {
            this.GetDefaultData();
            var usr = db.Users.Find(id);
            List<District> DistrictList = db.Districts.ToList();
            ViewBag.DistrictList = new SelectList(DistrictList, "DistrictID", "DistrictName");
            return View(usr);
        }

        [HttpPost]
        public ActionResult EditAddress(User user)
        {
            List<District> DistrictList = db.Districts.ToList();
            ViewBag.DistrictList = new SelectList(DistrictList, "DistrictID", "DistrictName");
            var cust = db.Users.Find(Session["uid"]);
            try
            {
                cust.Address = user.Address;
                cust.DistrictID = user.DistrictID;
                cust.PostalCode = user.PostalCode;
                db.Entry(cust).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Address", "Account");
            }
            catch
            {
                TempData["msg"] = "Address isn't updated!";
                return RedirectToAction("EditAddress", "Account");
            }
        }

        #endregion
        #region SellerAccount
        [Authorize]
        public ActionResult SaIndex()
        {
            var usr = db.Users.Find(Session["sid"]);
            return View(usr);
        }
        [Authorize]
        public ActionResult SaEdit(int? id)
        {
            //this.GetDefaultData();

            var usr = db.Users.Find(id);
            Session["image"] = usr.Picture;
            return View(usr);
        }
        [HttpPost]
        public ActionResult SaEdit(HttpPostedFileBase Image, User user)
        {
            try
            {
                if (Image != null)
                {
                    user.Modified = DateTime.Now;
                    user.Picture = Image.FileName.ToString();
                    var folder = Server.MapPath("~/Uploads/");
                    Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("SaIndex", "Account");
                }
                else
                {

                    user.Modified = DateTime.Now;
                    user.Picture = Session["image"].ToString();
                    //var folder = Server.MapPath("~/Uploads/");
                    //Image.SaveAs(Path.Combine(folder, Session["image"].ToString()));
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("SaIndex", "Account");
                }


            }
            catch
            {
                TempData["msg"] = "Product isn't updated!" +
                    "You must update the product Image..";
                return RedirectToAction("SaEdit", "Account");
            }


        }

        #endregion
    }
}