using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using backend.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using backend.Services;
using Microsoft.OpenApi.Models;
using backend.Helpers;
using backend.Helpers.Interfaces;
using backend.Services.Interfaces;
using Twilio;
using backend.Managers;

namespace backend
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
            services.AddTransient<ISMTP, SMTP>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthorizeAttribute, AuthorizeAttribute>();
            services.AddTransient<IFriendService, FriendService>();
            services.AddTransient<IInviteService, InviteService>();
            services.AddTransient<ITestingService, TestingService>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Friendes's Tree", Version = "v1" });
            });

            //Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("VueCorsPolicy", builder =>
                {
                    builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("https://localhost:5001");
                });
            });

            //JSON Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.IsEssential = true;
            });

            // Setup SMS settings
            TwilioClient.Init(Configuration["Twilio:AuthToken"], Configuration["Twilio:AccountSID"]);
            services.Configure<TwilioVerifySettings>(Configuration.GetSection("Twilio"));

            services.AddSpaStaticFiles(configuration: options => { options.RootPath = "wwwroot"; });
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("VueCorsPolicy");

            dbContext.Database.EnsureCreated();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Friendes's Tree WEB API v1");
            });

            app.UseSpaStaticFiles();
            app.UseSpa(configuration: builder =>
            {
                if (env.IsDevelopment())
                {
                    builder.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }
            });
        }
    }
}
