using BLL.Services;
using DAL;
using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using DAL.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped< IRepository<User, int>, UserRepository>();
            builder.Services.AddScoped< IRepository<Category, int>, CategoryRepository>();
            builder.Services.AddScoped<IRepository<Note, int>, NoteRepository>();
            builder.Services.AddScoped<DataAccessFactory>();
            builder.Services.AddScoped<NoteService>();
            builder.Services.AddScoped<CategoryService>();
            //builder.Services.AddScoped<UserService>();


            //add database connetction
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Note}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
