using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class CsvbPatch
{
    [HarmonyPatch(typeof(Csvb<ParameterDigimonData>), "GetRecordMax")]
    [HarmonyPrefix]
    private static bool Csvb_GetRecordMax_Prefix(Csvb<ParameterDigimonData> __instance, ref int __result)
    {
        if (__instance.Pointer == ParameterManagerPointer.PlacementEnemyPointer ||
            __instance.Pointer == ParameterManagerPointer.PlacementNPCPointer ||
            __instance.Pointer == ParameterManagerPointer.NPCEnemyPointer ||
            __instance.Pointer == AppMainScript.Ref.m_parameters.m_csvbShopItemData.Pointer)
        {
            if (__instance.m_params != null)
            {
                __result = __instance.m_params.Length;
                return false;
            }
        }

        return true;
    }

    [HarmonyPatch(typeof(Csvb<ParameterDigimonData>), "GetParams", new System.Type[] { typeof(int) })]
    [HarmonyPrefix]
    private static bool Csvb_GetParams_Prefix(Csvb<ParameterDigimonData> __instance, ref object __result, int recordIndex)
    {
        if (__instance.Pointer == ParameterManagerPointer.PlacementEnemyPointer ||
            __instance.Pointer == ParameterManagerPointer.PlacementNPCPointer ||
            __instance.Pointer == ParameterManagerPointer.NPCEnemyPointer ||
            __instance.Pointer == AppMainScript.Ref.m_parameters.m_csvbShopItemData.Pointer)
        {
            if (__instance.m_params != null)
            {
                __result = __instance.m_params[recordIndex];
                return false;
            }
        }

        return true;
    }
}
