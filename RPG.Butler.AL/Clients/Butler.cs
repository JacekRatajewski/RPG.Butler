using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RPG.Butler.BLL.Interfaces;
using RPG.Butler.BLL.Models;
using System;
using System.Threading.Tasks;

namespace RPG.Butler.AL.Clients
{
    public class ButlerClient
    {
        private DiscordSocketClient _discordCli;
        private ILogger _logger;
        private IButlerService _butlerService;
        private AuthModel _auth;

        public ButlerClient(ILogger<ButlerClient> logger, IButlerService butlerService, IConfiguration configuration, DiscordSocketClient discordCli)
        {
            _discordCli = discordCli;
            _logger = logger;
            _butlerService = butlerService;
            _auth = new AuthModel(configuration.GetSection("DiscordApi"));
        }

        public async Task RunAsync()
        {
            try
            {
                await _discordCli.LoginAsync(TokenType.Bot, _auth.Token);
                await _discordCli.StartAsync();
                _discordCli.Connected += () =>
                {
                    Console.WriteLine("Butler is connected and happy!");
                    return Task.CompletedTask;
                };
                _discordCli.Ready += async () =>
                {
                    Console.WriteLine("Butler is ready to go!");
                    Console.WriteLine("(type 'exit' to close Butler)");
                };
                await _butlerService.Init();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                throw;
            }
        }

        public async Task CloseAsync()
        {
            try
            {
                await _discordCli.LogoutAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                throw;
            }
        }
    }
}
