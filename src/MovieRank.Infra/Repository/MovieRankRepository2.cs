using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using MovieRank.Domains.DTO.Request;
using MovieRank.Domains.DTO.Response;
using MovieRank.Domains.Gateways;
using MovieRank.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRank.Infra.Repository
{
    public class MovieRankRepository2
    {
        private const string TableName = "MovieRank";
        private readonly IAmazonDynamoDB _dynamoClient;
        public MovieRankRepository2(IAmazonDynamoDB client)
        {
            _dynamoClient = client;
        }

        public async Task<ScanResponse> GetAllMovieItems()
        {
            var request = new ScanRequest(TableName);

            var result = await _dynamoClient.ScanAsync(request);

            return result;
        }

        public async Task<GetItemResponse> GetMovieItem(int userId, string movieName)
        {
            var request = new GetItemRequest(TableName, new Dictionary<string, AttributeValue>()
            {
                {"UserId", new AttributeValue(){ N = userId.ToString() } },
                {"MovieName", new AttributeValue(){ S = movieName} }
            });

            var result = await _dynamoClient.GetItemAsync(request); 

            return result;
        }

        public async Task<QueryResponse> GetUserRankedItems(int userId, string movieName)
        {
            var request = new QueryRequest(TableName)
            {
                KeyConditionExpression = "UserId = :userId and begins_with(MovieName, :movieName)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":userId", new AttributeValue(){ N = userId.ToString() } },
                    {":movieName", new AttributeValue(){ S = movieName } }
                }
            };                               

            var result = await _dynamoClient.QueryAsync(request);

            return result;
        }
        public async Task<PutItemResponse> AddMovieRankedItem(Movie movie)
        {
            var request = new PutItemRequest(TableName, new Dictionary<string, AttributeValue>()
            {
                {"UserId", new AttributeValue(){ N = movie.UserId.ToString() } },
                {"MovieName", new AttributeValue(){ S = movie.MovieName } },
                {"Description", new AttributeValue(){ S = movie.Description } },
                {"Actors", new AttributeValue(){ SS = movie.Actors } },
                {"Ranking", new AttributeValue(){ N = movie.Ranking.ToString() } },
                {"RankedDate", new AttributeValue(){ S = movie.RankedDate } }
            });

            var result = await _dynamoClient.PutItemAsync(request);

            return result;
    }
        public async Task<UpdateItemResponse> UpdateMovieRankedItem(Movie movie)
        {
            var request = new UpdateItemRequest(
                TableName, 
                new Dictionary<string, AttributeValue>()
                {
                    {"UserId", new AttributeValue(){ N = movie.UserId.ToString() } },
                    {"MovieName", new AttributeValue(){ S = movie.MovieName } }               
                },
                new Dictionary<string, AttributeValueUpdate>()
                {
                    { "Ranking", new AttributeValueUpdate()
                        {
                            Action = AttributeAction.PUT,
                            Value = new AttributeValue(){ N = movie.Ranking.ToString() }
                        }
                    }
                }          
              );

            var result = await _dynamoClient.UpdateItemAsync(request);

            return result;
        }

        public async Task<QueryResponse> GetMovieItemByIndexName(string movieName)
        {
            var request = new QueryRequest(TableName)
            {
                IndexName="MovieName-index",
                KeyConditionExpression = "MovieName=:movieName",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":movieName", new AttributeValue(){ S = movieName } }
                }
            };

            var result = await _dynamoClient.QueryAsync(request);

            return result;
        }
    }
}
