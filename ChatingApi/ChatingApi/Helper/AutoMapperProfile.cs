using AutoMapper;
using ChatingApi.DTOs;
using ChatingApi.Entities;
using ChatingApi.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Helper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(des => des.PhotoURL, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(des => des.Age, opt => opt.MapFrom(src =>src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom
                (src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
               .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom
               (src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
