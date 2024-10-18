using DBZK_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBZK_Core.Games
{
    public class DBZKakarot : IVideoGame
    {
        public string Name { get; set; } = "Dragonball Z: Kakarot";
        public string Logo { get; set; } = "/Images/dbz-kakarot-logo.png";
        public string InstallationPath { get; set; } = string.Empty;
        public string ModFolder { get; set; } = "\\AT\\Content\\Paks\\~mods";
        public string DisableFolder { get; set; } = "\\AT\\Content\\Paks\\~disabled";
        public bool ModPatchRequired { get; set; } = false;
        public string Version { get; set; } = "1.0";

        public bool InstallPatch()
        {
            throw new NotImplementedException();
        }
    }
}
