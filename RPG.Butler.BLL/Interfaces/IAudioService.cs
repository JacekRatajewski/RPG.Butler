using Discord.Audio;
using System.Threading.Tasks;

namespace RPG.Butler.BLL.Interfaces
{
    public interface IAudioService
    {
        Task<bool> SendAsync(IAudioClient client, string path);
    }
}
