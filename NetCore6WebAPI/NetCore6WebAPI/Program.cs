using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// This sets up the configuration to pull information from the appsettings.json file
//"JWT:ValidAudience": "yourdomain.com",
//"JWT:ValidIssuer": "yourdomain.com",
//"JWT:IssuerSigningKey": "THISISAREALLYSECRETKEYONLYUSEDINTHISPROGRAM"
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

// Adding Authentication
builder.Services.AddAuthentication(options => {
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
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = ApiConstants.ValidateIssuer,
        ValidateAudience = ApiConstants.ValidateAudience,
        ValidateLifetime = ApiConstants.ValidateLifetime,
        ValidateIssuerSigningKey = ApiConstants.ValidateIssuerSigningKey,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:IssuerSigningKey"]))
    };
});

builder.Services.AddAntiforgery(options =>
{
    //options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-XSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;

});

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
