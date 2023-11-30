using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder _WebApplicationBuilder = WebApplication.CreateBuilder(args);
ConfigurationManager _ConfigurationManager = _WebApplicationBuilder.Configuration;

// Adding Cors
// READ THIS: https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-8.0#set-the-allowed-origins
string _MainCorsPolicyName = "CorsPolicy";
_WebApplicationBuilder.Services.AddCors(options =>
{
    options.AddPolicy(_MainCorsPolicyName, builder => builder.AllowAnyOrigin()
    //.WithOrigins("https://example.com", "https://www.example.com")
    .AllowAnyHeader()
    .AllowAnyMethod());
    //.AllowCredentials()); // Add AllowCredentials() if you use WithOrigins()
});

// Adding Authentication
_WebApplicationBuilder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => {
    //options.Cookie.Name = "auth_cookie";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Events.OnRedirectToLogin = redirectContext =>
    {
        //if (!(IsWebRequest(redirectContext.Request) || IsApiRequest(redirectContext.Request)))
        //{
        //    redirectContext.Response.Redirect(redirectContext.RedirectUri);
        //    return Task.CompletedTask;
        //}
        redirectContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
}).AddJwtBearer(options =>
{
    // This sets up the configuration to pull information from the appsettings.json file
    //"JWT:ValidAudience": "yourdomain.com",
    //"JWT:ValidIssuer": "yourdomain.com",
    //"JWT:IssuerSigningKey": "THISISAREALLYSECRETKEYONLYUSEDINTHISPROGRAM"
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = ApiConstants.ValidateIssuer,
        ValidateAudience = ApiConstants.ValidateAudience,
        ValidateLifetime = ApiConstants.ValidateLifetime,
        ValidateIssuerSigningKey = ApiConstants.ValidateIssuerSigningKey,
        ValidAudience = _ConfigurationManager["JWT:ValidAudience"],
        ValidIssuer = _ConfigurationManager["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_ConfigurationManager["JWT:IssuerSigningKey"]))
    };
});

_WebApplicationBuilder.Services.AddAntiforgery(options =>
{
    //options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-XSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;

});

_WebApplicationBuilder.Services.AddMvc(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

_WebApplicationBuilder.Services.AddSingleton<IConfiguration>(_ConfigurationManager);
_WebApplicationBuilder.Services.AddControllers();
_WebApplicationBuilder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
_WebApplicationBuilder.Services.AddSwaggerGen();

var app = _WebApplicationBuilder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(_MainCorsPolicyName);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
