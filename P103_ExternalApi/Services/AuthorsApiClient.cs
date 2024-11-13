using P103_ExternalApi.Dtos;

namespace P103_ExternalApi.Services
{
    public interface IAuthorsApiClient
    {
        Task<long?> CreateAuthor(string connectionId, AuthorApiRequest req);
        Task DeleteAuthor(string connectionId, int id);
        Task<AuthorApiResult?> GetAuthor(string connectionId, int authorId);
        Task<IEnumerable<AuthorApiResult>?> GetAuthors(string connectionId);
        Task UpdateAuthor(string connectionId, int authorId, AuthorApiRequest req);
    }

    public class AuthorsApiClient : IAuthorsApiClient
    {
        private readonly ILogger<AuthorsApiClient> _logger;
        private readonly HttpClient _client;

        public AuthorsApiClient(ILogger<AuthorsApiClient> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IEnumerable<AuthorApiResult>?> GetAuthors(string connectionId)
        {
            _client.DefaultRequestHeaders.Add("connectionId", connectionId);
            var response = await _client.GetFromJsonAsync<IEnumerable<AuthorApiResult>>("authors");
            return response;
        }

        public async Task<AuthorApiResult?> GetAuthor(string connectionId, int authorId)
        {
            _client.DefaultRequestHeaders.Add("connectionId", connectionId);
            var response = await _client.GetFromJsonAsync<AuthorApiResult>($"authors/{authorId}");
            return response;
        }

        public async Task<long?> CreateAuthor(string connectionId, AuthorApiRequest req)
        {
            _client.DefaultRequestHeaders.Add("connectionId", connectionId);
            var response = await _client.PostAsJsonAsync("authors", req);
            if (response.IsSuccessStatusCode)
            {
                AuthorApiResult? content = await response.Content.ReadFromJsonAsync<AuthorApiResult>();
                return content!.Id;
            }
            return null;
        }

        public async Task UpdateAuthor(string connectionId, int authorId, AuthorApiRequest req)
        {
            _client.DefaultRequestHeaders.Add("connectionId", connectionId);
            await _client.PutAsJsonAsync($"authors/{authorId}", req);
        }

        public Task DeleteAuthor(string connectionId, int id)
        {
            _client.DefaultRequestHeaders.Add("connectionId", connectionId);
            return _client.DeleteAsync($"authors/{id}");
        }
    }
}
