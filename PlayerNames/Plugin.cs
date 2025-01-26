using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles; // Required for RoleTypeId

namespace PlayerNames
{
    public class PlayerNames : Plugin<Config>
    {
        public override string Name => "PlayerNames";
        public override string Author => "Shibusek";
        public override Version Version => new Version(1, 0, 0);

        // List of all default SCP:SL roles (as of version 14.0)
        private static readonly RoleTypeId[] DefaultRoles =
        {
            RoleTypeId.ClassD, RoleTypeId.Scientist, RoleTypeId.FacilityGuard,
            RoleTypeId.NtfPrivate, RoleTypeId.NtfSergeant, RoleTypeId.NtfCaptain, RoleTypeId.NtfSpecialist,
            RoleTypeId.ChaosConscript, RoleTypeId.ChaosRifleman, RoleTypeId.ChaosRepressor, RoleTypeId.ChaosMarauder,
            RoleTypeId.Scp049, RoleTypeId.Scp0492, RoleTypeId.Scp079, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939,
            RoleTypeId.Tutorial
        };

        public override void OnEnabled()
        {
            Log.Info("Thanks for using PlayerNames by Shibusek");
            // Registering the ChangingRole event
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            // Unregistering the ChangingRole event
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            base.OnDisabled();
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!Config.IsEnabled) return;

            var role = ev.NewRole; // The new role being assigned to the player
            if (Config.Debug)
                Log.Info($"Changing role for player {ev.Player.Nickname} to {role}");

            // Ignore all custom roles (roles not in DefaultRoles)
            if (!DefaultRoles.Contains(role))
            {
                if (Config.Debug)
                    Log.Info($"Role {role} is custom. Ignoring nickname modification.");
                return;
            }

            // Modify nickname for Class D players if DNumbers is enabled
            if (role == RoleTypeId.ClassD && Config.DNumbers)
            {
                // Generate a random D-XXXX number
                ev.Player.DisplayNickname = $"D-{new Random().Next(1000, 9999)}";
            }
            else if (Config.RoleNicknames.ContainsKey(role.ToString()))
            {
                // Assign a random nickname from the list for the role
                var nicknames = Config.RoleNicknames[role.ToString()];
                string randomNickname = nicknames[new Random().Next(nicknames.Count)];
                ev.Player.DisplayNickname = randomNickname; // Use only the nickname
            }
            else
            {
                if (Config.Debug)
                    Log.Info($"No nickname configuration found for role {role}");
            }
        }
    }
}
