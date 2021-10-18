using AutoMapper;
using ChatingApi.Data;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Interface;
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
    [Authorize]

    public class UserController : BaseApiController

    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser()
        //{
        //    var user = await _userRepository.GetUsersAsync();
        //    var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(user);
        //    return Ok(userToReturn);
           
        //}
        [HttpGet]
      
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser()
        {
            var user = await _userRepository.GetMembersAsync();
          
            return Ok(user);

        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
           
        }
     
    }
}
