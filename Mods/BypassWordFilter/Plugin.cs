using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace BypassWordFilter;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.BypassWordFilter";
    internal const string PluginName = "BypassWordFilter";
    internal const string PluginVersion = "1.0.0";

    public override void Load() =>
        Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(MainTitle), "Update")]
    [HarmonyPostfix]
    public static void MainTitle_Update_Postfix(MainTitle __instance)
    {
        if (__instance.m_State == MainTitle.State.NameEntry)
        {
            if (__instance.m_name_entry != null)
            {
                if (__instance.m_name_entry.m_bl != null)
                    if (__instance.m_name_entry.m_bl.Count != 0)
                        __instance.m_name_entry.m_bl = new List<string>();

                if (__instance.m_name_entry.m_wl != null)
                    if (__instance.m_name_entry.m_wl.Count != 0)
                        __instance.m_name_entry.m_wl = new List<string>();
            }
        }
    }

    [HarmonyPatch(typeof(DigitamaScene), "Update")]
    [HarmonyPostfix]
    public static void DigitamaScene_Update_Postfix(DigitamaScene __instance)
    {
        if (__instance.m_sceneNo == DigitamaScene.SceneNo.NameEntry)
        {
            if (__instance.m_nameEntry != null)
            {
                if (__instance.m_nameEntry.m_bl != null)
                    if (__instance.m_nameEntry.m_bl.Count != 0)
                        __instance.m_nameEntry.m_bl = new List<string>();

                if (__instance.m_nameEntry.m_wl != null)
                    if (__instance.m_nameEntry.m_wl.Count != 0)
                        __instance.m_nameEntry.m_wl = new List<string>();
            }
        }
    }
}
