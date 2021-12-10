using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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
    public class MovieRankRepository: IMovieRankGateway
    {
        private readonly DynamoDBContext context;
        public MovieRankRepository(IAmazonDynamoDB client)
        {
            context = new DynamoDBContext(client);
        }

        public async Task<IEnumerable<Movie>> GetAllMovieItems()
        {
            var result = await context.ScanAsync<Movie>(new List<ScanCondition>()).GetRemainingAsync();

            return result;
        }

        public async Task<Movie> GetMovieItem(int userId, string movieName)
        {
            var result = await context.LoadAsync<Movie>(userId, movieName);

            return result;
        }

        public async Task<IEnumerable<Movie>> GetUserRankedItems(int userId, string movieName)
        {
            var dynamoConfig = new DynamoDBOperationConfig()
            {
                QueryFilter = new List<ScanCondition>()
                {
                    new ScanCondition("MovieName", ScanOperator.BeginsWith, movieName)
                }
            };           

            var result = await context.QueryAsync<Movie>(userId, dynamoConfig).GetRemainingAsync();

            return result;
        }
        public async Task AddMovieRankedItem(Movie movie)
        {
            await context.SaveAsync(movie);
        }
        public async Task UpdateMovieRankedItem(Movie movie)
        {
            await context.SaveAsync(movie);
        }

        public async Task<IEnumerable<Movie>> GetMovieItemByIndexName(string movieName)
        {
            var dynamoConfig = new DynamoDBOperationConfig()
            {
                IndexName="MovieName-index"
            };

            var result = await context.QueryAsync<Movie>(movieName, dynamoConfig).GetRemainingAsync();

            return result;
        }
    }
}
