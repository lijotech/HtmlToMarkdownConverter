using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
// Configure services
builder.Services.AddControllers(); // Add controller services

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
// Add MemoryCache (required for rate limiting)
builder.Services.AddMemoryCache();
// Configure rate limiting
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true; // Enable endpoint-based rate limiting
    options.StackBlockedRequests = false;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.EndpointWhitelist = new List<string>
    {
        "/css/*",
        "/js/*",
        "/images/*",
        "/lib/*"
    };
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*", // Apply to all endpoints
            Period = "1m",  // Limit requests per minute
            Limit = 10      // Maximum 10 requests per minute
        }
    };
});
builder.Services.AddInMemoryRateLimiting();

// Build the app
var app = builder.Build();

// Middleware for rate limiting
app.UseStaticFiles();
app.UseIpRateLimiting();

// Middleware for routing and controllers
app.MapControllers();
app.UseHttpsRedirection();

app.MapRazorPages();
app.Run();
