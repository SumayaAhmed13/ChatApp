using ChatingApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
