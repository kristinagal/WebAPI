using P103_ExternalApi.Dtos;

namespace P103_ExternalApi.Services
{
    public interface IAuthorsMapper
    {
        AuthorResult Map(AuthorApiResult o);
        IEnumerable<AuthorResult> Map(IEnumerable<AuthorApiResult> o);
        AuthorApiRequest Map(AuthorRequest req);
    }

    public class AuthorsMapper : IAuthorsMapper
    {
        public AuthorResult Map(AuthorApiResult o)
        {
            return new AuthorResult
            {
                Id = o.Id,
                Name = o.Name
            };
        }

        public IEnumerable<AuthorResult> Map(IEnumerable<AuthorApiResult> o)
        {
            return o.Select(Map);
        }

        public AuthorApiRequest Map(AuthorRequest req)
        {
            return new AuthorApiRequest
            {
                Name = req.Name
            };
        }
    }
}
