using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class ParameterDigimonDataPatch
{
    [HarmonyPatch(typeof(ParameterDigimonData), "FindBaseIdToModelName")]
    [HarmonyPrefix]
    private static bool ParameterDigimonData_FindBaseIdToModelName_Prefix(string modelName, ref uint __result)
    {
        ParameterDigimonData @params = ParameterManagerLib.DigimonDataList.Find(x => x.m_mdlName == modelName);
        if (@params == null)
            return true;

        __result = @params.m_id;
        return false;
    }
}
