using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Task_Board_API
{
    public class TaskBoardRestfulApiTests
    {
        private const string url = "https://taskboard-2.stanislavzlatan.repl.co/api";
        private RestClient client ;
        private RestRequest request;
        

        [SetUp]
        public void Setup()
        {
            client = new RestClient(url);
        }
        [Test]
        public void Test_GetAllTasks_FirstTasksName()
        {
            // Arrange
            request = new RestRequest(url);

            // Act
            var response = client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);
            var boards = JsonSerializer.Deserialize<List<Board>>(response.Content);

            // Assert
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(tasks.Count > 0);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            foreach (var board in boards)
            {
                var boardName = board.name;
                if (boardName == "Done")
                {
                    Assert.AreEqual("Project skeleton", tasks.First().title);
                    break;
                }
            }
        }
        [Test]
        public void Test_SearchTasksKeyword_Valid()
        {
            // Arrange
            request = new RestRequest(url + "/tasks/search/Home");

            // Act
            var response = client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);

            // Assert
            Assert.IsNotNull(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.AreEqual("Home page", tasks.First().title);
        }
        [Test]
        public void Test_SearchTasksInvalidKeyword()
        {
            // Arrange
            request = new RestRequest(url + "/tasks/search/mising1234567");
         
            // Act
            var response = client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);

            // Assert
           
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            Assert.IsTrue(tasks.Count == 0);
        }
        [Test]
        public void Test_Create_New_Task_Invalid_Data()
        { 
            //Arrange
            RestRequest request = new RestRequest($"{url}/tasks");
            //Act
            string title = string.Empty;
            string description = "Description";
            string name = "Open";

            var body = new
            {
                title,
                description,
                name
            };

            RestResponse response = client.Execute(request, Method.Post);

            //Assert

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Test]
        public void Test_Create_New_Task_Valid_Data()
        {
            RestRequest request = new RestRequest($"{url}/tasks");
            string title = "New Task" + DateTime.Now.Ticks;
            string description = "Description";
            string name = "Open";


            var body = new
            {
                title,
                description,
                name
            };
            request.AddJsonBody(body);
            RestResponse response = client.Execute(request, Method.Post);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            string keyword = title;

            request = new RestRequest($"{url}/tasks");
            response = client.Execute(request);

            List<TaskBoard> tasks = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);

            TaskBoard result = tasks.Last();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(keyword, result.title);
        }
    }
}