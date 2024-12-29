using System.Text;
using System.Text.Json.Serialization;
using ClubApi.Application;
using ClubApi.Domain.Abstractions;
using ClubApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ClubApi.Presentation.Core;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Logging.AddConsole();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? builder.Configuration["Jwt:Issuer"],
    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ?? builder.Configuration["Jwt:Secret"]!))
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Personal"));
});

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"Club Access Control API",
                    Description = "An example to share an implementation of API in .NET 8.",
                    Contact = new OpenApiContact
                    {
                        Name = "Carlos Rafael Mendoza",
                        Email = "carlos.rafael.mbm@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Club Access Control API - License - MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter JWT token only",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }
);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

#region Security

app.UseHsts();

#endregion Security

#region Exception Middleware

app.UseMiddleware<ExceptionHandlingMiddleware>();

#endregion

app.UseSwagger();
app.UseSwaggerUI(c =>
            c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                $"ClubAC API - V1"));

app.UseHttpsRedirection();

app.MapControllers();

#region CORS Configuration

app.UseCors(config => config
  .AllowAnyHeader()
  .AllowAnyMethod()
  .AllowAnyHeader()
  .SetIsOriginAllowed(_ => true)
  .AllowCredentials()
);

#endregion

#region Authentication and Authorization

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Run Application

app.Run();

#endregion
