using System.Security.Claims;
using System.Text;
using elfak_futmgr.Helpers;
using elfak_futmgr.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Neo4jClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthDbService, AuthDbService>();
var graphClient = new BoltGraphClient(
    new Uri(builder.Configuration.GetValue<string>("Neo4j:Uri")),
    builder.Configuration.GetValue<string>("Neo4j:Username"),
    builder.Configuration.GetValue<string>("Neo4j:Password"));
graphClient.ConnectAsync();
builder.Services.AddSingleton<IGraphClient>(graphClient);
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters();
    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretTokenForMyApp"));
    options.TokenValidationParameters.ValidateIssuer = false;
    options.TokenValidationParameters.ValidateAudience = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
/* might be necessary 
app.UseStaticFiles();
*/
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.Use(async (context, next) =>
{
    var jwToken = context.Session.GetString("JWToken");
    if (!string.IsNullOrEmpty(jwToken))
    {
        var user = JwtTokenHelper.DecodeToken(jwToken);
        if (user != null)
        {
            var list = new List<Claim>();
            list.Add(new(ClaimTypes.Sid, user.Id.ToString()));
            list.Add(new(ClaimTypes.Name, user.Username));
            list.Add(new(ClaimTypes.Role, user.Role));
            
            ClaimsIdentity identity;
            identity = new ClaimsIdentity(list,"Custom");
            identity.Actor = null;
            identity.BootstrapContext = null;
            identity.Label = null;
            context.User = new(identity);
            context.Request.Headers.Add("Authorization", $"Bearer {jwToken}");
        }
    }
    await next();
});
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=Index}/");
});
app.MapControllers();
app.Run();
