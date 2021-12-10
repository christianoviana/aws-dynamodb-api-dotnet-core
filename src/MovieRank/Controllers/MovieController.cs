using Microsoft.AspNetCore.Mvc;
using MovieRank.Domains.DTO.Request;
using MovieRank.Domains.DTO.Response;
using MovieRank.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieRank.Api.Controllers
{
    /// <summary>
    /// Movie ranking rest entrypoint
    /// </summary>
    [ApiController]
    [Route("api/v1/movies")]
    public class MovieController : ControllerBase
    {
        private IMovieRankService _movieRankService;

        /// <summary>
        /// Movie ranking constructor
        /// </summary>
        /// <param name="movieRankService">Movie rank service</param>
        public MovieController(IMovieRankService movieRankService)
        {
            this._movieRankService = movieRankService;
        }

        /// <summary>
        /// Get all movies rank in dynamo database
        /// </summary>
        /// <returns>Movie rank data</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetAllItems()
        {
            var result = await _movieRankService.GetAllMovies();
            return Ok(result);
        }

        /// <summary>
        /// Get movie ranking by partition and sort key
        /// </summary>
        /// <param name="userId">Partition key</param>
        /// <param name="movieName">Sort key</param>
        /// <returns></returns>
        [HttpGet("{userId}/{movieName}")]
        public async Task<ActionResult<MovieResponse>> GetMovieByName(int userId, string movieName)
        {
            var result = await _movieRankService.GetMovie(userId, movieName);

            if (result == null)
            {
                var error = new { StatusCode = HttpStatusCode.NotFound, Message = $"The movie {movieName} was not found" };
                return NotFound(error);
            }

            return Ok(result);
        }

        /// <summary>
        /// Get all movies ranking that begin with specifc name by partition and sort key
        /// </summary>
        /// <param name="userId">Partition key</param>
        /// <param name="movieName">Sort key</param>
        /// <returns></returns>
        [HttpGet("{userId}/ranked/{movieName}")]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetUsersRankedMovieByName(int userId, string movieName)
        {
            var result = await _movieRankService.GetUsersRankedMovie(userId, movieName);
            return Ok(result);
        }

        /// <summary>
        /// Add movie rank
        /// </summary>
        /// <param name="userId">Partition key</param>
        /// <param name="movie">Movie rank object</param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public async Task<ActionResult> AddMovie(int userId, [FromBody] MovieCreateRequest movie)
        {
            await _movieRankService.AddRankedMovie(userId, movie);
            return Ok();
        }

        /// <summary>
        /// Update movie rank
        /// </summary>
        /// <param name="userId">Partition key</param>
        /// <param name="movie">Movie rank object</param>
        /// <returns></returns>
        [HttpPatch("{userId}")]
        public async Task<ActionResult> UpdateMovie(int userId, [FromBody] MovieUpdateRequest movie)
        {
            await _movieRankService.UpdateRankedMovie(userId, movie);
            return Ok();
        }

        /// <summary>
        /// Get ranking by movie rank name
        /// </summary>
        /// <param name="movieName">MovieName-index - Index</param>
        /// <returns></returns>
        [HttpGet("{movieName}/ranking")]
        public async Task<ActionResult<MovieRankResponse>> GetMovieRankig(string movieName)
        {
            var result = await _movieRankService.GetMovieRank(movieName);

            if (result == null)
            {
                var error = new { StatusCode = HttpStatusCode.NotFound, Message = $"The movie {movieName} was not found" };
                return NotFound(error);
            }

            return Ok(result);
        }
    }
}
