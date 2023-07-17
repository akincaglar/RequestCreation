using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RequestCreation.Context;
using RequestCreation.Models;

namespace RequestCreation
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
            services.AddControllersWithViews();

            string connectionString = "Server=DESKTOP-SNQMH12;Database=Requests;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=true;";
            services.AddDbContext<RequestDbContext>(options =>
                options.UseSqlServer(connectionString));


            services.AddDistributedMemoryCache(); // Oturum verilerini bellekte saklamak için dağıtılmış bellek önbelleğini kullanın

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturumun süresi 30 dakika olarak ayarlanmıştır
                options.Cookie.HttpOnly = true; // Oturum çerezi yalnızca HTTP üzerinden iletilir
                options.Cookie.IsEssential = true; // Oturum çerezi zorunlu olarak kabul edilir
            });





        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseSession(); // Oturum kullanımını etkinleştir
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}

