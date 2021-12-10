using AutoMapper;
using MovieRank.Domains.DTO.Request;
using MovieRank.Domains.DTO.Response;
using MovieRank.Domains.Gateways;
using MovieRank.Domains.Interfaces;
using MovieRank.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRank.Api.Services
{
    public class MovieRankService: IMovieRankService
    {
        private IMovieRankGateway _gateway;
        private IMapper _mapper;

        public MovieRankService(IMovieRankGateway gateway, IMapper mapper)
        {
            this._gateway = gateway;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<MovieResponse>> GetAllMovies()
        {
            var result = await _gateway.GetAllMovieItems();
            return _mapper.Map<IEnumerable<MovieResponse>>(result);
        }

        public async Task<MovieResponse> GetMovie(int userId, string movieName)
        {
            var result = await _gateway.GetMovieItem(userId, movieName);
            return _mapper.Map<MovieResponse>(result);
        }

        public async Task<IEnumerable<MovieResponse>> GetUsersRankedMovie(int userId, string movieName)
        {
            var result = await _gateway.GetUserRankedItems(userId, movieName);
            return _mapper.Map<IEnumerable<MovieResponse>>(result);
        }

        public async Task AddRankedMovie(int userId, MovieCreateRequest movie)
        {
            var _movie = _mapper.Map<Movie>(movie);
            _movie.UserId = userId;

            await _gateway.AddMovieRankedItem(_movie);
        }

        public async Task UpdateRankedMovie(int userId, MovieUpdateRequest movie)
        {
            var _movie = _mapper.Map<Movie>(movie);
            _movie.UserId = userId;

            await _gateway.UpdateMovieRankedItem(_movie);
        }

        public async Task<MovieRankResponse> GetMovieRank(string movieName)
        {
            var result = await _gateway.GetMovieItemByIndexName(movieName);

            if(result?.Count() > 0)
            {
                var overallRanking = Math.Round(result.Select(m => m.Ranking).Average());

                return new MovieRankResponse(movieName, overallRanking);
            }

            return null;          
        }
    }
}
