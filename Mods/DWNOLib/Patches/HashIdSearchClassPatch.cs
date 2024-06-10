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

        if (data.Pointer == AppMainScript.parameterManager.itemDataBattle.Pointer)
        {
            ParameterItemDataBattle @params = ParameterManagerLib.ItemBattleDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemDataEvolution.Pointer)
        {
            ParameterItemDataEvolution @params = ParameterManagerLib.ItemEvolutionDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemDataOther.Pointer)
        {
            ParameterItemDataOther @params = ParameterManagerLib.ItemOtherDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemDataRecovery.Pointer)
        {
            ParameterItemDataRecovery @params = ParameterManagerLib.ItemRecoveryDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemDataFood.Pointer)
        {
            ParameterItemDataFood @params = ParameterManagerLib.ItemFoodDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemDataMaterial.Pointer)
        {
            ParameterItemDataMaterial @params = ParameterManagerLib.ItemMaterialDataList.Find(x => x.m_id == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        if (data.Pointer == AppMainScript.parameterManager.itemDataKeyItem.Pointer)
        {
            ParameterItemDataKeyItem @params = ParameterManagerLib.ItemKeyItemDataList.Find(x => x.m_id == _id);
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

        if (data.Pointer == ParameterManagerPointer.NPCEnemyPointer)
        {
            ParameterNpcEnemyData @params = ParameterManagerLib.NPCEnemyDataList.Find(x => x.m_ParamId == _id);
            if (@params != null)
            {
                __result = @params;
                return false;
            }
        }

        return true;
    }
}
