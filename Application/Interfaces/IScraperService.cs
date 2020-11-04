using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Interfaces;

namespace Application.Interfaces
{
    public interface IScraperService
    {
        public Task<IEnumerable<T>> ScrapeWith<T>(IRequestModel m) where T: IAppModel;
        Task<IEnumerable<T>> ScrapeManyWithSubPath<T>(string parentPath, IEnumerable<int> parentsId, string subPath)
            where T : IAppModel;
    }
}