using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace TamerExpMultiplier;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.TamerExpMultiplier";
    internal const string PluginName = "TamerExpMultiplier";
    internal const string PluginVersion = "1.0.0";

    public static ConfigEntry<float> Multiplier;

    public override void Load()
    {
        Multiplier = Config.Bind("#General", "Multiplier", 5.0f, "Multiply the tamer earned after a battle.");
        Harmony.CreateAndPatchAll(typeof(Plugin));
    }

    [HarmonyPatch(typeof(PlayerData), "AddExpBattle")]
    [HarmonyPrefix]
    public static bool PlayerData_AddExpBattle_Prefix(PlayerData __instance, int growth, int level, bool kingSize = false)
    {
        int expBattle = __instance.GetExpBattle(growth, level, kingSize);
        __instance.AddExp(expBattle);
        return false;
    }

    [HarmonyPatch(typeof(PlayerData), "GetExpBattle")]
    [HarmonyPostfix]
    public static void PlayerData_GetExpBattle_Postfix(ref int __result) => __result = (int)(__result * Multiplier.Value);
}
