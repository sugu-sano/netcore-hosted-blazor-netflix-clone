using System.Text.Json.Serialization;
using NetCoreHostedBlazorNetflixClone.Shared.Domain.Models;

namespace NetCoreHostedBlazorNetflixClone.Server.Infrastructure.Api.Movie
{
    public class MovieApiResponse
    {
        [JsonPropertyName("results")]
        public MovieEntity[] Results { get; set; }
    }
}
