using ChatingApi.Data;
using ChatingApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Controllers
{

    public class UserController : BaseApiController

    {
        private readonly DataContext _db;
        public UserController(DataContext context)
        {
            _db = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser()
        {
            return await _db.AppUser.ToListAsync();
           
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _db.AppUser.FindAsync(id);
           
        }
    }
}
