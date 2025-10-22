using HarmonyLib;
using System.Collections.Generic;
using TeamCherry.Localization;

namespace InfinityCrest.Patches.Localization;

[HarmonyPatch(typeof(Language), nameof(Language.SwitchLanguage), typeof(LanguageCode))]
public static class Localization
{
    [HarmonyPostfix]
    private static void AddNewSheet()
    {
        Dictionary<string, Dictionary<string, string>> fullStore = Language._currentEntrySheets;

        if (!fullStore.ContainsKey("INFINITY"))
        {
            fullStore.Add("INFINITY", new Dictionary<string, string>()
            {
                { "INFINITYCRESTNAME", "Infinity" },
                { "INFINITYCRESTDESC", "Wield the full power of Pharloom. Equip every blue and yellow tool at once." }
            });
        }

        Language._currentEntrySheets = fullStore;
    }
}