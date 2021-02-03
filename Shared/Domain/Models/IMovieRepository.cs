using System;
using System.Threading.Tasks;

namespace NetCoreHostedBlazorNetflixClone.Shared.Domain.Models
{
    public interface IMovieRepository
    {
        ValueTask<MovieEntity[]> ListAsync(MovieCategory category);
    }
}
