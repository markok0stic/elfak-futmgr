using Neo4jClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/");
});
app.MapControllers();
app.Run();