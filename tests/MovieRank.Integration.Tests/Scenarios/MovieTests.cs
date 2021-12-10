using MovieRank.Api;
using MovieRank.Domains.DTO.Request;
using MovieRank.Domains.DTO.Response;
using MovieRank.Domains.Models;
using MovieRank.Integration.Tests.Setup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.Integration.Tests.Scenarios
{
    [Collection("api")]
    public class MovieTests:IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        readonly HttpClient _client;
        public MovieTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddMovieAndReturnsOkStatus()
        {
            var userId = 1;

            var response = await AddMovie(userId);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetLeastOneMovieAndReturnsOkStatus()
        {
            var userId = 2;

            var response = await AddMovie(userId);

            var moviesRequest = await _client.GetAsync($"movies");

            using var content = moviesRequest.Content.ReadAsStringAsync();
            var movies = JsonConvert.DeserializeObject<IEnumerable<MovieResponse>>(content.Result);
          
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(movies);
            Assert.True(movies.Count() > 0);
        }

        [Fact]
        public async Task GetMovieAndReturnsExpectedMovieName()
        {
            var userId = 3;
            var movieName = "TesteMovie3";

            var response = await AddMovie(userId);

            var movieRequest = await _client.GetAsync($"movies/{userId}/{movieName}");

            using var content = movieRequest.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<MovieResponse>(content.Result);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(movie);
            Assert.Equal(movieName, movie.MovieName);
        }

        private async Task<HttpResponseMessage> AddMovie(int userId)
        {
            var movie = new MovieCreateRequest()
            {
                MovieName = $"TestMovie{userId}",
                Description = "Test Description",
                Ranking = 2,
                RankedDate = DateTime.Now.ToString(),
                Actors = new List<string>()
                {
                    "Actor1",
                    "Actor2"
                }
            };

            var json = JsonConvert.SerializeObject(movie);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"movies/{userId}", jsonContent);

            return response;
        }
    }
}
