using DWNOLib.Library;
using HarmonyLib;
using static uDigiviceBG;

namespace DWNOLib.Patches;
internal class HashIdSearchClassPatch
{
    [HarmonyPatch(typeof(HashIdSearchClass<ParameterDigimonData>), "GetParam")]
    [HarmonyPrefix]
    private static bool HashIdSearchClass_GetParam_Prefix(Csvb<ParameterDigimonData> data, uint _id, ref object __result)
    {
        if (data.Pointer == AppMainScript.parameterManager.digimonData.Pointer)
        {
            ParameterDigimonData @params = ParameterManagerLib.DigimonDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.usableSkillData.Pointer)
        {
            ParameterUsableSkillData @params = ParameterManagerLib.UsableSkillList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemData.Pointer)
        {
            ParameterItemData @params = ParameterManagerLib.ItemDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == ParameterManagerPointer.DigiviceSoloCameraPointer)
        {
            DigiviceSoloCameraData @params = ParameterManagerLib.SoloCameraDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == ParameterManagerPointer.PlacementNPCPointer)
        {
            ParameterPlacementNpc @params = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        return true;
    }
}
