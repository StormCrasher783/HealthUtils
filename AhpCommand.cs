using System;
using System.Collections.Generic;
using System.Timers;
using CommandSystem;
using Exiled.API.Features;

namespace YourPluginNamespace
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AddAHPCommand : ICommand
    {
        public string Command => "addAHP";
        public string Description => "Adds artificial health (AHP) to a player with a set decay rate.";
        public string[] Aliases => new string[] { };

        private static Dictionary<Player, Timer> ActiveAHPDecay = new();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 3)
            {
                response = "Usage: addAHP <player> <amount> <rate_of_degeneration>";
                return false;
            }

            Player target = Player.Get(arguments.At(0));
            if (target == null)
            {
                response = "Player not found.";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float ahpAmount) || ahpAmount <= 0)
            {
                response = "Invalid AHP amount.";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float ahpDecayRate) || ahpDecayRate < 0)
            {
                response = "Invalid AHP decay rate.";
                return false;
            }

            // Apply initial AHP
            target.ArtificialHealth += ahpAmount;

            // Start decay process
            StartAHPDecay(target, ahpDecayRate);

            response = $"Gave {target.Nickname} {ahpAmount} AHP with a decay rate of {ahpDecayRate}.";
            return true;
        }

        private void StartAHPDecay(Player player, float decayRate)
        {
            if (ActiveAHPDecay.ContainsKey(player))
            {
                ActiveAHPDecay[player].Stop();
                ActiveAHPDecay[player].Dispose();
                ActiveAHPDecay.Remove(player);
            }

            Timer decayTimer = new Timer(1000);
            decayTimer.Elapsed += (sender, e) =>
            {
                if (player.ArtificialHealth <= 0 || !player.IsAlive)
                {
                    decayTimer.Stop();
                    decayTimer.Dispose();
                    ActiveAHPDecay.Remove(player);
                }
                else
                {
                    player.ArtificialHealth = Math.Max(0, player.ArtificialHealth - decayRate);
                }
            };

            ActiveAHPDecay[player] = decayTimer;
            decayTimer.Start();
        }
    }
}
