//using Moq.Protected;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using P103_ExternalApi.Tests;
//using Microsoft.Extensions.Logging;
//using P103_ExternalApi.Dtos;
//using P103_ExternalApi.Services;
//using System.Net.Http.Json;

//namespace P103_ExternalApi_Tests
//{
//    public class AuthorsApiClientTests
//    {
//        //[Theory, AuthorData]              //Method_Scenario_Outcome
//        [Fact] // Test method naming: MethodName_Scenario_ExpectedOutcome
//        public async Task GetAuthor_Response200_ReturnsAuthorApiResult()
//        {
//            // Arrange
//            var authorId = 1;
//            var connectionId = "test-connection-id";

//            var expectedAuthor = new AuthorApiResult
//            {
//                Id = authorId,
//                Name = "Test Author"
//            };

//            // Create a mock HttpMessageHandler to intercept and respond to HttpClient requests
//            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
//            httpMessageHandlerMock
//                .Protected()
//                .Setup<Task<HttpResponseMessage>>(
//                    "SendAsync",
//                    ItExpr.Is<HttpRequestMessage>(req =>
//                        req.Method == HttpMethod.Get &&
//                        req.RequestUri!.AbsolutePath == $"/authors/{authorId}"),
//                    ItExpr.IsAny<CancellationToken>()
//                )
//                .ReturnsAsync(new HttpResponseMessage
//                {
//                    StatusCode = HttpStatusCode.OK,
//                    Content = JsonContent.Create(expectedAuthor)
//                });

//            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
//            {
//                BaseAddress = new Uri("https://example.com/")
//            };

//            var loggerMock = new Mock<ILogger<AuthorsApiClient>>();
//            var sut = new AuthorsApiClient(loggerMock.Object, httpClient);

//            // Act
//            var result = await sut.GetAuthor(connectionId, authorId);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(expectedAuthor.Id, result.Id);
//            Assert.Equal(expectedAuthor.Name, result.Name);

//            // Verify that the connectionId was added to the headers
//            httpMessageHandlerMock.Protected().Verify(
//                "SendAsync",
//                Times.Once(),
//                ItExpr.Is<HttpRequestMessage>(req =>
//                    req.Headers.Contains("connectionId") &&
//                    req.Headers.GetValues("connectionId").Contains(connectionId)),
//                ItExpr.IsAny<CancellationToken>()
//            );
//        }
//    }
//}
