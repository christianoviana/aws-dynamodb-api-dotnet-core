using System.Collections.Generic;

namespace MovieRank.Domains.DTO.Request
{
    public class MovieCreateRequest
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public List<string> Actors { get; set; }
        public int Ranking { get; set; }
        public string RankedDate { get; set; }
    }
}
