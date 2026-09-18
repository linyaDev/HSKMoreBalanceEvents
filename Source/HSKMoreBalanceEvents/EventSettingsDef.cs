using System.Collections.Generic;
using RimWorld;
using Verse;

namespace HSKMoreBalanceEvents
{
    // Пороги гейтов событий, задаются в Defs/Misc/EventSettings.xml.
    // Если дефа нет, каждый патч берёт свои запасные значения из кода.
    public class EventSettingsDef : Def
    {
        // Событие (IncidentDef defName) -> минимальный техуровень игрока
        public Dictionary<string, TechLevel> incidentMinTechLevel;
        // Вариант жуткого присоединившегося (CreepJoinerFormKindDef defName) ->
        // минимальный техуровень игрока
        public Dictionary<string, TechLevel> creepJoinerFormMinTechLevel;
        // Гости Hospitality: на сколько уровней фракция может быть выше / ниже
        // игрока (-1 = без ограничения)
        public int guestMaxTechAhead = -1;
        public int guestMaxTechBehind = -1;

        private static EventSettingsDef cachedInstance;

        public static EventSettingsDef Instance
        {
            get
            {
                if (cachedInstance == null)
                    cachedInstance = DefDatabase<EventSettingsDef>.GetNamedSilentFail("HSKMoreBalanceEvents_Settings");
                return cachedInstance;
            }
        }

        // Значения на случай отсутствия дефа
        public static readonly Dictionary<string, TechLevel> defaultIncidentMinTechLevel = new Dictionary<string, TechLevel>
        {
            { "LongNight", TechLevel.Industrial },
            { "IceAge", TechLevel.Industrial },
            { "MechanoidTerraformerIncident", TechLevel.Spacer },
        };

        public static readonly Dictionary<string, TechLevel> defaultCreepJoinerFormMinTechLevel = new Dictionary<string, TechLevel>
        {
            { "LoneGenius", TechLevel.Industrial },
        };

        public const int defaultGuestMaxTechAhead = 0;
        public const int defaultGuestMaxTechBehind = 2;

        public static Dictionary<string, TechLevel> IncidentGates =>
            Instance?.incidentMinTechLevel ?? defaultIncidentMinTechLevel;

        public static Dictionary<string, TechLevel> CreepJoinerGates =>
            Instance?.creepJoinerFormMinTechLevel ?? defaultCreepJoinerFormMinTechLevel;

        public static int GuestMaxTechAhead =>
            Instance?.guestMaxTechAhead ?? defaultGuestMaxTechAhead;

        public static int GuestMaxTechBehind =>
            Instance?.guestMaxTechBehind ?? defaultGuestMaxTechBehind;
    }
}
