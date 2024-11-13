using Microsoft.EntityFrameworkCore;
using P099_File.Repositories;
using P099_File.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddTransient<IDbChangeTracker, DbChangeTracker>();
builder.Services.AddTransient<IChangeRecordMapper, ChangeRecordMapper>();
builder.Services.AddTransient<IChangeRecordRepository, ChangeRecordRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
