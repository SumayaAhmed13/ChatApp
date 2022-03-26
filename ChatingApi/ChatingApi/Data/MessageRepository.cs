using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Helper;
using ChatingApi.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }
        public void AddMessage(Message message)
        {
            _db.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _db.Messages.Remove(message);
        }

        public async Task<IEnumerable<MessageDto>> GetMassageThread(string currentUserName, string recipientUserName)
        {
            var messages = await _db.Messages
                          .Include(u=>u.Sender).ThenInclude(p=>p.Photos)
                          .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                          .Where(c => c.Recipient.UserName == currentUserName 
                                      && c.Sender.UserName== recipientUserName
                                      && c.RecipientDeleted==false
                                     ||c.Recipient.UserName== recipientUserName 
                                     && c.Sender.UserName== currentUserName
                                     && c.SenderDeleted==false)
                          .OrderBy(v=>v.MassageSent)
                          .ToListAsync();
            var unreadMessage = messages.Where(c => c.DateRead == null && c.Recipient.UserName == currentUserName).ToList();
            if (unreadMessage.Any())
            {
                foreach( var message in unreadMessage)
                {
                    message.DateRead = DateTime.Now;
                }
                await _db.SaveChangesAsync();
            }
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _db.Messages
                .Include(u=>u.Sender)
                .Include(u=>u.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
               
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _db.Messages.OrderByDescending(c => c.MassageSent)
                       .AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" =>query.Where(u=>u.Recipient.UserName== messageParams.UserName && u.RecipientDeleted==false),
                "Outbox" =>query.Where(u=>u.Sender.UserName== messageParams.UserName && u.SenderDeleted==false),
                _ => query.Where(u => u.Recipient.UserName == messageParams.UserName && u.RecipientDeleted == false && u.DateRead==null)

                // "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
                //    && u.RecipientDeleted == false),
                //"Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                //    && u.SenderDeleted == false),
                //_ => query.Where(u => u.RecipientUsername ==
                //    messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };
            var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(message, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
