using AutoMapper;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
namespace ChatingApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context,IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }
        

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _db.AppUser.FindAsync(id);
        }

        public async Task<AppUser>GetUserByUsernameAsync(string username)
        {
            return await _db.AppUser
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>>GetUsersAsync()
        {
            return await _db.AppUser
                .Include(x=>x.Photos)
                .ToListAsync();
        }

        public async Task<bool>SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
             _db.Entry(user).State = EntityState.Modified;
        }
        public async Task<IEnumerable<MemberDto>>GetMembersAsync()
        {
            return await _db.AppUser.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();

        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _db.AppUser.Where(c => c.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        //public async Task<MemberDto> GetMemberAsync(string username)
        //{
        //    return await _context.Users
        //        .Where(x => x.UserName == username)
        //        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        //        .SingleOrDefaultAsync();
        //}
    }
}
