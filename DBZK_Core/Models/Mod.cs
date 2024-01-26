using System;

namespace DBZK_Core.Models
{
    public class Mod
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsEnabled { get; set; }
        public string Version { get; set; }
    }
}
