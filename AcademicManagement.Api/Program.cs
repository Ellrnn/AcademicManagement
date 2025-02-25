using AcademicManagement.Api.Filters;
using AcademicManagement.Api.Infrastructure.DataAcess;
using AcademicManagement.Api.UseCases.Courses;
using AcademicManagement.Api.UseCases.Enrollments;
using AcademicManagement.Api.UseCases.Students;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddHealthChecks();

var connection_string = Environment.GetEnvironmentVariable("DATABASE_URL") ?? builder.Configuration.GetConnectionString("DataBasePg");

if (!string.IsNullOrEmpty(connection_string) && connection_string.StartsWith("postgres://"))
{
    var databaseUri = new Uri(connection_string.Replace("postgresql://", "postgres://"));
    var userInfo = databaseUri.UserInfo.Split(':');

    var npgBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port,
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? userInfo[1] : "",
        Database = databaseUri.AbsolutePath.TrimStart('/'),
        SslMode = SslMode.Prefer,
    };

    connection_string = npgBuilder.ConnectionString;
}

builder.Services.AddDbContext<DataBaseContext>(options => options.UseNpgsql(connection_string));

builder.Services.AddScoped<RegisterStudentsUseCase>();
builder.Services.AddScoped<UpdateStudentUseCase>();
builder.Services.AddScoped<GetStudentsUseCase>();
builder.Services.AddScoped<DeleteStudentUseCase>();

builder.Services.AddScoped<CreateCourseUseCase>();
builder.Services.AddScoped<GetCoursesUseCase>();
builder.Services.AddScoped<UpdateCourseUseCase>();
builder.Services.AddScoped<DeleteCourseUseCase>();
builder.Services.AddScoped<GetCourseStudentsUseCase>();

builder.Services.AddScoped<EnrollStudentInCoursesUseCase>();
builder.Services.AddScoped<GetEnrolledStudentsUseCase>();
builder.Services.AddScoped<DeleteEnrollmentUseCase>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));

var app = builder.Build();

app.UseHealthChecks("/health");

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
