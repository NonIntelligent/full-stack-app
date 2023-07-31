using M70Service.Areas.Identity;
using M70Service.Data;
using M70Service.Data.Database;
using M70Service.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;

// Automatically uses secrets when in "Development"
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to environment
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add custom database context and name of migration code to execute using Reflection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, 
    sso => sso.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(o => { o.DetailedErrors = true; });
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<OperationService>();
builder.Services.AddSyncfusionBlazor();



if (builder.Environment.IsDevelopment()) {
    //var signalConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
    //builder.Services.AddSignalR().AddAzureSignalR(signalConnectionString);
    Console.WriteLine("This is development");
}
else if (builder.Environment.IsProduction()) {
    // Cannot obtain local secrets so signal does not work in production
    //var signalConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
    //builder.Services.AddSignalR().AddAzureSignalR(signalConnectionString);
    Console.WriteLine("This is Production");
}


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    //app.UseAzureSignalR(routes => { routes.MapHub<DataHub>("/DataHub"); });
}
else if (app.Environment.IsProduction()){
    app.UseMigrationsEndPoint();
    //app.UseAzureSignalR(routes => { routes.MapHub<DataHub>("/DataHub"); });
}

// Enable middleware
Configure(app, app.Environment);

app.Run();

// Sets up all of the middleware to be used by the app
void Configure(WebApplication app, IWebHostEnvironment env) {
    // register licence key with syncfusion
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2VVhhQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkFjXH5cc3NXQGdeU0U=;NjA3MDM2QDMyMzAyZTMxMmUzME1qblRKQURUNGVoZlJPeEdMZDJpWVhHQkgvRkZ3WWoyekZtWldyQkNnSGM9");
    
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    // Setup robots.txt file to disallow friendly web-crawling and search engine indexing of login-required pages
    app.Use(async (context, next) => {
        if (context.Request.Path.StartsWithSegments("/robots.txt")) {
            var robotsTxtPath = Path.Combine(env.ContentRootPath + "/Areas/Indexing", "robots.txt");
            // Disallows everything if the robot file does not exist.
            string output = "User-agent: *  \nDisallow: /";
            if (File.Exists(robotsTxtPath)) {
                output = await File.ReadAllTextAsync(robotsTxtPath);
            }
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(output);
        }
        else await next();
    });

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();
    app.UseEndpoints(endpoints => { endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"); });

    app.MapControllers();
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    // Initialise Entity framework model and get reference to the database context.
    EF_Model.Initialize_DbContext_Startup(app.Services);
    app.Services.GetService<OperationService>().DbContext = EF_Model.dbContext;
    // Create the tables
    EF_Model.dbContext.Database.Migrate();
}