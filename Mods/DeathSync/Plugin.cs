using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace DeathSync;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "DeathSync";
    internal const string PluginName = "DeathSync";
    internal const string PluginVersion = "1.0.0";

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(MainGameDigitama), "StartEvent")]
    [HarmonyPrefix]
    public static void MainGameDigitama_StartEvent_Prefix(int no)
    {
        if (no == 0)
        {
            if (StorageData.saveData.m_playerData.m_partners[0].m_lifetime <= 0f)
                StorageData.saveData.m_playerData.m_partners[1].m_lifetime = 0f;
            else if (StorageData.saveData.m_playerData.m_partners[1].m_lifetime <= 0f)
                StorageData.saveData.m_playerData.m_partners[0].m_lifetime = 0f;
        }
    }
}
