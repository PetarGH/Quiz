using Domain.Data;
using Application.IManagers;
using Microsoft.EntityFrameworkCore;
using Application.Managers;
using Domain.Entities;
using Application.Repositories;
using Application.IRepositories;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.IHelpers;
using Backend.Controllers;
using Backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


//Services (Dependencies)
//Users
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IJwtService, JwtService>();
//Categories
builder.Services.AddTransient<ICategoryManager, CategoryManager>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
//Quizes
builder.Services.AddTransient<IQuizManager, QuizManager>();
builder.Services.AddTransient<IQuizRepository, QuizRepository>();
//Questions
builder.Services.AddTransient<IQuestionManager, QuestionManager>();
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();
//Answers
builder.Services.AddTransient<IAnswerRepository, AnswerRepository>();
builder.Services.AddTransient<IAnswerManager, AnswerManager>();
builder.Services.AddSingleton<ChatHub>();

builder.Services.AddDbContext<Dbi477163Context>();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

builder.Services.AddCors(options =>
{
    var frontendURL = configuration.GetValue<string>("frontend_url");

    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


//lets me pass the data but its null.
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseWebSockets();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.UseAuthorization();
app.UseAuthentication();
app.UseCookiePolicy();

app.Run();
