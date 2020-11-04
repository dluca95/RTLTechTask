using Application.Interfaces.Models;
using Application.Models;
using AutoMapper;
using Persistence.Models;

namespace Application.Mappings
{
    public class ShowProfile: Profile
    {
        public ShowProfile()
        {
            CreateMap<IShowModel, Show>()
                .ForMember(dest => dest.Cast, opt => opt.Ignore());
            CreateMap<Show, IShowModel>()
                .ConstructUsing(ctor => new ShowModel { Id = ctor.Id, Name = ctor.Name });
        }
    }
    
    public class ActorProfile: Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorModel, ActorShow>();
            CreateMap<Actor, IActorModel>()
                .ConstructUsing(ctor => new ActorModel { Id = ctor.Id, Name = ctor.Name, Birthday = ctor.Birthday });
            CreateMap<ActorModel, Actor>()
                .ForMember(dest => dest.Shows, opt => opt.Ignore());
        }
    }

    public class ActorShowProfile : Profile
    {
        public ActorShowProfile()
        {
            CreateMap<ActorShow, IActorShowModel>();
            CreateMap<ActorShow, IActorModel>()
                .ConvertUsing<ActorShowConverter>();
            CreateMap<IActorShowModel, ActorShow>()
                .ForMember(dest => dest.Actor, opt => opt.Ignore())
                .ForMember(dest => dest.Show, opt => opt.Ignore());
        }
    }
}