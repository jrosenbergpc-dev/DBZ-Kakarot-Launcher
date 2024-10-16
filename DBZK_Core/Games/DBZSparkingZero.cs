using DBZK_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBZK_Core.Games
{
    public class DBZSparkingZero : IVideoGame
    {
        public string Name { get; set; }
        public string InstallationPath { get; set; }
        public string ModFolder { get; set; }
        public string DisableFolder { get; set; }
        public bool ModPatchRequired { get; set; }
        public string Version { get; set; }

        public void InstallPatch()
        {
            throw new NotImplementedException();
        }
    }
}
