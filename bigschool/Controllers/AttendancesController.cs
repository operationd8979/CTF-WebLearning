using bigschool.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace bigschool.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course courseDTO)
        {
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Please login first!");
            bigschoolContext bigschoolContext = new bigschoolContext();
            var attendance = new Attendance()
            {
                CourseId = courseDTO.Id,
                Attendee = userID
            };
            bigschoolContext.Attendances.Add(attendance);
            bigschoolContext.SaveChanges();
            return Ok();
        }
    }
}
