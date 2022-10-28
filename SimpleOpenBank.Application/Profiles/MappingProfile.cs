using AutoMapper;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOpenBank.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountBD, AccountResponse>().ReverseMap();
            CreateMap<UserBD, CreateUserResponse>().ReverseMap();
            CreateMap<MovimBD, MovimResponse>().ReverseMap();
        }
    }
}
