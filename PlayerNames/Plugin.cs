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
        public override Version Version => new Version(1, 0, 3);

        private static readonly Random random = new Random();

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
                Log.Info($"[DEBUG] ChangingRole triggered for {ev.Player.Nickname}. Current role: {ev.NewRole}");

            if (ev.Player.SessionVariables.TryGetValue("PlayerNames_LastRole", out object lastRole) && lastRole.ToString() == ev.NewRole.ToString())
            {
                if (Config.Debug)
                    Log.Info($"[DEBUG] Skipping rename for {ev.Player.Nickname}. Role {ev.NewRole} was already processed.");
                return;
            }

            ev.Player.SessionVariables["PlayerNames_LastRole"] = ev.NewRole.ToString();

            if (ev.NewRole == RoleTypeId.ClassD && Config.DNumbers)
            {
                string dNumber = $"D-{random.Next(1000, 9999)}";
                ev.Player.DisplayNickname = dNumber;
                if (Config.Debug)
                    Log.Info($"[DEBUG] Assigned D-Class number: {dNumber} to {ev.Player.Nickname}");
            }
            else if (Config.RoleNicknames.TryGetValue(ev.NewRole.ToString(), out var nicknames) && nicknames.Any())
            {
                string randomNickname = nicknames[random.Next(nicknames.Count)];
                ev.Player.DisplayNickname = randomNickname;
                if (Config.Debug)
                    Log.Info($"[DEBUG] Assigned random nickname: {randomNickname} to {ev.Player.Nickname}");
            }
            else
            {
                if (Config.Debug)
                    Log.Info($"[DEBUG] No nickname configuration found for role {ev.NewRole}.");
            }
        }
    }
}
