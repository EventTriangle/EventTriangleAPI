using EventTriangleAPI.Authorization.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Authorization.Presentation.Constants;
using EventTriangleAPI.Shared.DTO.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var azAdSection = builder.Configuration.GetSection("AzureAd");
var azAdConfig = azAdSection.Get<AzureAdConfiguration>();
var adClientSecret = Environment.GetEnvironmentVariable("EVENT_TRIANGLE_AD_CLIENT_SECRET");
var corsPolicyName = "CorsPolicyName";
var allowedHosts = builder.Configuration["AllowedHosts"];

azAdConfig.ClientSecret = adClientSecret;
azAdConfig.AzureAdTokenUrl = $"{azAdConfig.Instance}/{azAdConfig.TenantId}/oauth2/v2.0/token";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "EventTriangle Authorization API", Version = "v1" });
});
builder.Services.AddSpaStaticFiles(config => { config.RootPath = "wwwroot"; });
builder.Services.AddMvc();

if (string.IsNullOrEmpty(adClientSecret))
{
    throw new ArgumentNullException(nameof(adClientSecret));
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "appOidc";
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("appOidc", options =>
    {
        options.Authority = $"{azAdConfig.Instance}/{azAdConfig.TenantId}/";
        options.ClientId = azAdConfig.ClientId.ToString();
        options.ClientSecret = azAdConfig.ClientSecret;
        options.CallbackPath = new PathString("/authorization-redirect");
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;
        options.Scope.Clear();
        options.Scope.Add("EventTriangleLocalAuth.All");
        options.Scope.Add("offline_access");
        options.Scope.Add("openid");
        options.UseTokenLifetime = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(allowedHosts)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped(_ => azAdConfig);
builder.Services.AddScoped<RefreshTokenCommandHandler>();
builder.Services.AddScoped<GetTokenCommandHandler>();

builder.Services.AddGrpc();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventTriangle Authorization API V1"); });

app.UseStaticFiles();

app.UseSpaStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Map(SpaRouting.Transactions, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Cards, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Deposit, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Contacts, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Support, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Tickets, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));
app.Map(SpaRouting.Users, config => config.UseSpa(spa => spa.Options.SourcePath = "/wwwroot"));

app.Run();