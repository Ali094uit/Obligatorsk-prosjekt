using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AndreasBlog.Client;
using AndreasBlog.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationService>();


await builder.Build().RunAsync();


