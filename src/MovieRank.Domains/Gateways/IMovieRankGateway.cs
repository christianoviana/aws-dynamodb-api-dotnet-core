using MovieRank.Domains.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRank.Domains.Gateways
{
    public interface IMovieRankGateway
    {
        Task<IEnumerable<Movie>> GetAllMovieItems();
        Task<Movie> GetMovieItem(int userId, string movieName);
        Task<IEnumerable<Movie>> GetMovieItemByIndexName(string movieName);
        Task<IEnumerable<Movie>> GetUserRankedItems(int userId, string movieName);
        Task AddMovieRankedItem(Movie movie);
        Task UpdateMovieRankedItem(Movie movie);
    }
}
