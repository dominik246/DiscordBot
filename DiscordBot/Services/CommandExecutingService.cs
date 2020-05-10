using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public class CommandExecutingService
    {
        private readonly CommandService _commands;
        private readonly LogService _logger;

        public CommandExecutingService(CommandService commands, LogService logger)
        {
            _commands = commands;
            _commands.CommandExecuted += OnCommandExecutedAsync;

            _logger = logger;
            _commands.Log += _logger.LogCommand;
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            string commandName = command.IsSpecified ? command.Value.Name : "A command";

            await _logger.LogCommand(new LogMessage(LogSeverity.Info, "CommandExc", $"'{commandName}' was executed " +
                $"at '{DateTime.Now}' by '{context.Message.Author.Username}#{context.Message.Author.Discriminator}' " +
                $"in guild '{context.Guild.Name}'."));
        }
    }
}
