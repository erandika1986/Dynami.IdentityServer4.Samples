using IdentityServerHost.Quickstart.UI;
using InMemorySample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebAPIServices();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
