using bigschool.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.WebPages;
using static System.Net.Mime.MediaTypeNames;


namespace bigschool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            bigschoolContext _db = new bigschoolContext();
            Course course = new Course();
            course.ListCategory = _db.Categories.ToList();
            return View(course);
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public ActionResult Create(Course course,string date,string time)
        {
            bigschoolContext _db = new bigschoolContext();
            
            ModelState.Remove("LecturerID");
            if (!ModelState.IsValid)
            {
                course.ListCategory = _db.Categories.ToList();
                return View("Create", course);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            course.LecturerId = user.Id;
            DateTime dateTime = DateTime.Parse(DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).ToString("M/d/yyyy hh:mm:ss tt"));
            {
                course.DateTime = dateTime;
                _db.Courses.Add(course);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            //return RedirectToAction(time + date+ "(*)" + course.DateTime.ToString(), "abc");
        }

        public ActionResult Processing(String searchString, String tileCategory)
        {
            bigschoolContext _db = new bigschoolContext();
            String userid = User.Identity.GetUserId();
            List<Attendance> attendances = Attendance.GetAll().Where(a => a.Attendee == userid).ToList();
            searchString += "";
            foreach(Attendance attendance in attendances)
            {
                attendance.Course = Course.FindById(attendance.CourseId);
            }
            var upcomingCourse = attendances.Select(a=> new Course(a.Course));
            if (tileCategory != null && tileCategory != "-1")
            {
                int tile = int.Parse(tileCategory);
                upcomingCourse = upcomingCourse.Where(p => p.CategoryId == tile).ToList();
            }
            List<SelectListItem> listview = new List<SelectListItem>();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                listview.Add(new SelectListItem { Text = user.FullName, Value = i.Id.ToString() });
            }
            ViewBag.user = listview;
            ViewBag.LoginUser = User.Identity.GetUserId();
            ViewBag.searchString = searchString;
            ViewBag.tileCategory = tileCategory;
            ViewBag.ListCategory = Category.GetAll().Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
            //keyword tìm kiếm
            return View(upcomingCourse);
        }
        public ActionResult Teaching()
        {
            bigschoolContext _db = new bigschoolContext();
            String userid = User.Identity.GetUserId();
            var upcomingCourse = _db.Courses.Where(p => p.LecturerId == userid && p.IsCanceled!= true).OrderBy(p => p.DateTime).ToList();
            ViewBag.Time = DateTime.Now;
            return View(upcomingCourse);
        }
        public ActionResult Delete(int id)
        {
            var userID = User.Identity.GetUserId();
            using (bigschoolContext db = new bigschoolContext())
            {
                Course course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
                course.IsCanceled = true;
                db.SaveChanges();
            }
            return RedirectToAction("Teaching");
        }
        public ActionResult Edit(int id)
        {
            if (id == null)
                return HttpNotFound();
            Course course = Course.FindById(id);
            if (course == null)
                return HttpNotFound();
            bigschoolContext _db = new bigschoolContext();
            course.ListCategory = _db.Categories.ToList();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(course.LecturerId);
            ViewBag.user = user.FullName;
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course, string date, string time)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTime = DateTime.Parse(DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).ToString("M/d/yyyy hh:mm:ss tt"));
                course.DateTime = dateTime;
                course.Update();
                return RedirectToAction("Index");
            }
            return View("Edit", course);
        }
        public ActionResult Follower(String searchString, String tileCategory)
        {
            bigschoolContext _db = new bigschoolContext();
            searchString += "";
            String userid = User.Identity.GetUserId();
            List<Attendance> attendances = Attendance.GetAll().Where(a => a.Attendee == userid).ToList();
            List<Course> upcomingCourse = _db.Courses.Where(p => p.DateTime > DateTime.Now && p.Name.Contains(searchString) && p.IsCanceled != true).OrderBy(p => p.DateTime).ToList();
            foreach (Attendance attendance in attendances)
            {
                foreach (Course c in upcomingCourse)
                {
                    if (c.Id == attendance.CourseId)
                        c.isShowGoing = true;
                }
            }
            if (tileCategory != null && tileCategory != "-1")
            {
                int tile = int.Parse(tileCategory);
                upcomingCourse = upcomingCourse.Where(p => p.CategoryId == tile).ToList();
            }
            List<SelectListItem> listview = new List<SelectListItem>();
            List<Following> listFollow = _db.Followings.Where(f => f.FollowerId == userid).ToList();
            List<Course> showlist = new List<Course>();
            foreach (Course i in upcomingCourse)
            {
                foreach(Following f in listFollow){
                    if (f.FolloweeId == i.LecturerId)
                    {
                        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                        listview.Add(new SelectListItem { Text = user.FullName, Value = i.Id.ToString() });
                        Following findFollow = _db.Followings.FirstOrDefault(p => p.FollowerId == userid && p.FolloweeId == i.LecturerId);
                        if (findFollow != null)
                        {
                            i.isShowFollow = true;
                        }
                        showlist.Add(i);
                    }
                }
                
            }
            ViewBag.user = listview;
            ViewBag.LoginUser = User.Identity.GetUserId();
            ViewBag.searchString = searchString;
            ViewBag.tileCategory = tileCategory;
            ViewBag.ListCategory = Category.GetAll().Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
            //keyword tìm kiếm
            return View(showlist);
        }

        public ActionResult demo(int? id)
        {
            ViewBag.id = 1;
            if (id == null)
                id = 1;
            bigschoolContext db = new bigschoolContext();
            List<DetailKH> detailCourses = db.DetailKHs.ToList();
            ViewBag.id = id;
            return View(detailCourses);
        }


    }
}