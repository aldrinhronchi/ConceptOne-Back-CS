using Coopersam_WebAPI_CS.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
NativeInjector.RegisterBuild(builder);

IServiceCollection services = builder.Services;
NativeInjector.RegisterServices(services);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.MapOpenApi();
//}

NativeInjector.ConfigureApp(app, app.Environment);

app.MapControllers();

app.Run();