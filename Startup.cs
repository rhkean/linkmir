using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using linkmir.DbModels;

namespace linkmir
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
            services.AddDbContext<LinkmirDbContext>(opt => opt.UseInMemoryDatabase("linkmir"));
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LinkmirDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //var context = app.ApplicationServices.GetService<LinkmirDbContext>();
                

                AddTestData(context);
            }

            // app.UseDefaultFiles();
            // app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddTestData(LinkmirDbContext context)
        {
            var entryOne = new LinkmirLinkDbItem("https://www.google.com");
            context.Links.Add(entryOne);

            var entryTwo = new LinkmirLinkDbItem("https://images.google.com");
            context.Links.Add(entryTwo);

            var entryThree = new LinkmirLinkDbItem("https://open.spotify.com");
            context.Links.Add(entryThree);

            context.SaveChanges();
        }
    }
}
