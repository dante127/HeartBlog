using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HeartBlog.Models;
using CaptchaMvc.HtmlHelpers;
using PagedList;
namespace HeartBlog.Controllers
{
    public class consultsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        string admin = "admin@admin.com";
        string sl = "Login";
        // GET: consults
        public ActionResult Index(int? page)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                int pagesize = 5;
                int pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1; // for show the results on multiple pages
                var consults = db.consults.Include(c => c.user);
                return View(consults.ToList().ToPagedList(pageindex,pagesize));
            }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        [HttpGet]
        public ActionResult myconsult()
        {
            if (Session[sl] != null)
            {
                string usermail = Session[sl].ToString();
                person u = db.people.Where(s => s.email == usermail).FirstOrDefault();
                consult c = db.consults.Where(s => s.userid == u.Id).FirstOrDefault();
                if (c != null)
                {
                    ViewBag.body = c.body;
                    if (c.answer != null)
                    {
                        ViewBag.ans = c.answer;

                    }
                }
               else { ViewBag.body = "You did not submit any Consulation yet"; }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        [HttpGet]
        [ActionName("usercons")]
        public ActionResult usercons()
        {
            if(Session[sl] ==null)
            {
                return RedirectToAction("Login", "users");
            }
            return View();
        }

        [HttpPost]
        [ActionName("usercons")]
        public ActionResult Postusercons(string history, string body)
        {
           

            string usermail = Session[sl].ToString();
            person u = db.people.Where(s => s.email == usermail).FirstOrDefault();
            int userid = u.Id;
            consult c = new consult();
            c.history = history;
            c.body = body;
            c.DateTime = DateTime.Now;
            c.userid = userid;
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                db.consults.Add(c);
                db.SaveChanges();
                ViewBag.ErrMessage = "your conslation is sent sucssessfuly";

            }
            else
            {
                ViewBag.ErrMessage = "Error: captcha is not valid";
            }
            return View();
            
        }

        public ActionResult consreq()
        {
            if(Session[sl]!=null)
            return View();
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        // GET: consults/Details/5
        public ActionResult Details(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                consult consult = db.consults.Find(id);
                if (consult == null)
                {
                    return HttpNotFound();
                }
                return View(consult);
            }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        // GET: consults/Create
        public ActionResult Create()
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                ViewBag.userid = new SelectList(db.people, "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        // POST: consults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,history,body,userid,answer,DateTime")] consult consult)
        {
            if (ModelState.IsValid)
            {
                db.consults.Add(consult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userid = new SelectList(db.people, "Id", "Name", consult.userid);
            return View(consult);
        }

        // GET: consults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                consult consult = db.consults.Find(id);
                if (consult == null)
                {
                    return HttpNotFound();
                }
                ViewBag.userid = new SelectList(db.people, "Id", "Name", consult.userid);
                return View(consult);
            }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }


        // POST: consults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,history,body,userid,answer,DateTime")] consult consult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userid = new SelectList(db.people, "Id", "Name", consult.userid);
            return View(consult);
        }

        // GET: consults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session[sl] !=null && Session[sl].ToString()==admin)
            { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            consult consult = db.consults.Find(id);
            if (consult == null)
            {
                return HttpNotFound();
            }
            return View(consult);
        }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        // POST: consults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            consult consult = db.consults.Find(id);
            db.consults.Remove(consult);
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
