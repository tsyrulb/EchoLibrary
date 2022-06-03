/*
using Microsoft.EntityFrameworkCore;
using Services;
using Repository;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EchoAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FeedbackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FeedbackContext") ?? throw new InvalidOperationException("Connection string 'FeedbackContext' not found.")));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<ContextData>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<MessageService>();


//builder.Services.AddScoped<ChatHub>();


builder.Services.AddSingleton<ChatHub>();

// for SignalIR
builder.Services.AddSignalR();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()

    

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidAudience = builder.Configuration["JWTParams:Audience"],

        ValidIssuer = builder.Configuration["JWTParams:Issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTPArams:SecretKey"]))
    };

});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
            // allow any origin



        });   
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
      app.UseSwaggerUI();

}

app.UseHttpsRedirection();


app.UseCors("ClientPermission");

app.MapControllers();
app.UseRouting();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
});

app.Run();

*/

using EchoAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Repository;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FeedbackContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FeedbackContext") ?? throw new InvalidOperationException("Connection string 'FeedbackContext' not found.")));

builder.Services.AddDbContextPool<MariaDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MariaDbContext"),
    new MariaDbServerVersion(new Version(8, 0, 11))));

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ContextData>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChatHub>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddSignalR();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()

    {

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidAudience = builder.Configuration["JWTParams:Audience"],

        ValidIssuer = builder.Configuration["JWTParams:Issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTPArams:SecretKey"]))
    };

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow All",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()

            .AllowAnyHeader();
            // allow any origin



        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermission", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseCors("ClientPermission");
app.UseCors("Allow All");




app.MapControllers();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");
});
app.Run();