using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace InstantFishing;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.InstantFishing";
    internal const string PluginName = "InstantFishing";
    internal const string PluginVersion = "1.0.0";

    public static ConfigEntry<bool> NoMiss;

    public override void Load()
    {
        NoMiss = Config.Bind("#General", "NoMiss", false, "Prevent \"Could not fish!\" if true.");
        Harmony.CreateAndPatchAll(typeof(Plugin));
    }

    [HarmonyPatch(typeof(MainGameFishing), "Activate")]
    [HarmonyPrefix]
    public static void MainGameFishing_Activate_Prefix()
    {
        StorageData.m_playerData.SetTutorialAlreadyFlag(PlayerData.TutorialAlreadyFlags.Fishing00, true);
        StorageData.m_playerData.SetTutorialAlreadyFlag(PlayerData.TutorialAlreadyFlags.Fishing01, true);
    }

    [HarmonyPatch(typeof(uFishingPanel), "Update")]
    [HarmonyPrefix]
    public static bool uFishingPanel_Update_Prefix(uFishingPanel __instance)
    {
        if (__instance.isOpened)
        {
            if (__instance.m_step == 1)
            {
                int missing_chance = 50;
                if (ParameterTamerSkill.IsLearnedSkill(ParameterTamerSkill.TamerSkill.FISHERMAN))
                    missing_chance -= 20;
                if (ParameterTamerSkill.IsLearnedSkill(ParameterTamerSkill.TamerSkill.INGREDIENTS_HUNTER))
                    missing_chance -= 20;
                if (NoMiss.Value)
                    missing_chance = -1;

                int num = Random.Range(0, 100);
                if (num >= missing_chance)
                {
                    __instance.m_hitNum = 1;
                    if (ParameterTamerSkill.IsLearnedSkill(ParameterTamerSkill.TamerSkill.GURANDA))
                    {
                        if (num < 60)
                            __instance.m_hitNum = 1;
                        else if (num < 85)
                            __instance.m_hitNum = 2;
                        else if (num < 96)
                            __instance.m_hitNum = 3;
                        else if (num < 99)
                            __instance.m_hitNum = 4;
                        else
                            __instance.m_hitNum = 5;
                    }
                    while (__instance.m_hitItem == -1)
                        __instance.m_hitItem = __instance.fishTrigger.GetItemId((int)__instance.m_lure.lureResult, num);
                }
                else
                {
                    __instance.m_hitNum = 0;
                    __instance.m_hitItem = -1;
                }

                __instance.m_step = 4;
                return false;
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(uFishingPanel), "UpdateFishingRod")]
    [HarmonyPrefix]
    public static bool uFishingPanel_UpdateFishingRod_Prefix() => false;
}
