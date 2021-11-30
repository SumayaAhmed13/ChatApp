using AutoMapper;
using ChatingApi.Data;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Interface;
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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenService,IMapper mapper)
        {
            _db = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        //api/Account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("UserName Already Exist");
            using var hmac = new HMACSHA512();

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            user.KnownAs = registerDto.KnowAs;
            user.Gender = registerDto.Gender;
            user.DateOfBirth = registerDto.DateOfBrith;
            user.City = registerDto.City;
            user.Country = registerDto.Country;

            _db.AppUser.Add(user);
            await _db.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnowAs=user.KnownAs,
                Gender=user.Gender
               
            };


        }
        [HttpPost("login")]
        //api/Account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _db.AppUser
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid User");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i =0; i< computedHash.Length;i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnowAs=user.KnownAs,
                Gender=user.Gender
            };

        }
        private async Task<bool> UserExists(string userName)
        {
            return await _db.AppUser.AnyAsync(x=>x.UserName==userName.ToLower());
        }
    }
}
