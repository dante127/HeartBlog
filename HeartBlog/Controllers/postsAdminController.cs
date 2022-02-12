using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HeartBlog.Models;
using PagedList;
using CKSource.FileSystem;

namespace HeartBlog.Controllers
{
    [ValidateInput(false)]
    public class postsAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        string admin = "admin@admin.com";
        string sl = "Login";
        // GET: postsAdmin
        public ActionResult Index(int? page)
        {
            int pagesize = 5;
            int pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                return View(db.posts.ToList().ToPagedList(pageindex, pagesize));
            }
            else
            {
                return RedirectToAction("Login","users");
              }
        }

        // GET: postsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                post post = db.posts.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                return View(post);
            }
            else
            {
                return View("Login", "users");
            }
        }

        // GET: postsAdmin/Create
        public ActionResult Create()
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
                return View();
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        // POST: postsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,DateTime,title,shortBody,image,cat_id,numofvisitor")] post post,string image)
        {
            if (ModelState.IsValid)
            {
                post.DateTime = DateTime.Now;

                //  ViewBag.ca = post.category.Name;
                //   string a =  Server.MapPath("~/img/" + post.image);
                //Use Namespace called :  System.IO  
                string FileName = Path.GetFileNameWithoutExtension(post.image);

                //To Get File Extension  
                string FileExtension = Path.GetExtension(post.image);

                //Add Current Date To Attached File Name  
                FileName = FileName.Trim() + FileExtension;

                //Get Upload path from Web.Config file AppSettings.  
                string UploadPath = "/img/";

                //Its Create complete path to store in server.  
                post.image = UploadPath + FileName;

                //To copy and save file into server. 

                db.posts.Add(post);
              db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: postsAdmin/Edit/5
   
        public ActionResult Edit(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                post post = db.posts.Find(id);
                if (post == null)
                {
                    return HttpNotFound();
                }
                return View(post);
            }
            else
            {
                return RedirectToAction("Login", "users");
            }
        }

        // POST: postsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Body,DateTime,title,shortBody,image,cat_id,numofvisitor")] post post)
        {

            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: postsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session[sl] != null && Session[sl].ToString() == admin) 
            { 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            post post = db.posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
            else
            {
                return RedirectToAction("Login", "users");
            }

        }

        // POST: postsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            post post = db.posts.Find(id);
            db.posts.Remove(post);
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
