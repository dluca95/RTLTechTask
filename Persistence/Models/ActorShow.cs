using Common;
using Common.Interfaces;

namespace Persistence.Models
{
    public class ActorShow: IEntityModel
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
    }
}