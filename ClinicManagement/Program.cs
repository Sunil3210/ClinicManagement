using BLL;
using BLL.Infrastructure;
using ClinicManagement.Extension;
using ClinicManagement.Filters;
using ClinicManagement.Mapper;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JwtTokenFields"));
//DAL
builder.Services.AddScoped<IDepartmentDAL, DepartmentDAL>();
builder.Services.AddScoped<IDoctorDAL, DoctorDAL>();
builder.Services.AddScoped<IAdmissionDAL, AdmissionDAL>();
builder.Services.AddScoped<IStaffDAL, StaffDAL>();
builder.Services.AddScoped<IProfileDAL, ProfileDAL>();

//BLL
builder.Services.AddScoped<IDepartmentBLL, DepartmentBLL>();
builder.Services.AddScoped<IAdmissionBLL, AdmissionBLL>();
builder.Services.AddScoped<IDoctorBLL, DoctorBLL>();
builder.Services.AddScoped<IStaffBLL, StaffBLL>();
builder.Services.AddScoped<ITokenBLL, TokenBLL>();
builder.Services.AddScoped<IProfileBLL, ProfileBLL>();
builder.Services.AddScoped<IUserClaimService, UserClaimService>();
builder.Services.AddHttpContextAccessor();
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtTokenFields:AccessTokenKey"]);
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true,
ValidIssuer = builder.Configuration["JwtTokenFields:Issuer"],
ValidateAudience = false,
ValidateIssuerSigningKey = true,
IssuerSigningKey = new SymmetricSecurityKey(key),
ValidateLifetime = false,
};
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOrManager", policy =>
//        policy.RequireRole("Admin", "Manager")); //
//});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Bearer support to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
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
            new string[] {}
        }
    });
});
var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

app.Run();
