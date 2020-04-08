using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG.Butler.BLL.Models
{
    public class AuthModel
    {
        public AuthModel(IConfigurationSection configurationSection)
        {
            Token = configurationSection["Token"];
        }

        public string Token { get; set; }
    }
}
