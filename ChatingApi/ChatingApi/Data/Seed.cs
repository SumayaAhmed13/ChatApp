﻿using ChatingApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatingApi.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext _db)
        {
            if (await _db.AppUser.AnyAsync()) return;
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("fake@123"));
                user.PasswordSalt = hmac.Key;
                _db.AppUser.Add(user);
            }
            await _db.SaveChangesAsync();
        }
    }
}
