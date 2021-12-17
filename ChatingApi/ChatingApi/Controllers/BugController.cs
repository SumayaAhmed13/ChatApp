using ChatingApi.Data;
using ChatingApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Controllers
{
  
    public class BugController : BaseApiController
    {
        private readonly DataContext _db;
        public BugController(DataContext context)
        {
            _db = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret text";
        }
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _db.AppUser.Find(-1);
            if (thing == null) return NotFound();
           
            return Ok(thing);
        }
 
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
               var thing = _db.AppUser.Find(-1);
                var thingToReturn = thing.ToString();
                return thingToReturn;
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
