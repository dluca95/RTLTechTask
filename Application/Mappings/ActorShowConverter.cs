using Application.Interfaces.Models;
using Application.Models;
using AutoMapper;
using Persistence.Models;

namespace Application.Mappings
{
    public class ActorShowConverter: ITypeConverter<ActorShow, IActorModel>
    {
        public IActorModel Convert(ActorShow source, IActorModel destination, ResolutionContext context)
        {
            return new ActorModel
            {
                Id = source.ActorId,
                Name = source.Actor.Name,
                Birthday = source.Actor.Birthday
            };
        }
    }
}