using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Net;
using sinavproje.Models;


namespace sinavproje.Controllers
{
    public class AdminUserController : Controller
    {
        stokTakip021Entities1 db = new stokTakip021Entities1();
        public ActionResult Index()
        {
            if (Session["LoggedIn"] == null)
            {
                return RedirectToAction("adminLogin");
            }
            else
            {
                string loggedInUser = Session["LoggedIn"].ToString();
                var user = db.dbusers.FirstOrDefault(u => u.userName == loggedInUser);

                if (user != null)
                {
                    string username = user.userName;
                    ViewBag.Username = username;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult adminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult adminLogin(dbuser model)
        {
            if (ModelState.IsValid)
            {
                var user = db.dbusers.FirstOrDefault(u => u.userName == model.userName && u.password == model.password);
                if (user != null)
                {
                    Session["LoggedIn"] = model.userName;
                    var userName = model.userName;
                    return RedirectToAction("AuserList");
                }
                else
                {
                    ModelState.AddModelError("", "yanlış girdiniz");
                }
            }

            return View(model);
        }

        public ActionResult userInsert(dbuser model)
        {
           
            {
                dbuser user = new dbuser()
                {
                    userName = model.userName,
                    userSurname = model.userSurname,
                    sehirId = model.sehirId,
                    ilceId = model.ilceId,
                    departmandId = model.departmandId,
                    maas = model.maas,
                    izin = model.izin,
                    password = model.password,
                    role = model.role
                };

                db.dbusers.Add(user);
                db.SaveChanges();
            }

            return RedirectToAction("AuserList");
        }
        public ActionResult AuserList()
        {
            var usrlist = db.dbusers.ToList();
            return View(usrlist);
        }
        [HttpGet]
        public ActionResult AuserUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = db.dbusers.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AuserUpdate(dbuser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AuserList");
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                }
            }

            return View(model);
        }
        public ActionResult AuserDelete(int id)
        {
            var uDel = db.dbusers.FirstOrDefault(x => x.Id == id);
            if (uDel != null)
            {
                db.dbusers.Remove(uDel);
                db.SaveChanges();
            }

            var list = db.dbusers.ToList();
            return View("AuserList", list);

        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("adminLogin");
        }
    }
}