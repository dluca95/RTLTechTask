using Common;
using Common.Interfaces;

namespace Application.Interfaces.Models
{
    public interface IActorModel: IAppModel
    {
        string Name { get; set; }
        new int? ParentId { get; set; }

        string Birthday { get; set; }
    }
}