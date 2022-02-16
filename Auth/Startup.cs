using Auth.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Here I create the cookie.
            services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", config =>
            {
                //config.Cookie.HttpOnly = false;
                config.LoginPath = "/Account/Authenticate";
            });
            //I craete the claimBase authentification.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", config =>
                {
                    config.RequireAuthenticatedUser().RequireClaim(ClaimTypes.DateOfBirth);
                });

                options.AddPolicy("SuperAdmin", config =>
                {
                    config
                    .RequireRole("Admin")
                    .RequireClaim("CanDeleteUser")
                    .RequireClaim("CanUpdateUser")
                    .RequireClaim("CanCreateUser")
                    .RequireClaim("CanShowUsers");
                });


                options.AddPolicy("LittleAdmin", config =>
                {
                    config
                    .RequireRole("Admin")
                    .RequireClaim("CanShowUsers");
                });

                options.AddPolicy("Company", config =>
                {
                    config.RequireClaim("Company", "Mincrosogt");
                });

                options.AddPolicy("StartWithLetterPolicy", options =>
                {
                    options.Requirements.Add(new StartWithRequirement("M"));
                });
                options.AddPolicy("AgeRequirement", options =>
                {

                    options.Requirements.Add(new AgeRequirement(18));
                });
            });
            services.AddSingleton<IAuthorizationHandler, StartWithHandler>();
            services.AddSingleton<IAuthorizationHandler, AgeHandler>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}