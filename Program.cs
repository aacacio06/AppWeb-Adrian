using AppWeb_Adriann.Components;
using AppWeb_Adriann.Configs;
using AppWeb_Adriann.DAO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Conexao>();
builder.Services.AddScoped<ProcessoDAO>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
