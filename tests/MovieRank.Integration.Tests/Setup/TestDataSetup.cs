using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieRank.Integration.Tests.Setup
{
    public class TestDataSetup
    {
        private readonly IAmazonDynamoDB _dynamo = new AmazonDynamoDBClient(new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:8000"
        });
        public async Task CreateTable()
        {
            var tableRequest = new CreateTableRequest()
            {
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    { new AttributeDefinition() { AttributeName = "UserId", AttributeType = "N"  } },
                    { new AttributeDefinition() { AttributeName = "MovieName", AttributeType = "S"  } }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    { new KeySchemaElement(){ AttributeName = "UserId", KeyType = KeyType.HASH } },
                    { new KeySchemaElement(){ AttributeName = "MovieName", KeyType = KeyType.RANGE } }
                },
                ProvisionedThroughput = new ProvisionedThroughput()
                {
                    ReadCapacityUnits = 1,
                    WriteCapacityUnits = 1
                },
                TableName = "MovieRank",
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>()
                {
                    new GlobalSecondaryIndex(){
                        IndexName = "MovieName-index",
                        KeySchema = new List<KeySchemaElement>()
                        {
                            new KeySchemaElement(){ AttributeName = "MovieName", KeyType = KeyType.HASH }
                        },
                        ProvisionedThroughput = new ProvisionedThroughput()
                        {
                            ReadCapacityUnits = 1,
                            WriteCapacityUnits = 1
                        },
                        Projection = new Projection()
                        {
                            ProjectionType = ProjectionType.ALL
                        }
                    }
                }
            };

            await _dynamo.CreateTableAsync(tableRequest);
            await WaitUntilCreateTable(tableRequest.TableName);
        }

        private async Task WaitUntilCreateTable(string tableName)
        {
            string status = string.Empty;

            do
            {
                Thread.Sleep(5000);

                try
                {
                    status = await GetTableStatus(tableName);
                }
                catch (ResourceNotFoundException)
                {
                }
            } while (status != "ACTIVE");
        }

        private async Task<string> GetTableStatus(string tableName)
        {
            var response = await _dynamo.DescribeTableAsync(tableName);

            return response.Table.TableStatus;
        }
    }

   
}
