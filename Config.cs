using Exiled.API.Interfaces;

namespace SetMaxHPPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
}