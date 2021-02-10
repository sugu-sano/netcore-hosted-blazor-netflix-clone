using Microsoft.Extensions.Options;
using NetCoreHostedBlazorNetflixClone.Server.Options;
using NetCoreHostedBlazorNetflixClone.Shared.Domain.Models.Movie;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace NetCoreHostedBlazorNetflixClone.Server.Infrastructure.Api.Movie
{
    public class MovieRepository : IMovieRepository
    {
        private readonly HttpClient httpClient;
        private readonly TmdbOptions tmdbOptions;
        private static readonly Uri baseUri = new Uri("https://api.themoviedb.org/");
        private static readonly ReadOnlyDictionary<MovieCategory, Uri> CategoryUri = new ReadOnlyDictionary<MovieCategory, Uri>(new Dictionary<MovieCategory, Uri>
        {
            [MovieCategory.Trending] = new Uri(baseUri, "/3/trending/all/week?language=en-us"),
            [MovieCategory.NetflixOriginals] = new Uri(baseUri, "/3/discover/tv?with_networks=213"),
            [MovieCategory.TopRated] = new Uri(baseUri, "/3/discover/tv?languager=en-us"),
            [MovieCategory.ActionMovies] = new Uri(baseUri, "/3/discover/tv?with_genres=28"),
            [MovieCategory.ComedyMovies] = new Uri(baseUri, "/3/discover/tv?with_genres=35"),
            [MovieCategory.HorrorMovies] = new Uri(baseUri, "/3/discover/tv?with_genres=27"),
            [MovieCategory.RomanceMovies] = new Uri(baseUri, "/3/discover/tv?with_genres=10749"),
            [MovieCategory.DocumentMovies] = new Uri(baseUri, "/3/discover/tv?with_genres=99"),
        });
        private static readonly Uri imageBaseUri = new Uri("https://image.tmdb.org/t/p/original");

        public MovieRepository(HttpClient httpClient, IOptions<TmdbOptions> options)
        {
            this.httpClient = httpClient;
            this.tmdbOptions = options.Value;
        }

        public async ValueTask<MovieEntity[]> ListAsync(MovieCategory category)
        {
            if (!CategoryUri.TryGetValue(category, out Uri uri))
            {
                throw new NotImplementedException($"{nameof(MovieCategory)}: {category}");
            }

            uri = this.AppendApiKey(uri);

            HttpResponseMessage response = await this.httpClient.GetAsync(uri);

            string content = await (response.Content.ReadAsStringAsync() ?? Task.FromResult(""));

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response}{Environment.NewLine}{content}");
            }

            MovieApiResponse data = JsonSerializer.Deserialize<MovieApiResponse>(content);

            return data.Results.Select(m => { SetImageBasePath(m); return m; }).ToArray();
        }

        private Uri AppendApiKey(Uri uri)
        {
            var uriBuilder = new UriBuilder(uri);
            NameValueCollection querystring = HttpUtility.ParseQueryString(uri.Query);
            querystring["api_key"] = this.tmdbOptions.ApiKeyV3;
            uriBuilder.Query = querystring.ToString();
            return uriBuilder.Uri;
        }

        private static void SetImageBasePath(MovieEntity movie)
        {
            movie.BackdropPath = new Uri(imageBaseUri, "/t/p/original" + movie.BackdropPath).ToString();
            movie.PsoterPath = new Uri(imageBaseUri, "/t/p/original" + movie.PsoterPath).ToString();
        }
    }
}
