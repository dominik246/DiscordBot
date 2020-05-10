using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.DiscordBot.Services;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly CommandExecutingService _execution;
        public SocketUserMessage Message { get; set; }
        public IResult Result { get; set; }

        public CommandHandlingService(IServiceProvider services, CommandService commands, DiscordSocketClient client, CommandExecutingService execution)
        {
            _commands = commands;
            _client = client;
            _services = services;
            _execution = execution;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            //
            // If you do not use Dependency Injection, pass null.
            // See Dependency Injection guide for more information.
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            Message = messageParam as SocketUserMessage;
            if (Message == null)
                return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(Message.HasCharPrefix('!', ref argPos) ||
                Message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                Message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, Message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            Result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
