using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RPG.Butler.BLL.Interfaces;
using RPG.Butler.BLL.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace RPG.Butler.BLL.Services
{
    public class ButlerService : IButlerService
    {
        private DiscordSocketClient _discordCli;
        private CommandService _commands;
        private IServiceProvider _services;
        private AuthModel _auth;

        public ButlerService(CommandService commands, DiscordSocketClient discordCli, IServiceProvider services)
        {
            _commands = commands;
            _discordCli = discordCli;
            _services = services;
        }

        public async Task Init()
        {
            _discordCli.MessageReceived += HandleAsync;

            _commands.CommandExecuted += OnCommandExecutedAsync;
        }

        private async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason))
            {
                await context.Channel.SendMessageAsync(result.ErrorReason);
            }

            switch (result)
            {
                case CommandResult customResult:
                    await context.Channel.SendMessageAsync("rolled");
                    break;
            }
        }

        private async Task HandleAsync(SocketMessage message)
        {
            var _message = message as SocketUserMessage;

            if (_message is null || _message.Author.IsBot) return;

            int argPos = 0;

            if (_message.HasStringPrefix("!", ref argPos) || _message.HasMentionPrefix(_discordCli.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_discordCli, _message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
