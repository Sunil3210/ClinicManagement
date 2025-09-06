using BLL;
using ClinicManagement.Mapper;
using DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//DAL
builder.Services.AddScoped<IDoctorDAL, DoctorDAL>();
builder.Services.AddScoped<IDepartmentDAL, DepartmentDAL>();
builder.Services.AddScoped<IAdmissionDAL, AdmissionDAL>();

//BLL
builder.Services.AddScoped<IDepartmentBLL, DepartmentBLL>();
builder.Services.AddScoped<IDoctorBLL, DoctorBLL>();
builder.Services.AddScoped<IAdmissionBLL, AdmissionBLL>();
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
