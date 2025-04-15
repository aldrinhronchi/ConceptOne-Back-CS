using TMODELOBASET_WebAPI_CS.Connections.Configurations;
using TMODELOBASET_WebAPI_CS.Connections.Database;
using TMODELOBASET_WebAPI_CS.Extensions.Middleware;
using TMODELOBASET_WebAPI_CS.Services.Core;
using TMODELOBASET_WebAPI_CS.Services.Core.Interfaces;
using TMODELOBASET_WebAPI_CS.Services.Usuarios;
using TMODELOBASET_WebAPI_CS.Services.Usuarios.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace TMODELOBASET_WebAPI_CS.Extensions
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            //services.AddScoped<Interface, Service>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IPermissoesService, PermissoesService>();
            services.AddScoped<ICoreService, CoreService>();
            #endregion Services

            #region Other

            ServiceLocator.IncluirServico(services.BuildServiceProvider());

            #endregion Other
        }

        public static void RegisterBuild(WebApplicationBuilder builder)
        {
            #region Context

            builder.Services.AddDbContext<TMODELOBASETContext>(opt =>
            {
                opt.UseMySql(builder.Configuration.GetConnectionString("TMODELOBASETConnection"), new MySqlServerVersion(new Version(8, 0, 23))).EnableSensitiveDataLogging();
            });
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            #endregion Context

            #region Swagger

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerConfiguration();

            #endregion Swagger

            #region JWT

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            #endregion JWT

            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "Origins",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:4200",
                                                          "http://localhost",
                                                          "https://localhost:4200",
                                                          "https://localhost");
                                      policy.AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .AllowAnyOrigin()
                                            .SetIsOriginAllowedToAllowWildcardSubdomains();
                                  });
            });

            #endregion CORS
        }

        public static void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.AddMiddlewares();
            app.UseCors("Origins");

            #region Files

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
                RequestPath = "/Files"
            });

            #endregion Files

            app.UseSwaggerConfiguration();

            #region Auth

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion Auth

            //Boolean FlagEmail = false;
            //if (FlagEmail)
            //{
            //    SendEmailWorker EmailWorker = new SendEmailWorker();
            //    EmailWorker.Initialize();
            //}
        }
    }
    public static class MiddlewareRegistrationExtension
    {
        public static void AddMiddlewares(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}