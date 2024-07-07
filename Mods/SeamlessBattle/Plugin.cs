using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace SeamlessBattle;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.SeamlessBattle";
    internal const string PluginName = "SeamlessBattle";
    internal const string PluginVersion = "1.0.0";

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(uBattlePanel), "Update")]
    [HarmonyPostfix]
    public static void uBattlePanel_Update_Postfix(uBattlePanel __instance)
    {
        if (__instance.m_isTactics)
            __instance.DrawTargetArrow();
        
        if (__instance.m_isOpenCmd)
            MainGameManager.Ref.TimeScale = 1f;
    }

    [HarmonyPatch(typeof(uBattlePanel), "EnableTacticsMode")]
    [HarmonyPostfix]
    public static void uBattlePanel_EnableTacticsMode_Postfix(uBattlePanel __instance)
    {
        MainGameManager.Ref.TimeScale = 1f;
        __instance.VisibleDamage = true;
    }

    [HarmonyPatch(typeof(uBattlePanel), "EnableItemBoxMode")]
    [HarmonyPostfix]
    public static void uBattlePanel_EnableItemBoxMode_Postfix(uBattlePanel __instance)
    {
        MainGameManager.Ref.TimeScale = 1f;
        __instance.VisibleDamage = true;
    }

    [HarmonyPatch(typeof(uBattlePanelCommand), "CommandExecution")]
    [HarmonyPostfix]
    public static void uBattlePanelCommand_CommandExecution_Postfix()
    {
        MainGameManager.Ref.TimeScale = 1f;
    }
}
