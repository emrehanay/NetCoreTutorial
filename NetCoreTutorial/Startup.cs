using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetCoreTutorial.Helpers;
using NetCoreTutorial.Middleware;
using NetCoreTutorial.Repository;
using NetCoreTutorial.Service;
using Newtonsoft.Json;

namespace NetCoreTutorial
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
            });
            services.AddCors();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddControllers();

            services.AddScoped<DbContext, MainContext>();
            services.AddDbContext<MainContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IJwtHelper, JwtHelper>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(GenericBaseEntityRepository<>));
            services.AddScoped(typeof(GenericEntityRepository<>));

            services.AddMvc();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSettings> appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var logger = LogManager.GetLogger(typeof(Startup));

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    var message = "Unknown Error";
                    var exception = exceptionHandlerPathFeature?.Error;
                    logger.Error(exception);
                    if (exception is AppException)
                    {
                        message = exception.Message;
                    }

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        message = message
                    }));
                });
            });
            app.UseHsts();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<TimeInterceptorMiddleware>();
            app.UseMiddleware<JwtMiddleware>();


            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            LogLog.InternalDebugging = true;


            if (!env.IsDevelopment())
            {
                app.UseStaticFiles();

                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), appSettings.Value.UploadDirectory)
                    ),
                    RequestPath = new PathString($"/{appSettings.Value.UploadDirectory}")
                });
            }
        }
    }
}