using System.Threading.Tasks;

namespace NetCoreHostedBlazorNetflixClone.Shared.Domain.Models.Movie
{
    public interface IMovieRepository
    {
        ValueTask<MovieEntity[]> ListAsync(MovieCategory category);
    }
}
