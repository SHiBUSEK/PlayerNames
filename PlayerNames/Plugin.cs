using System;
using Exiled.API.Features;

namespace PlayerNames
{
    public class PlayerNames : Plugin<Config>
    {
        public override string Name => "PlayerNames";
        public override string Author => "Shibusek";
        public override Version Version => new Version(0, 0, 1);

        public override void OnEnabled()
        {
            Log.Info("Thanks for using PlayerNames by Shibusek");
            base.OnEnabled();
        }
     }
 }
