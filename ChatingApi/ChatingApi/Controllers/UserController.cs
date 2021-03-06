using AutoMapper;
using ChatingApi.Data;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Extension;
using ChatingApi.Helper;
using ChatingApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatingApi.Controllers
{
    [Authorize]

    public class UserController : BaseApiController

    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UserController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }
        #region Dead Code
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser()
        //{
        //    var user = await _userRepository.GetUsersAsync();
        //    var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(user);
        //    return Ok(userToReturn);

        //}
        //[HttpGet]

        //public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser([FromQuery] UserParams userParams)
        //{
        //    var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
        //    userParams.CurrentUserName = user.UserName;
        //    if (userParams.Gender.Contains('\n'))
        //    {
        //        string[] a = userParams.Gender.Split('\n');
        //        userParams.Gender = a[0];
              
        //    }

        //    if (string.IsNullOrEmpty(userParams.Gender))
        //     userParams.Gender = user.Gender.Equals("male") ? "female" : "male";
               
        //    var users = await _userRepository.GetMembersAsync(userParams);
        //    Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

        //    return Ok(users);

        //}
        #endregion


        [HttpGet]
      
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUser([FromQuery]UserParams userParams)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            userParams.CurrentUserName = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
            userParams.Gender = user.Gender == "male" ? "female" : "male";
            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
          
            return Ok(users);

        }

        [HttpGet("{username}",Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
           
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            _mapper.Map(memberUpdateDTO, user);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("User Update Failed");


        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var result = await _photoService.AddPhotoAync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            
            };
            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser",new { username=user.UserName }, _mapper.Map<PhotoDto>(photo));
            }
               
            return BadRequest("Problem Adding Photo");


        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo.IsMain) return BadRequest("This is already your main photo");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to set main photo");

        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You can not delete your main Photo");
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete photo");
        }


    }
}
