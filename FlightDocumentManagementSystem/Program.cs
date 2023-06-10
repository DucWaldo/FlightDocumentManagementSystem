using FlightDocumentManagementSystem.Contexts;
using FlightDocumentManagementSystem.Middlewares;
using FlightDocumentManagementSystem.Repositories.Implementations;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert [token]",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    IConfiguration configuration = builder.Configuration;
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "")),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(Assignment.ADMIN));
    options.AddPolicy("StaffPolicy", policy => policy.RequireRole(Assignment.STAFF));
    options.AddPolicy("PilotPolicy", policy => policy.RequireRole(Assignment.PILOT));
    options.AddPolicy("CrewPolicy", policy => policy.RequireRole(Assignment.CREW));
    options.AddPolicy("AdminOrStaffPolicy", policy => policy.RequireRole(Assignment.ADMIN, Assignment.STAFF));
    options.AddPolicy("PilotOrCrewPolicy", policy => policy.RequireRole(Assignment.PILOT, Assignment.CREW));
    options.AddPolicy("AllPolicy", policy => policy.RequireRole(Assignment.ADMIN, Assignment.STAFF, Assignment.PILOT, Assignment.CREW));
});

builder.Services.AddScoped<IRoleRepository, RoleRepository>()
    .AddScoped<IAccountRepository, AccountRepository>()
    .AddScoped<IAuthRepository, AuthRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IGroupRepository, GroupRepository>()
    .AddScoped<IMemberRepository, MemberRepository>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<IPermissionRepository, PermissionRepository>()
    .AddScoped<IAirportRepository, AirportRepository>()
    .AddScoped<IAircraftRepository, AircraftRepository>()
    .AddScoped<IFlightRepository, FlightRepository>()
    .AddScoped<IScheduleRepository, ScheduleRepository>()
    .AddScoped<IDocumentRepository, DocumentRepository>()
    .AddScoped<ISignatureRepository, SignatureRepository>()
    .AddScoped<IDisplayRepository, DisplayRepository>()
    .AddScoped<IEmailLogRepository, EmailLogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
