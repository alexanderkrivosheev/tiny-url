using TinyURL.Commands;
using TinyURL.Configs;
using TinyURL.Core.Interfaces;
using TinyURL.Core.Services;
using TinyURL.Infrastructure.Generators;
using TinyURL.Infrastructure.Repositories;
using TinyURL.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false,
    reloadOnChange: true);
});

builder.ConfigureServices((context, services) =>
{
    services.AddSingleton<TinyURL.AppContext>();
    services.AddTransient<TinyUrlServices>();
    services.AddTransient<ICommandRegistry, TinyURLCommandRegistry>();

    services.AddSingleton<IUrlShorteningService, UrlShorteningService>();
    services.AddSingleton<IUrlGenerator, Base62UrlGenerator>();
    services.AddSingleton<IUrlRepository, InMemoryUrlRepository>();

    services.AddTransient<ICommand, Welcome>();
    services.AddTransient<ICommand, Exit>();
    services.AddTransient<ICommand, Help>();
    services.AddTransient<ICommand, Create>();
    services.AddTransient<ICommand, Delete>();
    services.AddTransient<ICommand, Get>();
    services.AddTransient<ICommand, Stats>();

    services.Configure<UrlOptions>(context.Configuration.GetSection(nameof(UrlOptions)));
    services.AddSingleton<IUrlShorteningSettings, AppSettings>();
});

IHost host = builder.Build();

var urlServices = host.Services.GetRequiredService<TinyUrlServices>();

var commandLineArgs = (args.Length != 0) ? string.Join(" ", args): "welcome";

await urlServices.ExecuteAsync(commandLineArgs);