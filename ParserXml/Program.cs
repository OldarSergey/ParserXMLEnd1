using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParserXml.Data;
using ParserXml.Model.EntitiesDbContext;
using ParserXml.Service;
using XMLParser;
using XMLParser.Service;

namespace ParserXml
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("ParserXmlContextConnection") ?? throw new InvalidOperationException("Connection string 'ParserXmlContextConnection' not found.");

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IXmlFileParser,XmlFileParser>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<ILoggingsService, LoggingService>();

            var configuration = builder.Configuration;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BloggingDatabase")));

            builder.Services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();

            var app = builder.Build();


            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            SeedData.EnsureSeedData(scope.ServiceProvider);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
                        app.UseAuthentication();;

            app.UseAuthorization();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}