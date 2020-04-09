using Discord;
using Discord.Commands;
using RPG.Butler.BLL.Interfaces;
using RPG.Butler.BLL.Services;
using System.Threading.Tasks;

namespace RPG.Butler.AL.Modules
{
    public class Play : ModuleBase<SocketCommandContext>
    {
        private IAudioService _audio;

        public Play()
        {
            _audio = new AudioService();
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync(string url, bool loop = true)
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("Użytkownik musi wejść na kanał głosowy."); return;
            }
            var audio = await channel.ConnectAsync();
            var ended = await _audio.SendAsync(audio, url);

            if (ended)
            {
                await PlayAsync(url);
            }
        }
    }
}
