using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static uDigiviceBG;

namespace DWNOLib.Library;
public class ParameterManagerLoader
{
    public static bool is_loaded { get; private set; } = false;

    public static void LoadParameters(ParameterManager m_parameters)
    {
        if (is_loaded)
        {
            Logger.Log("Parameters are already loaded.", Logger.LogType.Error);
            return;
        }

        LoadDigimonDatas(m_parameters);
        LoadUsableSkillDatas(m_parameters);
        LoadCommonSelectWindowDatas(m_parameters);
        LoadShopItemDatas(m_parameters);

        foreach (ParameterDigimonData data in ParameterManagerLib.CustomDigimonDatas)
        {
            if (data.m_partnerFlag == 1)
                m_parameters.m_DigimonID_LIST.Add(data.m_id);
        }    

        ParameterManagerLib.ItemDataList = m_parameters.itemData.m_params.ToList();

        is_loaded = true;
    }

    private static void LoadDigimonDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.DigimonDataList = m_parameters.m_csvbDigimonData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomDigimonDatas.Count; i++)
            ParameterManagerLib.DigimonDataList.Add(ParameterManagerLib.CustomDigimonDatas[i]);

        Il2CppReferenceArray<ParameterDigimonData> RefArray = new Il2CppReferenceArray<ParameterDigimonData>(ParameterManagerLib.DigimonDataList.ToArray());
        Il2CppArrayBase<ParameterDigimonData> ArrayBase = Il2CppArrayBase<ParameterDigimonData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbDigimonData.m_params = ArrayBase;
    }

    private static void LoadUsableSkillDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.UsableSkillList = m_parameters.m_csvbUsableSkillData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomUsableSkillDatas.Count; i++)
            ParameterManagerLib.UsableSkillList.Add(ParameterManagerLib.CustomUsableSkillDatas[i]);

        Il2CppReferenceArray<ParameterUsableSkillData> RefArray = new Il2CppReferenceArray<ParameterUsableSkillData>(ParameterManagerLib.UsableSkillList.ToArray());
        Il2CppArrayBase<ParameterUsableSkillData> ArrayBase = Il2CppArrayBase<ParameterUsableSkillData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbUsableSkillData.m_params = ArrayBase;
    }

    public static void LoadDigiviceSoloCameraDatas(uDigiviceBG __instance)
    {
        ParameterManagerLib.SoloCameraDataList.Clear();
        ParameterManagerLib.SoloCameraDataList = __instance.m_soloCameraData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomSoloCameraDatas.Count; i++)
            ParameterManagerLib.SoloCameraDataList.Add(ParameterManagerLib.CustomSoloCameraDatas[i]);

        Il2CppReferenceArray<DigiviceSoloCameraData> RefArray = new Il2CppReferenceArray<DigiviceSoloCameraData>(ParameterManagerLib.SoloCameraDataList.ToArray());
        Il2CppArrayBase<DigiviceSoloCameraData> ArrayBase = Il2CppArrayBase<DigiviceSoloCameraData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        __instance.m_soloCameraData.m_params = ArrayBase;

        ParameterManagerPointer.DigiviceSoloCameraPointer = __instance.m_soloCameraData.Pointer;
    }

    public static bool LoadEnemyPlacementDatas(bool isEndLoad, int _mapNo, int _areaNo, bool __result, MainGameManager __instance)
    {
        Vector2 key = new Vector2(_mapNo, _areaNo);
        if (__result == false)
        {
            if (ParameterManagerLib.CustomPlacementEnemyDatas.ContainsKey(key))
            {
                Il2CppReferenceArray<ParameterPlacementEnemy> RefArray2 = new Il2CppReferenceArray<ParameterPlacementEnemy>(ParameterManagerLib.CustomPlacementEnemyDatas[key].ToArray());
                Il2CppArrayBase<ParameterPlacementEnemy> ArrayBase2 = Il2CppArrayBase<ParameterPlacementEnemy>.WrapNativeGenericArrayPointer(RefArray2.Pointer);
                Csvb<ParameterPlacementEnemy> csvb = new Csvb<ParameterPlacementEnemy>();
                csvb.m_params = ArrayBase2;
                __instance.m_PlacementDataEnemy = csvb;
                ParameterManagerPointer.PlacementEnemyPointer = __instance.m_PlacementDataEnemy.Pointer;
                return true;
            }
        }
        else if (isEndLoad && ParameterManagerLib.CustomPlacementEnemyDatas.ContainsKey(key))
        {
            ParameterManagerLib.PlacementEnemyDataList = __instance.m_PlacementDataEnemy.m_params.ToList();

            for (int i = 0; i < ParameterManagerLib.CustomPlacementEnemyDatas[key].Count; i++)
                ParameterManagerLib.PlacementEnemyDataList.Add(ParameterManagerLib.CustomPlacementEnemyDatas[key][i]);

            Il2CppReferenceArray<ParameterPlacementEnemy> RefArray = new Il2CppReferenceArray<ParameterPlacementEnemy>(ParameterManagerLib.PlacementEnemyDataList.ToArray());
            Il2CppArrayBase<ParameterPlacementEnemy> ArrayBase = Il2CppArrayBase<ParameterPlacementEnemy>.WrapNativeGenericArrayPointer(RefArray.Pointer);
            __instance.m_PlacementDataEnemy.m_params = ArrayBase;
            ParameterManagerPointer.PlacementEnemyPointer = __instance.m_PlacementDataEnemy.Pointer;
            ParameterManagerLib.PlacementEnemyDataList = __instance.m_PlacementDataEnemy.m_params.ToList();
            return true;
        }
        return __result;
    }

    public static bool LoadNPCPlacementDatas(bool isEndLoad, int _mapNo, int _areaNo, bool __result, MainGameManager __instance)
    {
        Vector2 key = new Vector2(_mapNo, _areaNo);
        if (__result == false)
        {
            if (ParameterManagerLib.CustomPlacementNPCDatas.ContainsKey(key))
            {
                Il2CppReferenceArray<ParameterPlacementNpc> RefArray2 = new Il2CppReferenceArray<ParameterPlacementNpc>(ParameterManagerLib.CustomPlacementNPCDatas[key].ToArray());
                Il2CppArrayBase<ParameterPlacementNpc> ArrayBase2 = Il2CppArrayBase<ParameterPlacementNpc>.WrapNativeGenericArrayPointer(RefArray2.Pointer);
                Csvb<ParameterPlacementNpc> csvb = new Csvb<ParameterPlacementNpc>();
                csvb.m_params = ArrayBase2;
                __instance.m_PlacementDataNpc = csvb;
                ParameterManagerPointer.PlacementNPCPointer = __instance.m_PlacementDataNpc.Pointer;
                ParameterManagerLib.PlacementNPCDataList = __instance.m_PlacementDataNpc.m_params.ToList();
                return true;
            }
        }
        else if (isEndLoad && ParameterManagerLib.CustomPlacementNPCDatas.ContainsKey(key))
        {
            ParameterManagerLib.PlacementNPCDataList = __instance.m_PlacementDataNpc.m_params.ToList();

            for (int i = 0; i < ParameterManagerLib.CustomPlacementNPCDatas[key].Count; i++)
                ParameterManagerLib.PlacementNPCDataList.Add(ParameterManagerLib.CustomPlacementNPCDatas[key][i]);

            Il2CppReferenceArray<ParameterPlacementNpc> RefArray = new Il2CppReferenceArray<ParameterPlacementNpc>(ParameterManagerLib.PlacementNPCDataList.ToArray());
            Il2CppArrayBase<ParameterPlacementNpc> ArrayBase = Il2CppArrayBase<ParameterPlacementNpc>.WrapNativeGenericArrayPointer(RefArray.Pointer);
            __instance.m_PlacementDataNpc.m_params = ArrayBase;
            ParameterManagerPointer.PlacementNPCPointer = __instance.m_PlacementDataNpc.Pointer;
            return true;
        }
        return __result;
    }

    private static void LoadCommonSelectWindowDatas(ParameterManager m_parameters)
    {
        List<Csvb<ParameterCommonSelectWindow>> CommonSelectWindowCsvbList = m_parameters.m_csvbCommonSelectWindowData.ToList();

        foreach (KeyValuePair<int, List<ParameterCommonSelectWindow>> list in ParameterManagerLib.CustomCommonSelectWindowDatas)
        {
            if (CommonSelectWindowCsvbList.Count > list.Key && CommonSelectWindowCsvbList[list.Key] != null)
            {
                List<ParameterCommonSelectWindow> @params = CommonSelectWindowCsvbList[list.Key].m_params.ToList();

                for (int i = 0; i < list.Value.Count; i++)
                    @params.Add(list.Value[i]);

                Il2CppReferenceArray<ParameterCommonSelectWindow> RefArray2 = new Il2CppReferenceArray<ParameterCommonSelectWindow>(@params.ToArray());
                Il2CppArrayBase<ParameterCommonSelectWindow> ArrayBase2 = Il2CppArrayBase<ParameterCommonSelectWindow>.WrapNativeGenericArrayPointer(RefArray2.Pointer);
                CommonSelectWindowCsvbList[list.Key].m_params = ArrayBase2;
            }
            else
            {
                Csvb<ParameterCommonSelectWindow> csvb = new Csvb<ParameterCommonSelectWindow>();

                Il2CppReferenceArray<ParameterCommonSelectWindow> RefArray2 = new Il2CppReferenceArray<ParameterCommonSelectWindow>(list.Value.ToArray());
                Il2CppArrayBase<ParameterCommonSelectWindow> ArrayBase2 = Il2CppArrayBase<ParameterCommonSelectWindow>.WrapNativeGenericArrayPointer(RefArray2.Pointer);
                csvb.m_params = ArrayBase2;

                CommonSelectWindowCsvbList.Add(csvb);
            }
        }

        Il2CppReferenceArray<Csvb<ParameterCommonSelectWindow>> RefArray = new Il2CppReferenceArray<Csvb<ParameterCommonSelectWindow>>(CommonSelectWindowCsvbList.ToArray());
        m_parameters.m_csvbCommonSelectWindowData = RefArray;
    }

    private static void LoadShopItemDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ShopItemList = m_parameters.m_csvbShopItemData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomShopItemDatas.Count; i++)
            ParameterManagerLib.ShopItemList.Add(ParameterManagerLib.CustomShopItemDatas[i]);

        Il2CppReferenceArray<ParameterShopItemData> RefArray = new Il2CppReferenceArray<ParameterShopItemData>(ParameterManagerLib.ShopItemList.ToArray());
        Il2CppArrayBase<ParameterShopItemData> ArrayBase = Il2CppArrayBase<ParameterShopItemData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbShopItemData.m_params = ArrayBase;
    }
}
