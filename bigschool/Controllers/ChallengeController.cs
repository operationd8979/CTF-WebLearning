using bigschool.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Data.Entity;

namespace bigschool.Controllers
{
    public class ChallengeController : Controller
    {
        // GET: Challenge
        public ActionResult Index(int? page,String searchString)
        {
            searchString += "";
            List<Challenge> challenges;
            using (bigschoolContext db = new bigschoolContext())
            {
                challenges = db.Challenges.Where(c=>c.Name.Contains(searchString)).ToList();
            } 
            if (page == null)
                page = 1;
            int pageSize = 6;
            List<SelectListItem> listview = new List<SelectListItem>();
            foreach (Challenge i in challenges)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.IdUser);
                listview.Add(new SelectListItem { Text = user.FullName, Value = i.Id.ToString() });
            }
            ViewBag.users=listview;
            ViewBag.searchString = searchString;
            return View(challenges.ToPagedList(page.Value, pageSize));
        }

        public ActionResult Create()
        {
            bigschoolContext _db = new bigschoolContext();
            Challenge challenge = new Challenge();
            //challenge.ListCategory = _db.Categories.ToList();
            return View(challenge);
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public ActionResult Create(Challenge challenge, string date, string time)
        {
            try
            {
                bigschoolContext _db = new bigschoolContext();
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                challenge.IdUser = user.Id;
                DateTime dateTime = DateTime.Parse(DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).ToString("M/d/yyyy hh:mm:ss tt"));
                {
                    challenge.TimePost = dateTime;
                    _db.Challenges.Add(challenge);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Challenge");
                }
            }catch(Exception ex)
            {
                return View("Create", challenge);
            }
            
        }

        public ActionResult Detail(int id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Challenge challenge = Challenge.FindById(id);
            if (challenge == null)
            {
                return HttpNotFound();
            }
            return View(challenge);
        }   


        public ActionResult MyChallenge()
        {
            bigschoolContext _db = new bigschoolContext();
            String userid = User.Identity.GetUserId();
            var upcomingCourse = _db.Challenges.Where(c => c.IdUser == userid).OrderBy(c => c.TimePost).ToList();
            ViewBag.LoginUser = userid;
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
                    Course course = db.Courses.FirstOrDefault(c => c.Id == id);
                    if (course == null)
                    {
                        return View("Delete", course);
                    }

                    List<Attendance> attendances = db.Attendances.Where(a => a.CourseId == id).ToList();
                    if (attendances.Count > 0)
                    {
                        foreach (Attendance attend in attendances)
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
    }
}