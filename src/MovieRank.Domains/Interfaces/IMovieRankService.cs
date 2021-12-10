using MovieRank.Domains.DTO.Request;
using MovieRank.Domains.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRank.Domains.Interfaces
{
    public interface IMovieRankService
    {
        Task<IEnumerable<MovieResponse>> GetAllMovies();
        Task<MovieResponse> GetMovie(int userId, string movieName);
        Task<IEnumerable<MovieResponse>> GetUsersRankedMovie(int userId, string movieName);
        Task AddRankedMovie(int userId, MovieCreateRequest movie);
        Task UpdateRankedMovie(int userId, MovieUpdateRequest movie);
        Task<MovieRankResponse> GetMovieRank(string movieName);

    }
}
