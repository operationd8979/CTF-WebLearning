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
    public class FollowingsController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Follow(Following followingDTO)
        {
            var loginUser = User.Identity.GetUserId();
            followingDTO.FollowerId = loginUser;
            bigschoolContext db = new bigschoolContext();
            Following find = db.Followings.FirstOrDefault(p => p.FollowerId == loginUser && p.FolloweeId == followingDTO.FolloweeId);
            if (find == null)
                db.Followings.Add(followingDTO);
            else
                db.Followings.Remove(find);
            db.SaveChanges();
            return Ok();
        }
    }
}
