using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace PlayerNames
{
    public class PlayerNames : Plugin<Config>
    {
        public override string Name => "PlayerNames";
        public override string Author => "Shibusek";
        public override Version Version => new Version(1, 0, 2);

        public override void OnEnabled()
        {
            Log.Info("Thanks for using PlayerNames by Shibusek");
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            base.OnDisabled();
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!Config.IsEnabled) return;

            if (Config.Debug)
                Log.Info($"Changing role for player {ev.Player.Nickname} to {ev.NewRole}");

            // ✅ If the player already has a custom name, don't change it
            if (ev.Player.DisplayNickname != ev.Player.Nickname)
            {
                if (Config.Debug)
                    Log.Info($"[DEBUG] Player {ev.Player.Nickname} already has a custom name ({ev.Player.DisplayNickname}). Skipping rename.");
                return;
            }

            // Assign random D-XXXX number to Class-D if enabled
            if (ev.NewRole == RoleTypeId.ClassD && Config.DNumbers)
            {
                ev.Player.DisplayNickname = $"D-{new Random().Next(1000, 9999)}";
            }
            else if (Config.RoleNicknames.ContainsKey(ev.NewRole.ToString()))
            {
                var nicknames = Config.RoleNicknames[ev.NewRole.ToString()];
                string randomNickname = nicknames[new Random().Next(nicknames.Count)];
                ev.Player.DisplayNickname = randomNickname;
            }
            else
            {
                if (Config.Debug)
                    Log.Info($"No nickname configuration found for role {ev.NewRole}");
            }
        }
    }
}
