using System.Reflection;
using Application;
using Application.Handlers;
using Application.Handlers.Comment;
using Application.Handlers.List;
using Application.Handlers.Board;
using Application.Handlers.StarredBoard;
using Application.Services;
using Application.Services.Authorization;
using Application.Services.Comment;
using Application.Services.InviteMembers;
using Application.Services.List;
using Application.Services.StarredBoard;
using Application.Services.Tasks;
using Application.Services.Token;
using Application.Services.User;
using Application.Services.Utility;
using Application.Services.WorkspaceActivity;
using Application.Services.Workspace;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddControllers().AddApplicationPart(Assembly.Load("UI"));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Configuration
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "../Infrastructure/infrastructureSettings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "../Application/applicationSettings.json"), optional: false, reloadOnChange: true);

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            )
        };
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
    //[Authorize(Policy = "AdminOnly")]
});
builder.Services.AddScoped<UserContext>();
builder.Services.AddHttpContextAccessor();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<IStarredBoardRepository, StarredBoardRepository>();
builder.Services.AddScoped<IWorkspaceActivityRepository, WorkspaceActivityRepository>();
builder.Services.AddScoped<IInviteRepository, InviteRepository>();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IWorkspaceActivityService, WorkspaceActivityService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IStarredBoardService, StarredBoardService>();
builder.Services.AddScoped<IBoardService, BoardService>();


//DeleteHandlers
builder.Services.AddScoped<ITaskDeleteHandler, TaskDeleteHandler>();
builder.Services.AddScoped<IListDeleteHandler, ListDeleteHandler>();
builder.Services.AddScoped<ICommentDeleteHandler, CommentDeleteHandler>();
builder.Services.AddScoped<IBoardDeleteHandler, BoardDeleteHandler>();
builder.Services.AddScoped<IStarredBoardDeleteHandler, StarredBoardDeleteHandler>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var userContext = context.RequestServices.GetRequiredService<UserContext>();
    var httpContextAccessor = context.RequestServices.GetRequiredService<IHttpContextAccessor>();

    var httpContext = httpContextAccessor.HttpContext;
    if (httpContext != null && httpContext.Request.Headers.ContainsKey("Authorization"))
    {
        var authHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            // Decode JWT token and extract claims here (you can use a library like System.IdentityModel.Tokens.Jwt)
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assuming your JWT contains these claims
            userContext.Id = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            userContext.Name = jwtToken.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            userContext.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            userContext.Role = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
        }
    }

    await next.Invoke();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();