using System;
using System.Collections.Generic;
using System.Timers;
using CommandSystem;
using Exiled.API.Features;
using UnityEngine;

namespace SetMaxHPPlugin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PlayerRegenCommand : ICommand
    {
        public string Command => "regen";
        public string Description => "Grants a player health regeneration over a set duration.";
        public string[] Aliases => new string[] { };

        private static readonly Dictionary<Player, Timer> ActiveRegens = new();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 3)
            {
                response = "Usage: regen <player> <amount_per_second> <duration>";
                return false;
            }

            Player target = Player.Get(arguments.At(0));
            if (target == null)
            {
                response = "Player not found.";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float amountPerSecond) || amountPerSecond <= 0)
            {
                response = "Invalid amount per second.";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float duration) || duration < 0)
            {
                response = "Invalid duration.";
                return false;
            }

            StartRegen(target, amountPerSecond, duration);

            response = duration == 0
                ? $"{target.Nickname} will regenerate {amountPerSecond} HP per second indefinitely."
                : $"{target.Nickname} will regenerate {amountPerSecond} HP per second for {duration} seconds.";
            return true;
        }

        private void StartRegen(Player player, float amountPerSecond, float duration)
        {
            if (ActiveRegens.ContainsKey(player))
            {
                ActiveRegens[player].Stop();
                ActiveRegens[player].Dispose();
                ActiveRegens.Remove(player);
            }

            Timer regenTimer = new Timer(1000);
            float elapsed = 0;

            regenTimer.Elapsed += (sender, e) =>
            {
                if ((duration > 0 && elapsed >= duration) || !player.IsAlive)
                {
                    regenTimer.Stop();
                    regenTimer.Dispose();
                    ActiveRegens.Remove(player);
                }
                else
                {
                    player.Health = Mathf.Min(player.MaxHealth, player.Health + amountPerSecond);
                    elapsed += 1;
                }
            };

            ActiveRegens[player] = regenTimer;
            regenTimer.Start();
        }
    }
}