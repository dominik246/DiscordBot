﻿using Discord;
using DiscordBot.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.DiscordBot.Services
{
    public interface IEmbedHelper
    {
        Task<Embed> Build(List<(string, string)> content, string title = "");
    }
}