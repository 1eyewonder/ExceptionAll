using Example.Client;
using ExceptionAll.Client.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddExceptionAllClientServices();

// Addition of a standard http client
builder.Services.AddHttpClient();

// Addition of a different http client, which has a special header for tracing
builder.Services.AddHttpClient("Test", x =>
{
    x.DefaultRequestHeaders.Add("x-correlation-id", Guid.NewGuid().ToString());
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
