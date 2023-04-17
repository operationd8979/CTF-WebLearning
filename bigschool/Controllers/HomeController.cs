using bigschool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace bigschool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(String searchString,String tileCategory)
        {
            bigschoolContext _db = new bigschoolContext();
            searchString += "";
            String userid = User.Identity.GetUserId();
            List<Attendance> attendances = Attendance.GetAll().Where(a => a.Attendee == userid).ToList();
            var upcomingCourse = _db.Courses.Where(p => p.DateTime > DateTime.Now && p.Name.Contains(searchString) && p.IsCanceled!=true).OrderBy(p => p.DateTime).ToList();
            foreach (Attendance attendance in attendances)
            {
                foreach(Course c in upcomingCourse)
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
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                listview.Add(new SelectListItem { Text = user.FullName, Value = i.Id.ToString() });

                Following findFollow = _db.Followings.FirstOrDefault(p => p.FollowerId == userid && p.FolloweeId == i.LecturerId);
                if (findFollow != null)
                {
                    i.isShowFollow = true;
                }
            }
            ViewBag.user = listview;
            ViewBag.LoginUser = User.Identity.GetUserId();
            ViewBag.searchString = searchString;
            ViewBag.tileCategory = tileCategory;
            ViewBag.ListCategory = Category.GetAll().Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
            //keyword tìm kiếm
            return View(upcomingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}