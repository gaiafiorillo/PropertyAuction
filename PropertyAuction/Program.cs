using PropertyAuction.Components;

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

// builder.Services.AddSingleton<PropertyAuction.Services.Services.UserService>(); 

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
