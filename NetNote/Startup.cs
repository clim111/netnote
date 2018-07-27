using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetNote.DAL;
//using NetNote.Middleware;
using NetNote.Model;
using NetNote.Repository;

namespace NetNote
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
            //sql server
            //var connection = "server=.;Database=Note;UID=sa;Pwd=123456;";
            //services.AddDbContext<NoteContext>(options => options.UseSqlServer(connection
            //    , b => b.UseRowNumberForPaging()
            //    ));
            //sqlite
            var connectionString = "Filename=netnote.db";
            services.AddDbContext<NoteContext>(options => options.UseSqlite(connectionString));
            services.AddIdentity<NoteUser, IdentityRole>()
                .AddEntityFrameworkStores<NoteContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(optons=>
            {
                optons.Password.RequireNonAlphanumeric = false;
                optons.Password.RequireUppercase = false;
                //optons.Cookies.ApplicationCookie.LoginPath = "/Account/Login";
            });

            services.AddMvc();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteTypeRepository, NoteTypeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitData(app.ApplicationServices);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            //app.UseBasicMiddleware(new BasicUser() { UserName = "admin", Password = "123456" });
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }

        private void InitData(IServiceProvider serviceProvider)
        {
            using (var serviceScope =serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<NoteContext>();
                db.Database.EnsureCreated();
                if (db.NoteTypes.Count()==0)
                {
                    var notetype = new List<NoteType> { new NoteType() { Name="日常记录"},
                        new NoteType() { Name="代码收藏"},
                        new NoteType() { Name="消费记录"},
                        new NoteType() { Name="网站收藏"},
                    };
                    db.NoteTypes.AddRange(notetype);
                    db.SaveChanges();
                }
            }
        }
    }
}
