﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreHostedBlazorNetflixClone.Shared.Domain.Models.Movie;
using System;
using System.Threading.Tasks;

namespace NetCoreHostedBlazorNetflixClone.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        [HttpGet, Route("{category?}")]
        public async ValueTask<ActionResult<MovieEntity[]>> Get([FromRoute] MovieCategory? category)
        {
            if (!category.HasValue)
            {
                return this.RedirectToAction(nameof(Get), new { category = MovieCategory.Trending });
            }

            if (!Enum.IsDefined(category.Value))
            {
                return this.StatusCode(StatusCodes.Status404NotFound);
            }

            MovieEntity[] movies = await this.movieRepository.ListAsync(category.Value);

            return movies;
        }
    }
}
