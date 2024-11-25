//using AutoFixture;
//using AutoFixture.Xunit2;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using P103_ExternalApi.Controllers;
//using P103_ExternalApi.Dtos;
//using P103_ExternalApi.Services;
//using P103_ExternalApi_Tests;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using System.Text.Json;

//namespace P103_ExternalApi.Tests.Controllers
//{
//    public class BooksApiControllerTests
//    {
//        private readonly IFixture _fixture;
//        private readonly Mock<IBooksApiClient> _mockBooksApiClient;
//        private readonly Mock<IBooksMapper> _mockMapper;
//        private readonly BooksApiController _controller;

//        public BooksApiControllerTests()
//        {
//            _fixture = new Fixture();
//            _fixture.Customizations.Add(new BookSpecimenBuilder()); // Add custom specimen builder for Book data
//            _mockBooksApiClient = _fixture.Freeze<Mock<IBooksApiClient>>();
//            _mockMapper = _fixture.Freeze<Mock<IBooksMapper>>();
//            _controller = new BooksApiController(_mockBooksApiClient.Object, _mockMapper.Object);
//        }

//        [Fact]
//        public async Task GetBook_WhenBookExists_ReturnsOk()
//        {
//            // Arrange
//            var connectionId = _fixture.Create<string>();
//            var bookApiResult = _fixture.Create<BookApiResult>();
//            var bookResult = _fixture.Create<BookResult>();

//            _mockBooksApiClient.Setup(x => x.GetBook(It.IsAny<string>(), bookApiResult.Id)).ReturnsAsync(bookApiResult);
//            _mockMapper.Setup(x => x.Map(bookApiResult)).Returns(bookResult);

//            // Act
//            var result = await _controller.GetBook(connectionId, bookApiResult.Id);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
//            Assert.Equal(bookResult, okResult.Value);
//        }

//        [Fact]
//        public async Task GetBooks_WithListOfBooks_ReturnsOkResult()
//        {
//            // Arrange
//            var connectionId = _fixture.Create<string>();
//            var bookList = _fixture.Create<List<BookApiResult>>();
//            var mappedBooks = _fixture.Create<List<BookResult>>();

//            _mockBooksApiClient.Setup(x => x.GetBooks(connectionId)).ReturnsAsync(bookList);
//            _mockMapper.Setup(x => x.Map(bookList)).Returns(mappedBooks);

//            // Act
//            var result = await _controller.GetBooks(connectionId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
//            Assert.Equal(mappedBooks, okResult.Value);
//        }


//        [Fact]
//        public async Task CreateBook_ReturnsCreatedResult()
//        {
//            // Arrange
//            var connectionId = "test-connection-id";
//            var bookRequest = new BookRequest { Title = "New Book", Authors = new List<int> { 1 }, Genres = new List<string> { "Fiction" }, Year = 2022 };
//            var bookApiRequest = new BookApiRequest { Title = "New Book", Authors = new List<int> { 1 }, Genres = new List<string> { "Fiction" }, Year = 2022 };
//            var newBookId = 1;

//            _mockMapper.Setup(x => x.Map(bookRequest)).Returns(bookApiRequest);
//            _mockBooksApiClient.Setup(x => x.CreateBook(connectionId, bookApiRequest)).ReturnsAsync(newBookId);

//            // Act
//            var result = await _controller.CreateBook(connectionId, bookRequest);

//            // Assert
//            var createdResult = Assert.IsType<CreatedResult>(result);
//            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

//            // Serialize the expected and actual values to JSON and compare
//            var expectedValue = JsonSerializer.Serialize(new { id = newBookId });
//            var actualValue = JsonSerializer.Serialize(createdResult.Value);

//            Assert.Equal(expectedValue, actualValue);
//        }

//        //[Fact]
//        //public async Task CreateBook_ReturnsCreatedResult()
//        //{
//        //    // Arrange
//        //    var connectionId = "test-connection-id";
//        //    var bookRequest = new BookRequest { Title = "New Book", Authors = new List<int> { 1 }, Genres = new List<string> { "Fiction" }, Year = 2022 };
//        //    var bookApiRequest = new BookApiRequest { Title = "New Book", Authors = new List<int> { 1 }, Genres = new List<string> { "Fiction" }, Year = 2022 };
//        //    var newBookId = 1;

//        //    _mockMapper.Setup(x => x.Map(bookRequest)).Returns(bookApiRequest);
//        //    _mockBooksApiClient.Setup(x => x.CreateBook(connectionId, bookApiRequest)).ReturnsAsync(newBookId);

//        //    // Act
//        //    var result = await _controller.CreateBook(connectionId, bookRequest);

//        //    // Assert
//        //    var createdResult = Assert.IsType<CreatedResult>(result);
//        //    Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

//        //    var actualValue = Assert.IsType<Dictionary<string, int>>(createdResult.Value);
//        //    Assert.Equal(newBookId, actualValue["id"]);
//        //}

//        [Fact]
//        public async Task UpdateBook_WhenBookExists_ReturnsNoContentResult()
//        {
//            // Arrange
//            var connectionId = _fixture.Create<string>();
//            var bookId = _fixture.Create<int>();
//            var bookRequest = _fixture.Create<BookRequest>();
//            var bookApiRequest = _fixture.Create<BookApiRequest>();

//            _mockBooksApiClient.Setup(x => x.GetBook(It.IsAny<string>(), bookId)).ReturnsAsync(_fixture.Create<BookApiResult>());
//            _mockMapper.Setup(x => x.Map(bookRequest)).Returns(bookApiRequest);
//            _mockBooksApiClient.Setup(x => x.UpdateBook(connectionId, bookId, bookApiRequest)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.UpdateBook(connectionId, bookId, bookRequest);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Fact]
//        public async Task UpdateBook_WhenBookDoesNotExist_ReturnsNotFound()
//        {
//            // Arrange
//            var connectionId = _fixture.Create<string>();
//            var bookId = _fixture.Create<int>();
//            var bookRequest = _fixture.Create<BookRequest>();

//            _mockBooksApiClient.Setup(x => x.GetBook(connectionId, bookId)).ReturnsAsync((BookApiResult)null);

//            // Act
//            var result = await _controller.UpdateBook(connectionId, bookId, bookRequest);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public async Task DeleteBook_WhenBookExists_ReturnsNoContentResult()
//        {
//            // Arrange
//            var connectionId = _fixture.Create<string>();
//            var bookId = _fixture.Create<int>();

//            _mockBooksApiClient.Setup(x => x.GetBook(connectionId, bookId)).ReturnsAsync(_fixture.Create<BookApiResult>());
//            _mockBooksApiClient.Setup(x => x.DeleteBook(connectionId, bookId)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _controller.DeleteBook(connectionId, bookId);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Fact]
//        public async Task DeleteBook_WhenBookDoesNotExist_ReturnsNotFound()
//        {
//            // Arrange
//            var connectionId = _fixture.Create<string>();
//            var bookId = _fixture.Create<int>();

//            _mockBooksApiClient.Setup(x => x.GetBook(connectionId, bookId)).ReturnsAsync((BookApiResult)null);

//            // Act
//            var result = await _controller.DeleteBook(connectionId, bookId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }
//    }
//}
