//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using P103_ExternalApi.Controllers;
//using P103_ExternalApi.Dtos;
//using P103_ExternalApi.Services;
//using Xunit;

//namespace P103_ExternalApi.Tests
//{
//    public class AuthorsApiControllerTests
//    {
//        private readonly Mock<IAuthorsApiClient> _authorsApiClientMock;
//        private readonly Mock<IAuthorsMapper> _authorsMapperMock;
//        private readonly AuthorsApiController _sut;

//        public AuthorsApiControllerTests()
//        {
//            _authorsApiClientMock = new Mock<IAuthorsApiClient>();
//            _authorsMapperMock = new Mock<IAuthorsMapper>();
//            _sut = new AuthorsApiController(_authorsApiClientMock.Object, _authorsMapperMock.Object);
//        }

//        [Fact]
//        public async Task GetAuthors_WithListOfAuthors_ReturnsOkResult()
//        {
//            // Arrange
//            var authorsApiResult = new List<AuthorApiResult> { new AuthorApiResult { Id = 1, Name = "Author1" } };
//            var authorsResult = new List<AuthorResult> { new AuthorResult { Id = 1, Name = "Author1" } };

//            _authorsApiClientMock.Setup(x => x.GetAuthors(It.IsAny<string>())).ReturnsAsync(authorsApiResult);
//            _authorsMapperMock.Setup(x => x.Map(It.IsAny<IEnumerable<AuthorApiResult>>())).Returns(authorsResult);

//            // Act
//            var result = await _sut.GetAuthors("test-connection-id");

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);
//            Assert.Equal(authorsResult, okResult.Value);
//        }

//        [Fact]
//        public async Task GetAuthor_WithAuthor_ReturnsOkResult()
//        {
//            // Arrange
//            var authorApiResult = new AuthorApiResult { Id = 1, Name = "Author1" };
//            var authorResult = new AuthorResult { Id = 1, Name = "Author1" };

//            _authorsApiClientMock.Setup(x => x.GetAuthor(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(authorApiResult);
//            _authorsMapperMock.Setup(x => x.Map(It.IsAny<AuthorApiResult>())).Returns(authorResult);

//            // Act
//            var result = await _sut.GetAuthor("test-connection-id", 1);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);
//            Assert.Equal(authorResult, okResult.Value);
//        }

//        [Fact]
//        public async Task GetAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
//        {
//            // Arrange
//            _authorsApiClientMock.Setup(x => x.GetAuthor(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((AuthorApiResult)null);

//            // Act
//            var result = await _sut.GetAuthor("test-connection-id", 1);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public async Task CreateAuthor_ReturnsCreatedAtActionResult()
//        {
//            // Arrange
//            var authorRequest = new AuthorRequest { Name = "Author1" };
//            var authorApiRequest = new AuthorApiRequest { Name = "Author1" };

//            _authorsMapperMock.Setup(x => x.Map(It.IsAny<AuthorRequest>())).Returns(authorApiRequest);
//            _authorsApiClientMock.Setup(x => x.CreateAuthor(It.IsAny<string>(), authorApiRequest)).ReturnsAsync(1);

//            // Act
//            var result = await _sut.CreateAuthor("test-connection-id", authorRequest);

//            // Assert
//            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
//            Assert.Equal(201, createdAtActionResult.StatusCode);
//            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
//        }

//        [Fact]
//        public async Task UpdateAuthor_WhenUpdateIsSuccessful_ReturnsNoContent()
//        {
//            // Arrange
//            var authorRequest = new AuthorRequest { Name = "Updated Author" };
//            var authorApiRequest = new AuthorApiRequest { Name = "Updated Author" };
//            var existingAuthor = new AuthorApiResult { Id = 1, Name = "Original Author" };

//            _authorsApiClientMock.Setup(x => x.GetAuthor(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(existingAuthor);
//            _authorsMapperMock.Setup(x => x.Map(It.IsAny<AuthorRequest>())).Returns(authorApiRequest);

//            // Act
//            var result = await _sut.UpdateAuthor("test-connection-id", 1, authorRequest);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Fact]
//        public async Task DeleteAuthor_WhenAuthorIsDeleted_ReturnsNoContent()
//        {
//            // Arrange
//            var existingAuthor = new AuthorApiResult { Id = 1, Name = "Author1" };

//            _authorsApiClientMock.Setup(x => x.GetAuthor(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(existingAuthor);

//            // Act
//            var result = await _sut.DeleteAuthor("test-connection-id", 1);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Fact]
//        public async Task DeleteAuthor_WhenAuthorDoesNotExist_ReturnsNotFound()
//        {
//            // Arrange
//            _authorsApiClientMock.Setup(x => x.GetAuthor(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((AuthorApiResult)null);

//            // Act
//            var result = await _sut.DeleteAuthor("test-connection-id", 1);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Theory, AuthorData]
//        public async Task GetAuthor_WhenAuthorExists_ReturnsOk(AuthorApiResult authorApiResult, AuthorResult authorResult)
//        {
//            // Arrange
//            _authorsApiClientMock.Setup(x => x.GetAuthor(It.IsAny<string>(), It.IsAny<int>()))
//                                 .ReturnsAsync(authorApiResult);
//            _authorsMapperMock.Setup(x => x.Map(authorApiResult)).Returns(authorResult);

//            // Act
//            var result = await _sut.GetAuthor("test-connection-id", authorApiResult.Id);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(200, okResult.StatusCode);
//            Assert.Equal(authorResult, okResult.Value);
//        }
//    }
//}
