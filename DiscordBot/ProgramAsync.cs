using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.DiscordBot.Handlers;
using DiscordBot.DiscordBot.Helpers;
using DiscordBot.DiscordBot.Helpers.JsonHelpers;
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
            _client.Log += services.GetRequiredService<LogService>().Log;

            try
            {
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
                await _client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InstallCommandsAsync();
                await services.GetRequiredService<TimerHelper>().Init();
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
            .AddSingleton<DeleteMessagesService>()
            .AddSingleton<ISpamService, SpamService>()
            .AddSingleton<ISteamService, SteamService>()
            .AddSingleton<IGoogleApiHelper, GoogleApiHelper>()
            .AddSingleton<IReadFromFileHelper, ReadFromFileHelper>()
            .AddSingleton<ILoggerHelper, LoggerHelper>()
            .AddSingleton<IJsonHelper, JsonHelper>()
            .AddSingleton<IEmbedHelper, EmbedHelper>()
            .AddSingleton<IDmHelper, DmHelper>()
            .AddSingleton<IEmbedHelper, EmbedHelper>()
            .AddSingleton<LogService>()
            .AddSingleton<ReminderService>()
            .AddSingleton<JsonConstructor>()
            .AddSingleton<IGetDateFromString, GetDateFromString>()
            .AddSingleton<ClientStatusService>()
            .AddSingleton<TimerHelper>()
            .BuildServiceProvider();
    }
}
