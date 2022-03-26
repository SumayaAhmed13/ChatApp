using AutoMapper;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Extension;
using ChatingApi.Helper;
using ChatingApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessageController(IUserRepository userRepository, IMapper mapper,IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMassage(CreateMessageDto createMessageDto)
        {

            var username = User.GetUserName();
            if (username == createMessageDto.RecipientUserName.ToLower())
                return BadRequest("You can not send message on Yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient=await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUserName);
            if (recipient == null) return NotFound();
            var message = new Message
            {
                Sender=sender,
                Recipient=recipient,
                SenderUserName=sender.UserName,
                RecipientUserName=recipient.UserName,
                Content= createMessageDto.Content
            };
            _messageRepository.AddMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));
            return BadRequest("Failed to send message");

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            //var userName = User.GetUserName();
            messageParams.UserName = User.GetUserName();
            var message = await _messageRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(message.CurrentPage, message.PageSize, message.TotalCount, message.TotalPages);
            return message;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread(string username)
        {
            var currentUserName = User.GetUserName();
            return Ok(await _messageRepository.GetMassageThread(currentUserName, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMassage( int id)
        {
            var username = User.GetUserName();
            var message = await _messageRepository.GetMessage(id);
            if (message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();
            if (message.Sender.UserName == username) message.SenderDeleted = true;
            if (message.Recipient.UserName == username) message.RecipientDeleted = true;
            if (message.RecipientDeleted && message.SenderDeleted) _messageRepository.DeleteMessage(message);
            if (await _messageRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem Deleting Message");


        }
    }
}
