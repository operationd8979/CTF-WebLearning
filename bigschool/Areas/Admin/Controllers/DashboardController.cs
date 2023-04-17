using bigschool.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bigschool.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {
        // GET: Admin/Admin
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}