using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class ParameterDigimonDataPatch
{
    [HarmonyPatch(typeof(ParameterDigimonData), "FindBaseIdToModelName")]
    [HarmonyPrefix]
    private static bool ParameterDigimonData_FindBaseIdToModelName_Prefix(string modelName, ref uint __result)
    {
        __result = ParameterManagerLib.DigimonDataList.Find(x => x.m_mdlName == modelName).m_id;
        return false;
    }
}
