using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace BlockAllEvoPaths;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.BlockAllEvoPaths";
    internal const string PluginName = "BlockAllEvoPaths";
    internal const string PluginVersion = "1.0.0";

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(uGenelogyUI), "InputOK")]
    [HarmonyPrefix]
    public static bool uGenelogyUI_InputOK_Prefix(uGenelogyUI __instance)
    {
        uCommonMessageWindow center = MainGameManager.Ref.MessageManager.GetCenter();
        if (__instance.m_Type != uGenelogyUI.Type.Dojo_Block || __instance.m_DialogOpen || center.IsOpened() || __instance.m_CursorX != __instance.m_CursorX_Max - 1 || !__instance.m_EvolutionDigimon)
        {
            return false;
        }
        int num = 0;
        bool flag = false;
        for (int i = 0; i < __instance.m_CursorY_Max; i++)
        {
            ParameterDigimonData param = ParameterDigimonData.GetParam(__instance.m_EvoDigimons[i]);
            if (param == null)
            {
                continue;
            }
            if (StorageData.m_ScenarioProgressData.IsOnFlagSetAnd(param.m_evo1_flagset, true))
            {
                if (!StorageData.saveData.m_playerData.m_partners[__instance.m_SelectPartner].m_EvolutionBlock[(uint)i])
                {
                    num++;
                }
            }
            else if (__instance.m_CursorY == i)
            {
                flag = true;
            }
        }
        if (flag)
        {
            CriSoundManager.PlayCommonSe("S_008");
        }
        else
        {
            StorageData.saveData.m_playerData.m_partners[__instance.m_SelectPartner].m_EvolutionBlock[(uint)__instance.m_CursorY] = !StorageData.saveData.m_playerData.m_partners[__instance.m_SelectPartner].m_EvolutionBlock[(uint)__instance.m_CursorY];
            __instance.EnableRockObject(__instance.m_CursorY, StorageData.saveData.m_playerData.m_partners[__instance.m_SelectPartner].m_EvolutionBlock[(uint)__instance.m_CursorY]);
            if (!StorageData.saveData.m_playerData.m_partners[__instance.m_SelectPartner].m_EvolutionBlock[(uint)__instance.m_CursorY])
            {
                __instance.m_RockNum--;
            }
            CriSoundManager.PlayCommonSe("S_007");
        }
        return false;
    }
}
