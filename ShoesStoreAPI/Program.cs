using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoesStoreAPI.Areas.Identity.Data;
using ShoesStoreAPI.Class;
using ShoesStoreAPI.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = ConnectionURL.User ?? throw new InvalidOperationException("Connection string 'UserContextConnection' not found.");

builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<UserContext>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvcCore();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ServiceCollection>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.Run();
