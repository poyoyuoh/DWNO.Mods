using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace SkipTutorials;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.SkipTutorials";
    internal const string PluginName = "SkipTutorials";
    internal const string PluginVersion = "1.0.0";

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(MainTitle), "Update")]
    [HarmonyPostfix]
    public static void MainTitle_Update_Postfix(MainTitle __instance)
    {
        if (__instance.m_State == MainTitle.State.NameEntry)
        {
            if (StorageData.m_mapData.m_mapNo == 99)
            {
                StorageData.m_mapData.m_mapNo = 1;
                StorageData.m_mapData.m_areaNo = 10;

                // Mugendramon tutorials
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(204694512, true);
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(204694524, true);
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(204694526, true);
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(221472107, true);

                // Nigh Plain tutorials (except the patamon one)
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(4189531208, true);
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(4189531213, true);
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(4189531214, true);
                StorageData.m_ScenarioProgressData.SetScenarioFlagByFlagSet(4189531215, true);

                // Mechanic tutorials
                StorageData.m_playerData.SetTutorialAlreadyFlag(PlayerData.TutorialAlreadyFlags.Training00, true);
                StorageData.m_playerData.SetTutorialAlreadyFlag(PlayerData.TutorialAlreadyFlags.TamerLv00, true);
                StorageData.m_playerData.SetTutorialAlreadyFlag(PlayerData.TutorialAlreadyFlags.Fishing00, true);
                StorageData.m_playerData.SetTutorialAlreadyFlag(PlayerData.TutorialAlreadyFlags.Fishing01, true);
            }
        }
    }
}
