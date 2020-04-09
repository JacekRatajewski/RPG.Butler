using Microsoft.Extensions.Configuration;

namespace RPG.Butler.BLL.Models
{
    public class AuthModel
    {
        public AuthModel(IConfigurationSection configurationSection)
        {
            Token = configurationSection["Token"];
            Secret = configurationSection["Secret"];
            GuildId = ulong.Parse(configurationSection["GuildId"]);
        }

        public string Token { get; set; }
        public string Secret { get; set; }
        public ulong GuildId { get; set; }
    }
}
