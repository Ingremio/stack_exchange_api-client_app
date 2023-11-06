using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Project.Server.Helpers;
using Project.Server.Services.StackExchange;
using Project.Server.Services.StackExchange.Tags;
using Project.Server.Services.StackExchange.Users;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
{
    var apiSettings = builder.Configuration.GetSection(nameof(ApiSettings));

    string GetApiSetting(string property) {
        return apiSettings?.GetValue<string>(property) ?? "";
    }
    
    var services = builder.Services;
    {
        services.AddControllers();
    }

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    // Configure strongly typed settings object
    {
        services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
    }
    // Configure cors policies
    {
        services.AddCors(options => {
            options.AddPolicy("ClientPermission", policy => {
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(GetApiSetting(nameof(ApiSettings.AllowedOriginEndpoint)))
                .AllowCredentials();
            });
        });
    }
    // Configure DI for application services
    {
        services.AddScoped<StackExchangeHttpApiService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ITagsService, TagsService>();
    }
    {
        services.AddHttpClient<StackExchangeHttpApiService>(client => {
            client.BaseAddress = new Uri(GetApiSetting(nameof(ApiSettings.ThisWebApiEndpoint)));
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler() {
            AutomaticDecompression = DecompressionMethods.GZip
        });
    }
    {
        // Adding StackExchange authentication provider
        services.AddAuthentication("cookie")
            .AddCookie("cookie")
            .AddOAuth(GetApiSetting(nameof(ApiSettings.AuthenticationScheme)), configuration => {
                configuration.BackchannelHttpHandler = new HttpClientHandler {
                    AutomaticDecompression = DecompressionMethods.GZip
                };

                configuration.SignInScheme = "cookie";

                configuration.ClientId = GetApiSetting(nameof(ApiSettings.ClientId));
                configuration.ClientSecret = GetApiSetting(nameof(ApiSettings.ClientSecret));

                configuration.AuthorizationEndpoint = GetApiSetting(nameof(ApiSettings.AuthorizationEndpoint));
                configuration.TokenEndpoint = GetApiSetting(nameof(ApiSettings.TokenEndpoint));
                configuration.CallbackPath = "/oauth/stackExchange";

                configuration.SaveTokens = true;
                configuration.UserInformationEndpoint = GetApiSetting(nameof(ApiSettings.UserInformationEndpoint));

                configuration.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
                configuration.ClaimActions.MapJsonKey(ClaimTypes.Name, "display_name");

                configuration.Events.OnCreatingTicket = async context => {
                    //context.HttpContext.RequestServices.GetService<>();

                    var uriBuilder = new UriBuilder(context.Options.UserInformationEndpoint);

                    var parameters = HttpUtility.ParseQueryString(uriBuilder.Query);
                    parameters["access_token"] = context.AccessToken;
                    parameters["key"] = GetApiSetting(nameof(ApiSettings.Key));

                    uriBuilder.Query = parameters.ToString();

                    using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json") { CharSet = Encoding.UTF8.WebName });

                    using var response = await context.Backchannel.SendAsync(request);
                    var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                    var root = JsonDocument.Parse(json.ToString()).RootElement;
                    var user = root.GetProperty("items").EnumerateArray().FirstOrDefault();

                    context.RunClaimActions(user);
                };
            });
    }

    var app = builder.Build();

    // Configure authentication and mappings
    {
        app.UseAuthentication();

        app.MapGet("/oauth/me", (HttpContext context) => {
            return context.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
        });

        app.MapGet("/oauth/login", () => {
            return Results.Challenge(
                new AuthenticationProperties() { RedirectUri = GetApiSetting(nameof(ApiSettings.ThisWebApiEndpoint)) },
                authenticationSchemes: new List<string>() { GetApiSetting(nameof(ApiSettings.AuthenticationScheme)) }
            );
        });
    }
    
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();
    }

    // Configure the Cors
    {
        app.UseCors("ClientPermission");
    }

    // Configure the HTTP request pipeline.
    {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        } else {
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");
    }

    app.Run();
}