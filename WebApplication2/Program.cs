using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped(x => new SqliteConnection("Data Source = addresses.db"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Making the database
// Longest streetname is 55 chars long
// Longest cityname is 28 chars long

SqliteConnection m_dbConnection = new SqliteConnection("Data Source=addresses.db");
m_dbConnection.Open();


// Code for testing the database
AddressRepository repository = new AddressRepository(m_dbConnection);
repository.InsertAddress(new Address("Hooglandseweg-Zuid", "Amersfoort", "3813TC", 15));
repository.UpdateAddress(new Address("Hooglandseweg-Zuid", "Amersfoort", "3813TC", 16), 1);

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
