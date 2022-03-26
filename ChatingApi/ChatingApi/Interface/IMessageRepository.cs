using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Interface
{
    public interface IMessageRepository
    {
        public void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);

        Task<IEnumerable<MessageDto>> GetMassageThread(string currentUserName, string recipientUserName);
        Task<bool> SaveAllAsync();

    }
}
