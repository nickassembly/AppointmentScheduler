using AppointmentScheduler.Models;
using AppointmentScheduler.Seeders;
using AppointmentScheduler.Services;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AppointmentScheduler
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllersWithViews();

            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddDistributedMemoryCache();
            services.AddSession(options => 
            {
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Home/AccessDenied");
            });

            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseSession();

            dbInitializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
