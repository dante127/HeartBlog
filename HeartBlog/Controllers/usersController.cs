using HeartBlog.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
namespace HeartBlog.Controllers
{
    public class usersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        string admin = "admin@admin.com";
        string sl    = "Login";
        bool status  = false;
        string message = "";

        // GET: users
        public ActionResult Index(int? page)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                int pagesize  = 5;
                int pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                return View(db.people.ToList().ToPagedList(pageindex,pagesize));
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //this function is to hash the password:
        public static string Hash(string value)
        {
            return Convert.ToBase64String
                (
               SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }

        //this function to check if email is already exsit:
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var v = dc.people.Where(a => a.email == emailID).FirstOrDefault();
                return v != null;
            }
        }

        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                person user = db.people.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //login function:
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(person login)
        {
            string message = "";
            //check if  email in data base
            var v = db.people.Where(a => a.email == login.email).FirstOrDefault();
                if(v==null) // if the eamil is not found
                {
                    message = "Invalid credential provided";
                }
                else if (v != null) // if the email found we must check the paswword
                {
                // we hash the entered password and check if it the same as in data base 

                if (string.Compare(Hash(login.password), v.password) == 0)
                {
                            Session["Login"] = v.email.ToString();
                            message = "Welcome" + v.Name;
                            FormsAuthentication.SetAuthCookie(v.email, false);
                            return RedirectToAction("Index", "posts");
                }
                else
                {
                        message = "Invalid Password";
                }
            }
            ViewBag.Message = message;
            return View(login);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //   Session["Login"] = null;
            Session.Abandon(); 
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "users");
        }
        // GET: users/Create
        public ActionResult Create()
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        // POST: users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Age,phone,email,password,gender")] person user)
        {
            if (ModelState.IsValid)
            {
                var isexsit = IsEmailExist(user.email);
                if (isexsit)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    ViewBag.err = "email already exist";
                    return View(user);
                }
                user.password = Hash(user.password);
                db.people.Add(user);
                db.SaveChanges();
                message = "Registration successfully done.";
                return RedirectToAction("Index");
            }
            ViewBag.message = message;
            ViewBag.st = status;
            return View(user);
        }

        // GET: users/Edit/5
       
        public ActionResult Edit(int? id)
        {
            if (Session["Login"]!=null && Session["Login"].ToString() == "admin@admin.com")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                person user = db.people.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);

            }
            else
                return RedirectToAction("Login");
        }

        // POST: users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Age,phone,email,password,gender")] person user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                person user = db.people.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            person user = db.people.Find(id);
            db.people.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
