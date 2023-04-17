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
    public class FlagController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Challenge challengeDTO)
        {
            var userID = User.Identity.GetUserId();
            if (userID == null)
                return BadRequest("Please login first!");
            bigschoolContext db = new bigschoolContext();
            Challenge challenge = Challenge.FindById(challengeDTO.Id);
            if (challenge.flag == challengeDTO.flag)
            {
                /*try
                {
                    DetailClaim detailClaim = new DetailClaim();
                    detailClaim.IdUser = userID;
                    detailClaim.IdChallenge = challenge.Id;
                    detailClaim.Time = DateTime.Now;
                    db.DetailClaims.Add(detailClaim);
                    db.SaveChanges();
                }catch(Exception ex)
                {
                    throw ex;
                }
                throw (new Exception());*/
                return Ok();
            }
            return BadRequest("Flag incorect!");
        }
    }
    
}
