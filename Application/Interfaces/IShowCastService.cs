using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Models;

namespace Application.Interfaces
{
    public interface IShowCastService
    {
        public IEnumerable<IShowModel> GetNewShowsFrom(IEnumerable<IShowModel> showModels);
        public IEnumerable<IActorModel> GetNewActorsFrom(IEnumerable<IShowModel> showModels);
        public Task<IEnumerable<IActorShowModel>> GetAllCastFor(IEnumerable<IShowModel> showModels);
    }
}