using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Presentation.Constants;
using EventTriangleAPI.Authorization.Presentation.DependencyInjection;
using EventTriangleAPI.Shared.DTO.Models;

var builder = WebApplication.CreateBuilder(args);

var azAdSection = builder.Configuration.GetSection("AzureAd");
var azAdConfig = azAdSection.Get<AzureAdConfiguration>();
var adClientSecret = Environment.GetEnvironmentVariable("EVENT_TRIANGLE_AD_CLIENT_SECRET");
var allowedHosts = builder.Configuration["AllowedHosts"];
var reverseProxySection = builder.Configuration.GetSection("ReverseProxy");

azAdConfig.ClientSecret = adClientSecret;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "wwwroot"; });
builder.Services.AddMvc();
builder.Services.ConfigureYarp(reverseProxySection);
builder.Services.ConfigureCors(allowedHosts);

if (string.IsNullOrEmpty(adClientSecret))
{
    throw new ArgumentNullException(nameof(adClientSecret));
}

builder.Services.AddAppAuthentication(
    azAdConfig.Authority,
    azAdConfig.ClientId.ToString(),
    azAdConfig.ClientSecret,
    azAdConfig.CallbackPath,
    azAdConfig.Scopes);

builder.Services.AddScoped(_ => azAdConfig);
builder.Services.AddScoped<RefreshTokenCommandHandler>();
builder.Services.AddScoped<GetTokenCommandHandler>();

builder.Services.AddGrpc();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventTriangle Authorization API V1"); });

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSpaStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseRouting();

app.UseCors(CorsServices.CorsPolicyName);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(options =>
{
    options.MapReverseProxy();
    options.MapControllers();    
});

app.Map(SpaRouting.Transactions, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Cards, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Deposit, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Contacts, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Support, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Tickets, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Users, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));

app.Run();