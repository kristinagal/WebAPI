using P103_ExternalApi.Models;
using P103_ExternalApi.Services;

namespace P103_ExternalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<IBooksApiClient, BooksApiClient>();
            builder.Services.AddTransient<IBooksMapper, BooksMapper>();
            builder.Services.AddTransient<IAuthorsApiClient, AuthorsApiClient>();
            builder.Services.AddTransient<IAuthorsMapper, AuthorsMapper>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register HttpClient for BooksApiClient
            builder.Services.AddHttpClient<IBooksApiClient, BooksApiClient>((services, client) =>
            {
                var config = builder.Configuration.GetSection("BooksApi").Get<BooksApiConfig>();
                client.BaseAddress = new Uri(config.Url);
                client.DefaultRequestHeaders.Add("X-API-KEY", config.ApiKey);
            });

            // Register HttpClient for AuthorsApiClient
            builder.Services.AddHttpClient<IAuthorsApiClient, AuthorsApiClient>((services, client) =>
            {
                var config = builder.Configuration.GetSection("BooksApi").Get<BooksApiConfig>(); 
                client.BaseAddress = new Uri(config.Url);
                client.DefaultRequestHeaders.Add("X-API-KEY", config.ApiKey);
            });

            // Register BooksApiConfig
            //builder.Services.Configure<BooksApiConfig>(builder.Configuration.GetSection("BooksApi"));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
