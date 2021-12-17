using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Extension;
using ChatingApi.Helper;
using ChatingApi.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ChatingApi.Data
{
    public class LikeRepository : ILikesRepository
    {
        private readonly DataContext _db;
        public LikeRepository(DataContext db)
        {
            _db = db;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _db.Like.FindAsync(sourceUserId, likedUserId);
        }

       public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
       {
            var users = _db.AppUser.OrderBy(c => c.UserName).AsQueryable();
            var likes = _db.Like.AsQueryable();

            if (likeParams.Predicate == "liked")
            {
                likes = likes.Where(likes => likes.SourceUserId == likeParams.UserId);
                users = likes.Select(likes => likes.LikedUser);

            }
            if (likeParams.Predicate == "likedBy")
            {
                likes = likes.Where(likes => likes.LikedUserId== likeParams.UserId);
                users = likes.Select(likes => likes.SourceUser);

            }
            var likeUser = users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(c => c.IsMain).Url,
                City = user.City,
                Id = user.Id

            });
            return await PagedList<LikeDto>.CreateAsync(likeUser, likeParams.PageNumber, likeParams.PageSize);

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _db.AppUser
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(c => c.Id == userId);
        }
    }
}
