using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.Integration.Tests.Setup
{
    class TestContext : IAsyncLifetime
    {
        private DockerClient _dockerClient;
        private const string ContainerImageUrl = "amazon/dynamo-local";
        private string _containerId;

        public TestContext()
        {
            _dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
        }     

        public async Task InitializeAsync()
        {
            await PullImage();
            await StartContainer();

            await new TestDataSetup().CreateTable();
        }

        public async Task PullImage()
        {
            await _dockerClient.Images.CreateImageAsync(new ImagesCreateParameters()
            {
                FromImage = ContainerImageUrl,
                Repo = "latest"
            }, new AuthConfig(),
            new Progress<JSONMessage>());
        }

        public async Task StartContainer()
        {
            var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = ContainerImageUrl,
                ExposedPorts = new Dictionary<string, EmptyStruct>()
                {
                    { "8000", default(EmptyStruct) }
                },
                HostConfig = new HostConfig()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "8000", new List<PortBinding>{ new PortBinding{ HostPort = "8000"} } }
                    },
                    PublishAllPorts = true
                }                
            });

            _containerId = response.ID;

            await _dockerClient.Containers.StartContainerAsync(_containerId, null);
        }

        public async Task DisposeAsync()
        {
            if (_containerId != null)
            {
                await _dockerClient.Containers.KillContainerAsync(_containerId, new ContainerKillParameters());
            }
        }
    }
}
