using Exiled.API.Features;
using System;
using Exiled.CreditTags;

namespace SetMaxHPPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "SetMaxHPPlugin";
        public override string Author => "StormCrasher783";
        public override Version Version => new Version(1, 0, 0);

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            base.OnDisabled();
        }

        private void OnWaitingForPlayers()
        {
            Log.Info("SetMaxHPPlugin has been enabled!");
        }
    }
}