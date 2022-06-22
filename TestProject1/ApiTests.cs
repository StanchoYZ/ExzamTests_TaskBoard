using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace TaskBoardAPITests
{
    public class ApiTests
    {
        private const string url = "https://taskboard-2.stanislavzlatan.repl.co/api";
        private RestClient client;
        

        [SetUp]
        public void Setup()
        {
            {
                client = new RestClient();
            }

            


        }
        [Test]
        public void Test_DoneTitleContent()
        {
            RestRequest request = new RestRequest($"{url}/boards");
            RestResponse response = client.Execute(request, Method.Get);

            List<TasksBoard> tasks = JsonSerializer.Deserialize<List<TasksBoard>>(response.Content);

            //TasksBoard first = tasks.First();

            //string name = first.board.name;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Done", request);
            Assert.AreEqual("Project skeleton", request);
        }
    } 
}