using Microsoft.AspNetCore.Identity;
using PropertyAuction.Components;
using PropertyAuction.Core.Interfaces;
using PropertyAuction.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// Register services
builder.Services.AddSingleton<PropertyAuction.Services.Services.AuctionService>();

// Change BidService registration to use the one from AuctionService
builder.Services.AddSingleton<PropertyAuction.Services.Services.BidService>(sp =>
{
    var auctionService = sp.GetRequiredService<PropertyAuction.Services.Services.AuctionService>();
    return auctionService.GetBidService(); // Use the existing instance!
});

//register admin service
builder.Services.AddSingleton<PropertyAuction.Services.Services.AdminService>(sp =>
{
    var auctionService = sp.GetRequiredService<PropertyAuction.Services.Services.AuctionService>();
    return new PropertyAuction.Services.Services.AdminService(auctionService.GetTree());
});

// register user repository and service
builder.Services.AddSingleton<PropertyAuction.Core.Interfaces.IUserRepository, PropertyAuction.DataStructures.UserRepository>();
builder.Services.AddSingleton<PropertyAuction.Core.Interfaces.IUserService, PropertyAuction.Core.Services.UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
