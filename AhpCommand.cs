using CommandSystem;
using Exiled.API.Features;
using System;

namespace AhpCommand
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class AddAHPCommand : ICommand
    {
        public string Command => "addAHP";
        public string Description => "Adds artificial health points (AHP) to a player.";
        public string[] Aliases => new string[] { };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 2)
            {
                response = "Usage: addAHP <player> <amount> [decay rate]";
                return false;
            }

            Player target = Player.Get(arguments.At(0));
            if (target == null)
            {
                response = "Player not found.";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float amount) || amount <= 0)
            {
                response = "Invalid amount.";
                return false;
            }

            float decay = 0f;
            if (arguments.Count >= 3 && float.TryParse(arguments.At(2), out float parsedDecay))
            {
                decay = parsedDecay;
            }

            target.AddAhp(amount, 75f, decay, 0.7f, 0f, true);
            
            response = $"{target.Nickname} received {amount} AHP with a decay rate of {decay}.";
            return true;
        }
    }
}
