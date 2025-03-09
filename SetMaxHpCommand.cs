using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace SetMaxHPPlugin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetMaxHpCommand : ICommand
    {
        public string Command => "setmaxhp";
        public string[] Aliases => new string[] { "maxhp" };
        public string Description => "Sets a player's max HP.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender is CommandSender commandSender) || !commandSender.CheckPermission("setmaxhp.use"))
            {
                response = "You do not have permission to use this command!";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Usage: setmaxhp <player> <amount>";
                return false;
            }

            Player player = Player.Get(arguments.At(0));
            if (player == null)
            {
                response = "Player not found!";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float maxHp) || maxHp <= 0)
            {
                response = "Invalid max HP value!";
                return false;
            }

            player.MaxHealth = maxHp;
            response = $"Set {player.Nickname}'s max HP to {maxHp}.";
            return true;
        }
    }
}