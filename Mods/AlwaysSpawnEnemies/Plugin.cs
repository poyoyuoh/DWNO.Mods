using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace AlwaysSpawnEnemies;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.AlwaysSpawnEnemies";
    internal const string PluginName = "AlwaysSpawnEnemies";
    internal const string PluginVersion = "1.0.0";

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(EnemyManager), "_IsCreateNormalEnamyIncidence")]
    [HarmonyPrefix]
    public static bool EnemyManager__IsCreateNormalEnamyIncidence_Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}
