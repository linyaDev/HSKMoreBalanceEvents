using System;
using System.Reflection;
using RimWorld;
using HarmonyLib;
using Verse;

namespace HSKMoreBalanceEvents
{
    [StaticConstructorOnStartup]
    public static class IgnoranceCompat
    {
        private static readonly Func<TechLevel> getPlayerTech;
        private static readonly Func<TechLevel, bool> techIsEligible;
        private static readonly Func<Faction, bool> factionIsEligible;

        public static bool Active { get; }

        static IgnoranceCompat()
        {
            var t = AccessTools.TypeByName("DIgnoranceIsBliss.Core_Patches.IgnoranceBase");
            if (t == null)
                return;

            getPlayerTech = CreateDelegate<Func<TechLevel>>(AccessTools.PropertyGetter(t, "PlayerTechLevel"));
            techIsEligible = CreateDelegate<Func<TechLevel, bool>>(AccessTools.Method(t, "TechIsEligibleForIncident"));
            factionIsEligible = CreateDelegate<Func<Faction, bool>>(AccessTools.Method(t, "FactionInEligibleTechRange"));

            Active = getPlayerTech != null && techIsEligible != null && factionIsEligible != null;
            if (!Active)
                Log.Warning("[HSKMoreBalanceEvents] IgnoranceCompat: Ignorance Is Bliss найден, но его API изменилось — связка отключена.");
        }

        public static TechLevel PlayerTechLevel
        {
            get
            {
                if (Current.Game == null)
                    return TechLevel.Undefined;
                if (Active)
                    return getPlayerTech();
                return Faction.OfPlayer.def.techLevel;
            }
        }

        public static bool TechIsEligible(TechLevel tech)
        {
            return !Active || Current.Game == null || techIsEligible(tech);
        }

        public static bool FactionIsEligible(Faction faction)
        {
            if (faction == null)
                return true;
            return !Active || Current.Game == null || factionIsEligible(faction);
        }

        private static T CreateDelegate<T>(MethodInfo method) where T : Delegate
        {
            if (method == null)
                return null;
            try
            {
                return (T)method.CreateDelegate(typeof(T));
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
