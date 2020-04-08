using Discord.WebSocket;
using System.Threading.Tasks;

namespace RPG.Butler.BLL.Interfaces
{
    public interface IButlerService
    {
        Task Init();
    }
}
