using Microsoft.EntityFrameworkCore;
using PurchaseManagement.API.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration["ConnectionStrings:chaki"];
//ConnectionStrings:chaki
// Add services to the container.

builder.Services.AddControllers();

// Seetings


builder.Services.AddSqlServer<AccountContext>(connectionString, option =>
{
    option.EnableRetryOnFailure();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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
