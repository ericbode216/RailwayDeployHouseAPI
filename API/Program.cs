

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;


var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddHealthChecks();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HouseDbContext>(o =>
    o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();


var app = builder.Build();


app.UseHealthChecks("/health");

// Configure the HTTP request pipeline.
//Im intentionally going to expose Swagger since this is just a portfolio application and I want people to see my API structure
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors(p => p.WithOrigins("http://localhost:3000")
        .AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.mapHouseEndpoints();
app.mapBidEndpoints();



app.Run();