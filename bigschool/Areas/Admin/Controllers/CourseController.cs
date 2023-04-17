using bigschool.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;

namespace bigschool.Areas.Admin.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class CourseController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            bigschoolContext _db = new bigschoolContext();
            var upcomingCourse = _db.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            List<SelectListItem> listview = new List<SelectListItem>();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                listview.Add(new SelectListItem { Text = user.FullName, Value = i.Id.ToString() });
            }
            ViewBag.user = listview;
            ViewBag.LoginUser = User.Identity.GetUserId();
            ViewBag.Time = DateTime.Now;
            return View(upcomingCourse);
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
                course.DateTime= dateTime;
                course.Update();
                return RedirectToAction("Index");
            }
            return View("Edit", course);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Course course = Course.FindById(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {      
            using (bigschoolContext db = new bigschoolContext())
            {
                try
                {
                    Course course = db.Courses.FirstOrDefault(c=>c.Id==id);
                    if (course == null)
                    {
                        return View("Delete", course);
                    }
                    
                    List<Attendance> attendances = db.Attendances.Where(a=>a.CourseId==id).ToList();
                    if(attendances.Count > 0)
                    {
                        foreach(Attendance attend in attendances)
                        {
                            db.Entry(attend).State = EntityState.Modified;
                            db.Attendances.Remove(attend);
                            db.SaveChanges();
                        }   
                    }
                    db.Entry(course).State = EntityState.Modified;
                    db.Courses.Remove(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

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
        public ActionResult Create(Course course, string date, string time)
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
                return RedirectToAction("Index", "Course");
            }
            //return RedirectToAction(time + date+ "(*)" + course.DateTime.ToString(), "abc");
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/theme/imgs/" + file.FileName));
            return "/Content/theme/imgs/" + file.FileName;
        }

    }
}