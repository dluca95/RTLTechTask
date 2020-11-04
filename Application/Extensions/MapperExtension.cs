using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Application.Extensions
{
    public static class MapperExtension
    {
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> sources, IMapper mapper)
        {
            return sources.Select(mapper.Map<TSource, TResult>);
        }
    }
}