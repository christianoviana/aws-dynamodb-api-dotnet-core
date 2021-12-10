using AutoMapper;
using MovieRank.Domains.DTO.Request;
using MovieRank.Domains.DTO.Response;
using MovieRank.Domains.Models;

namespace MovieRank.Api.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Movie, MovieResponse>();
            CreateMap<Movie, MovieRankResponse>();
            CreateMap<MovieCreateRequest, Movie>()
                .ForMember(m => m.UserId, o => o.Ignore());
            CreateMap<MovieUpdateRequest, Movie>()
               .ForMember(m => m.UserId, o => o.Ignore());
        }
    }
}
