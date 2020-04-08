using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RPG.Butler.AL.Clients;
using RPG.Butler.BLL.Interfaces;
using RPG.Butler.BLL.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RPG.Butler.AL
{
    public class Startup
    {
        private IConfigurationRoot _configuration;
        public ServiceProvider Services;
        private DiscordSocketClient _client;
        private CommandService _commands;

        public Startup()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            ConfigureSettings();
            ConfigureServices();
        }

        public async Task RegisterCommands()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
        }

        private void ConfigureServices()
        {

            Services = new ServiceCollection()
               .AddSingleton<IConfiguration>(config => _configuration)
               .AddSingleton<DiscordSocketClient>(_client)
               .AddSingleton<CommandService>(_commands)
               .AddTransient<IButlerService, ButlerService>()
               .AddSingleton<ButlerClient>()
               .AddLogging(loggingBuilder =>
               {
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog("nlog.config");
               })
               .BuildServiceProvider();
        }

        private void ConfigureSettings()
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                _configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .Build();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}