using sinavproje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Net;

namespace sinavproje.Controllers
{
    public class LoginController : Controller
    {
        stokTakip021Entities1 db = new stokTakip021Entities1();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult loginInsert(login model)
        {
            login hesap = new login();
            hesap.name = model.name;
            hesap.username = model.username;
            hesap.email = model.email;
            hesap.password = model.password;
            db.logins.Add(hesap);
            db.SaveChanges();

            return RedirectToAction("luserList");
        }
        public ActionResult login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult login(login model1, string returnUrl,dbsepet model2)
        {
            if (ModelState.IsValid)
            {
                var user = db.logins.FirstOrDefault(u => u.username == model1.username && u.password == model1.password);
                if (user != null)
                {
                    Session["UserName"] = user.username;
                    if (user.musteriId.HasValue)
                    {
                        model2.musteriId = user.musteriId.Value;
                    }
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("sepetMusteri", "sepet");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "yanlış girdiniz");
                }
            }

            return View(model1);
        }

        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(login model)
        {
            if (ModelState.IsValid)
            {
                db.logins.Add(model);
                db.SaveChanges();

                return RedirectToAction("login");
            }

            return View(model);
        }

       public ActionResult dellogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult dellogin(int id)
        {
            var user = db.logins.Find(id);
            if (user != null)
            {
                db.logins.Remove(user);
                db.SaveChanges();

                return RedirectToAction("luserList");
            }
            else
            {
                ModelState.AddModelError("", "yanlış girdiniz");
            }

            return View();
        }

        public ActionResult luserList()
        {
            var lusrlist = db.logins.ToList();
            return View(lusrlist);
        }
        [HttpGet]
        public ActionResult loginUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.logins.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult loginUpdate(login model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("luserList");
            }

            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Session.Remove("username");
            Session["cartItems"] = null;

            return RedirectToAction("login");
        }

    }
}
