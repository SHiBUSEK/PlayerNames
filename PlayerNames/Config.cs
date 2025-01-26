using System.Collections.Generic;
using Exiled.API.Interfaces;

namespace PlayerNames
{
    public class Config : IConfig
    {
        /// <summary>
        /// Enables or disables the plugin.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Enables debug mode (more detailed logs in the server console).
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        /// If true, Class D players will receive random D-XXXX numbers instead of nicknames.
        /// </summary>
        public bool DNumbers { get; set; } = true;

        /// <summary>
        /// A dictionary containing nicknames for each default role in SCP:SL.
        /// </summary>
        public Dictionary<string, List<string>> RoleNicknames { get; set; } = new Dictionary<string, List<string>>
        {
            { "ClassD", new List<string> { "John", "Jake", "Mike", "Steve", "Ryan" } },
            { "Scientist", new List<string> { "Dr. Smith", "Dr. Johnson", "Dr. Brown", "Dr. Carter", "Dr. Blake" } },
            { "FacilityGuard", new List<string> { "Guard Alpha", "Guard Bravo", "Guard Charlie", "Guard Delta" } },
            { "NtfPrivate", new List<string> { "Private Axel", "Private Blaze", "Private Ghost", "Private Hawk" } },
            { "NtfSergeant", new List<string> { "Sergeant Shadow", "Sergeant Steel", "Sergeant Wolf" } },
            { "NtfCaptain", new List<string> { "Captain Falcon", "Captain Frost", "Captain Viper" } },
            { "NtfSpecialist", new List<string> { "Specialist Echo", "Specialist Bravo", "Specialist Zulu" } },
            { "ChaosConscript", new List<string> { "Conscript Reaper", "Conscript Viper", "Conscript Echo" } },
            { "ChaosRifleman", new List<string> { "Rifleman Tango", "Rifleman Bravo", "Rifleman Shadow" } },
            { "ChaosRepressor", new List<string> { "Repressor Omega", "Repressor Delta", "Repressor Nova" } },
            { "ChaosMarauder", new List<string> { "Marauder Phantom", "Marauder Blaze", "Marauder Fang" } },
            { "Scp049", new List<string> { "Plague Doctor", "Dr. Death" } },
            { "Scp0492", new List<string> { "Zombie A", "Zombie B", "Zombie C", "Zombie D" } },
            { "Scp079", new List<string> { "AI Overlord", "HAL9000", "NeuralNet" } },
            { "Scp096", new List<string> { "Shy Guy", "Rage Monster" } },
            { "Scp106", new List<string> { "Larry", "Old Man" } },
            { "Scp173", new List<string> { "Sculpture", "Peanut", "Statue" } },
            { "Scp939", new List<string> { "Doggo", "Hunter", "Predator" } },
            { "Tutorial", new List<string> { "Guide", "Mentor", "Helper" } }
        };
    }
}
