using backend.Data;
using backend.DTOs.Invite.Input;
using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using backend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using backend.Mappers;
using backend.Mappers.Background;
using backend.Mappers.Board;
using backend.Mappers.Invite;
using backend.Mappers.Member;
using backend.Mappers.StarredBoard;
using backend.Mappers.TaskMember;
using backend.Mappers.User;
using backend.Mappers.Workspace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

//Implements JWT into swagger
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

//Accesses appsettings.json "DefaultConnection" to find the database connection string
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        
        //Kufizime ne krijimin e nje user te ri
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true; //!@#$%^&*()
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDBContext>();

//Adding authentication to the application using .AddAuthentication, and specifying the authentication as JWTBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //Adding the JwtBearer authentication to the authentication services
    .AddJwtBearer(options =>
        {
            //Configuring how the token should be validated
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

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IBackgroundRepository, BackgroundRepository>();
builder.Services.AddScoped<IStarredBoardRepository, StarredBoardRepository>();
builder.Services.AddScoped<IInviteRepository, InviteRepository>();
builder.Services.AddScoped<ITaskMemberRepository, TaskMemberRepository>();
builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();
// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Adjust origin as needed
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

//AutoMappers
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(BoardProfile).Assembly);
builder.Services.AddAutoMapper(typeof(WorkspaceProfile).Assembly);
builder.Services.AddAutoMapper(typeof(BackgroundProfile).Assembly);
builder.Services.AddAutoMapper(typeof(MemberProfile).Assembly);
builder.Services.AddAutoMapper(typeof(StarredBoardProfile).Assembly);
builder.Services.AddAutoMapper(typeof(TaskMemberProfile));

builder.Services.AddAutoMapper(typeof(InviteProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy globally
app.UseCors("AllowReactApp");

//Tell the application to use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();