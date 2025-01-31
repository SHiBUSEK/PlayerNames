using System;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace PlayerNames
{
    public class PlayerNames : Plugin<Config>
    {
        public override string Name => "PlayerNames";
        public override string Author => "Shibusek";
        public override Version Version => new Version(1, 0, 0);

        private bool _ucrEnabled = false;
        private Type _ucrCustomRoleType;
        private MethodInfo _ucrGetMethod;

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

            // Check if UCR is installed
            var ucrAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "UncomplicatedCustomRoles");

            if (ucrAssembly != null)
            {
                _ucrCustomRoleType = ucrAssembly.GetType("CustomPlayerRoles.CustomRole");
                _ucrGetMethod = _ucrCustomRoleType?.GetMethod("Get");

                if (_ucrCustomRoleType != null && _ucrGetMethod != null)
                {
                    _ucrEnabled = true;
                    Log.Info("Uncomplicated Custom Roles (UCR) detected! Custom roles will be ignored.");
                }
            }

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

            var role = ev.NewRole;

            if (Config.Debug)
                Log.Info($"Changing role for player {ev.Player.Nickname} to {role}");

            // Check if UCR is enabled
            if (_ucrEnabled)
            {
                object customRole = _ucrGetMethod?.Invoke(null, new object[] { ev.Player });
                if (customRole != null)
                {
                    if (Config.Debug)
                        Log.Info($"[DEBUG] Player {ev.Player.Nickname} has a UCR role. Skipping name change.");
                    return;
                }
            }

            // ✅ NEW: Check if the player already has a custom display name (set by UCR)
            if (!string.IsNullOrEmpty(ev.Player.DisplayNickname))
            {
                if (Config.Debug)
                    Log.Info($"[DEBUG] Player {ev.Player.Nickname} already has a custom name ({ev.Player.DisplayNickname}). Skipping rename.");
                return;
            }

            // Ignore non-standard roles
            if (!DefaultRoles.Contains(role))
            {
                if (Config.Debug)
                    Log.Info($"Role {role} is custom (but not UCR). Ignoring nickname modification.");
                return;
            }

            // Assign random D-XXXX number to Class-D if enabled
            if (role == RoleTypeId.ClassD && Config.DNumbers)
            {
                ev.Player.DisplayNickname = $"D-{new Random().Next(1000, 9999)}";
            }
            else if (Config.RoleNicknames.ContainsKey(role.ToString()))
            {
                var nicknames = Config.RoleNicknames[role.ToString()];
                string randomNickname = nicknames[new Random().Next(nicknames.Count)];
                ev.Player.DisplayNickname = randomNickname;
            }
            else
            {
                if (Config.Debug)
                    Log.Info($"No nickname configuration found for role {role}");
            }
        }
    }
}
