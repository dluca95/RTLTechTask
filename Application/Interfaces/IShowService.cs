using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces.Models;
using Common;
using Common.Interfaces;

namespace Application.Interfaces
{
    public interface IShowService<T> where T: IShowModel
    {
        public Task<IEnumerable<T>> ScrapeShowsWithCast(IRequestModel model);
        public Task<IEnumerable<T>> GetShows(IRequestModel model);
        public Task AddShows(IEnumerable<T> data);
    }
}