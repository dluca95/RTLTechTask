using Application.Interfaces.Models;
using Persistence.Models;

namespace Application.Models
{
    public class ActorShowModel: IActorShowModel
    {
        public ShowModel Show { get; set; }
        public int ShowId { get; set; }
        public ActorModel Actor { get; set; }
        public int ActorId { get; set; }
    }
}