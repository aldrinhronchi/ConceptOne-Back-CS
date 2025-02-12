using Coopersam_WebAPI_CS.Connections.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection.Metadata;
using System.Text;

namespace Coopersam_WebAPI_CS.Extensions
{
    public class NativeInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services

            //services.AddScoped<Interface, Service>();

            #endregion Services

            #region Other

            //ServiceLocator.IncluirServico(services.BuildServiceProvider());

            #endregion Other
        }

        public static void RegisterBuild(WebApplicationBuilder builder)
        {
            #region Context

            //builder.Services.AddDbContext<SotequiContext>(opt =>
            //{
            //    opt.UseSqlServer(builder.Configuration.GetConnectionString("SotequiDB")).EnableSensitiveDataLogging();
            //});
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
            //app.AddMiddlewares();
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
}