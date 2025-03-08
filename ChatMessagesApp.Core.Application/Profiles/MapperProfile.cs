using AutoMapper;
using ChatMessagesApp.Core.Application.Features.Demands.Commands.CreateDemand;
using ChatMessagesApp.Core.Application.Features.Users.Commands;
using ChatMessagesApp.Core.Domain.Entities;

namespace ChatMessagesApp.Core.Application.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ChatMessagesApp.Core.Domain.DataObj.User, ChatMessagesApp.Core.Application.Features.Users.Commands.UserCommand>()
            .ReverseMap();


        CreateMap<UserCommand, LoginUserDto>().ReverseMap();
        CreateMap<CreateDemandCommand, CreateDemandDto>().ReverseMap();
        CreateMap<Demand, CreateDemandDto>().ReverseMap();
    }
}
