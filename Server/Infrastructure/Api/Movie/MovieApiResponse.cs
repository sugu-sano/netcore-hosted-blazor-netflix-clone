using NetCoreHostedBlazorNetflixClone.Shared.Domain.Models;
using System.Text.Json.Serialization;

namespace NetCoreHostedBlazorNetflixClone.Server.Infrastructure.Api.Movie
{
    public class MovieApiResponse
    {
        [JsonPropertyName("results")]
        public MovieEntity[] Results { get; set; }
    }
}
