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
using FluentValidation.AspNetCore;
using BLL.validators;
using FluentValidation;
using BLL.DTOs;
using WebPWrecover.Services;
using DAL.Services;

namespace ToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IValidator<NoteDTO>, NoteDTOValidator>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register services and repositories
            
            builder.Services.AddScoped<DataAccessFactory>();
            builder.Services.AddScoped<NoteService>();
            builder.Services.AddScoped<CategoryService>();

            //register session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(300);
            });
            builder.Services.AddHttpContextAccessor();

            //add database connetction
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));


            //Register Add Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();

            //Configure path for Identity
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            //Add EmailSender
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);


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

            //cofigure middlewares
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Note}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
