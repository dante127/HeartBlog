using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using HeartBlog.Models;
using PagedList;
namespace HeartBlog.Controllers
{
    public class postsController : Controller
    {
        //this object is to connect to db and for all operation select ,insert , update and delete 
        private ApplicationDbContext db = new ApplicationDbContext(); 

        // get all posts in main page
        public ActionResult Index(int?page) //show the posts order by number of visitors
        {
            int pagesize = 3; // number of posts in each page 
            int pageindex = 1;// start index 
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            //db.posts mean db table posts and in c# the posts is a list of object and we deal with it as list 
            var posts = db.posts.OrderByDescending(s => s.numofvisitor) //order by desc is the largest number of visitor 

                .Where(s=>s.cat_id==6)//the list is lamba expression
                .ToList().
                ToPagedList(pageindex, pagesize);
         
            return View(posts); //return the list of objec to index view 
        }

        [ActionName("Heartinfo")]
        //get posts by cate
        public ActionResult heart_info(int? page)
        {
            int pagesize = 3; // number of posts in each page 
            int pageindex = 1;// start index 
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            //get the posts by category
            var i = db.posts.Where(s => s.cat_id == 1).ToList().ToPagedList(pageindex,pagesize); 
            //to send info from controller to view
            ViewBag.info = i; //viewbag is a dynamic variable and the scope of it is from controller to view same in controller  

            return View(i);
        }

        [ActionName("disdia")]
        public ActionResult diseaseDiagnosis(int? page) 
        {
            int pagesize = 3; // number of posts in each page 
            int pageindex = 1;// start index 
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            //get the posts for disease Diagnosis page 
            var i = db.posts.Where(s => s.cat_id == 2).ToList().ToPagedList(pageindex, pagesize);
            return View(i);
        }

        [ActionName("distreat")]
        public ActionResult diseaseTreatment(int?page)
        {
            int pagesize = 3; // number of posts in each page 
            int pageindex = 1;// start index 
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            //get the posts for Disease Treatment page
            var i = db.posts.Where(s => s.cat_id == 3).ToList().ToPagedList(pageindex,pagesize);
          
            return View(i);
        }

        [ActionName("livdia")]
        public ActionResult LivingWithDisease(int? page)
        {
            int pagesize = 3; // number of posts in each page 
            int pageindex = 1;// start index 
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            //get the posts for Living With Disease page 
            var i = db.posts.Where(s => s.cat_id == 4).ToList().ToPagedList(pageindex,pagesize);
            return View(i);
        }
        [ActionName("patifood")]
        public ActionResult patientsFood(int?page)
        {
            int pagesize = 3; // number of posts in each page 
            int pageindex = 1;// start index 
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var i = db.posts.Where(s => s.cat_id == 5).ToList().ToPagedList(pageindex,pagesize);
            return View(i);
        }

        //post details 
        [ActionName("P_detials")]
        public ActionResult getPostDetails(int? id)
        {
            var p = db.posts.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            ViewData["img"] = p.image;
            ViewData["ti"] = p.title;
            ViewData["ii"] = p.Id;
            ViewData["det"] = p.Body;
            ViewBag.com = db.comments.Where(s => s.postId == id).ToList();
            ViewBag.size = db.comments.Where(s => s.postId == id).ToList().Count;

            p.numofvisitor = (int)Session["n"];
            db.SaveChanges();
            return View(p);
        }

        [HttpPost]
        [ActionName("P_detials")]
        public ActionResult comment(FormCollection fc)
        {
            string e = Session["Login"].ToString();
            person u = db.people.Where(s => s.email == e).FirstOrDefault();

            int id = u.Id;
            string name = u.Name;
            comment c = new comment();
            c.body = fc["comment"];
            c.name = name;
            c.email = Session["Login"].ToString();
            c.postId = int.Parse(fc["postid"]);
            c.userId = id;
            post p = db.posts.Find(c.postId);
            db.comments.Add(c);

            db.SaveChanges();
            return RedirectToAction("P_detials");


        }
        public ActionResult about()
        {
            return View();
        }

        public ActionResult adminManage()
        {
            if (Session["Login"] != null && Session["Login"].ToString() == "admin@admin.com")
            {
                return View();
            }
            else
                return RedirectToAction("Login", "users");
        }
    }
}
