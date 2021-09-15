﻿using ChatingApi.Data;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatingApi.Controllers
{
   
    public class AccountController : BaseApiController
    {
        private readonly DataContext _db;
        public AccountController(DataContext context)
        {
            _db = context;

        }

        [HttpPost("register")]
        //api/Account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("UserName Already Exist");
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
            };
            _db.AppUser.Add(user);
            await _db.SaveChangesAsync();

            return user;

        }
        [HttpPost("login")]
        //api/Account/login
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await _db.AppUser.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid User");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i =0; i< computedHash.Length;i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return user;

        }
        private async Task<bool> UserExists(string userName)
        {
            return await _db.AppUser.AnyAsync(x=>x.UserName==userName.ToLower());
        }
    }
}