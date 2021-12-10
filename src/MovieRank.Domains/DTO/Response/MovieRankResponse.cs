using System;
using System.Collections.Generic;

namespace MovieRank.Domains.DTO.Response
{
    public class MovieRankResponse
    {
        public MovieRankResponse()
        {

        }

        public MovieRankResponse(string movieName, double overallRanking)
        {
            MovieName = movieName;
            OverallRanking = overallRanking;
        }

        public string MovieName { get; set; }
        public double OverallRanking { get; set; }
    }
}
