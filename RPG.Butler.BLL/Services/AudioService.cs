using Discord.Audio;
using RPG.Butler.BLL.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RPG.Butler.BLL.Services
{
    public class AudioService : IAudioService
    {
        private Process CreateStream(string path)
        {
            var process = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $@"/C C:\YT\youtube-dl.exe --default-search ytsearch -o - { path } | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            return Process.Start(process);
        }

        public async Task<bool> SendAsync(IAudioClient client, string path)
        {
            using (var ffmpeg = CreateStream(path))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try
                {
                    Console.WriteLine($"Playing: {path}");
                    await output.CopyToAsync(discord);
                }
                finally
                {
                    await discord.FlushAsync();
                    Console.WriteLine($"Ended: {path}");
                }
                return true;
            }
        }
    }
}
