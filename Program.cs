using Game.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Game.Authorization.AuthorizationHandlers;
using Game.Authorization.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Connect app to the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// MVC => Setup app with controllers and views
builder.Services.AddControllersWithViews();

// Set fallback auth policy, now all users must be authenticated
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.AddPolicy("SubscriptionDuration", p => p.Requirements.Add(new MinimumSubscriptionDurationRequirement(2)));
});

// Register Auth Handlers
builder.Services.AddScoped<IAuthorizationHandler, CharacterAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, MinimumSubscriptionDurationHandler>();

// Build the app instance
var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    var password = builder.Configuration.GetValue<string>("SeedUserPW");
    try {
        await SeedData.Initialize(services, password);
    }
    catch(Exception e) {
        Console.WriteLine(e.Message);
        return;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
