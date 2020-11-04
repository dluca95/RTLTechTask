using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IHttpClient
    {
        Task<IEnumerable<T>> GetAll<T>(string path);
    }
}