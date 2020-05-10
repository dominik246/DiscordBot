using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.DiscordBot.Handlers;
using DiscordBot.DiscordBot.Services;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class ProgramAsync
    {
        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                ExclusiveBulkDelete = true,
                LogLevel = LogSeverity.Info
            });

            var services = BuildServiceProvider();
            _client.Log += services.GetRequiredService<LogService>().LogClient;

            try
            {
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
                await _client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InstallCommandsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await Task.Delay(-1);
            }
        }

        private IServiceProvider BuildServiceProvider()
            => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<CommandExecutingService>()
            .AddSingleton<DeleteMessagesService>()
            .AddSingleton<ISpamService, SpamService>()
            .AddSingleton<ISteamService, SteamService>()
            .AddSingleton<IGoogleApiHelper, GoogleApiHelper>()
            .AddSingleton<IReadFromFileHelper, ReadFromFileHelper>()
            .AddSingleton<ILoggerHelper, LoggerHelper>()
            .AddSingleton<IJsonHelper, JsonHelper>()
            .AddSingleton<IEmbedHelper, EmbedHelper>()
            .AddSingleton<IDmOwnerHelper, DmOwnerHelper>()
            .AddSingleton<LogService>()
            .BuildServiceProvider();
    }
}
