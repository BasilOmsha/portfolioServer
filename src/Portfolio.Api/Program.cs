using Portfolio.Infrastructure.Extensions;
using Portfolio.Api.Middleware;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Add Infrastructure services (configuration)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Application services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRecaptchaService, RecaptchaService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.WithOrigins("http://localhost:5174", "http://localhost:3000", "https://basilomsha.vercel.app")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();