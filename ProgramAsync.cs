using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class ProgramAsync
    {
        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            var services = BuildServiceProvider();
            var client = services.GetRequiredService<DiscordSocketClient>();
            services.GetRequiredService<CommandService>().Log += Log;

            try
            {
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
                await _client.StartAsync();
                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InstallCommandsAsync();
            }
            catch (Exception ex)
            {
                await Log(new LogMessage(LogSeverity.Critical, ex.Source, ex.Message));
            }
            finally
            {
                await Task.Delay(-1);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public IServiceProvider BuildServiceProvider()
            => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<WebClient>()
            .BuildServiceProvider();

    }
}
