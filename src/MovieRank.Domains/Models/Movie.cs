using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace MovieRank.Domains.Models
{
    [DynamoDBTable("MovieRank")]
    public class Movie
    {
        [DynamoDBHashKey]
        public int UserId { get; set; }
        [DynamoDBGlobalSecondaryIndexHashKey]
        public string MovieName { get; set; }
        public string Description { get; set; }
        public List<string> Actors { get; set; }
        public int Ranking { get; set; }
        public string RankedDate { get; set; }
    }
}
