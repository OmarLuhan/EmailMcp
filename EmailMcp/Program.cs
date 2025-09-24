using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using EmailMcp.Configuration;
using EmailMcp.Notificator;

var builder = Host.CreateEmptyApplicationBuilder(settings:null);

// Configurar las fuentes de configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(nameof(MailOptions)));
builder.Services.AddScoped<ISendEmail, SendEmail>();

var app = builder.Build();
await app.RunAsync();