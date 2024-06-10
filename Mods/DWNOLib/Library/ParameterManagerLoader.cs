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
        LoadItemDatas(m_parameters);
        LoadItemBattleDatas(m_parameters);
        LoadItemEvolutionDatas(m_parameters);
        LoadItemOtherDatas(m_parameters);
        LoadItemRecoveryDatas(m_parameters);
        LoadItemFoodDatas(m_parameters);
        LoadItemMaterialDatas(m_parameters);
        LoadItemKeyItemDatas(m_parameters);
        LoadShopItemDatas(m_parameters);
        LoadJoglessDatas(m_parameters);
        LoadJoglessGroupDatas(m_parameters);

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
        else if (isEndLoad)
        {
            ParameterManagerLib.PlacementEnemyDataList = __instance.m_PlacementDataEnemy.m_params.ToList();

            if (ParameterManagerLib.CustomPlacementEnemyDatas.ContainsKey(key))
            {
                for (int i = 0; i < ParameterManagerLib.CustomPlacementEnemyDatas[key].Count; i++)
                    ParameterManagerLib.PlacementEnemyDataList.Add(ParameterManagerLib.CustomPlacementEnemyDatas[key][i]);
            }

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
        else if (isEndLoad)
        {
            ParameterManagerLib.PlacementNPCDataList = __instance.m_PlacementDataNpc.m_params.ToList();

            if (ParameterManagerLib.CustomPlacementNPCDatas.ContainsKey(key))
            {
                for (int i = 0; i < ParameterManagerLib.CustomPlacementNPCDatas[key].Count; i++)
                    ParameterManagerLib.PlacementNPCDataList.Add(ParameterManagerLib.CustomPlacementNPCDatas[key][i]);
            }

            //ParameterPlacementNpc @params = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == 0x2fc0dec2);
            //if (@params != null)
            //    ParameterManagerLib.PlacementNPCDataList.Remove(@params);
            //@params = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == 0xb4ffeb05);
            //if (@params != null)
            //    ParameterManagerLib.PlacementNPCDataList.Remove(@params);
            //@params = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == 0x72e4259d);
            //if (@params != null)
            //    ParameterManagerLib.PlacementNPCDataList.Remove(@params);

            Il2CppReferenceArray<ParameterPlacementNpc> RefArray = new Il2CppReferenceArray<ParameterPlacementNpc>(ParameterManagerLib.PlacementNPCDataList.ToArray());
            Il2CppArrayBase<ParameterPlacementNpc> ArrayBase = Il2CppArrayBase<ParameterPlacementNpc>.WrapNativeGenericArrayPointer(RefArray.Pointer);
            __instance.m_PlacementDataNpc.m_params = ArrayBase;
            ParameterManagerPointer.PlacementNPCPointer = __instance.m_PlacementDataNpc.Pointer;
            return true;
        }
        return __result;
    }

    public static bool LoadNPCEnemyDatas(bool isEndLoad, int _mapNo, int _areaNo, bool __result, MainGameManager __instance)
    {
        Vector2 key = new Vector2(_mapNo, _areaNo);
        if (__result == false)
        {
            if (ParameterManagerLib.CustomNPCEnemyDatas.ContainsKey(key))
            {
                Il2CppReferenceArray<ParameterNpcEnemyData> RefArray2 = new Il2CppReferenceArray<ParameterNpcEnemyData>(ParameterManagerLib.CustomNPCEnemyDatas[key].ToArray());
                Il2CppArrayBase<ParameterNpcEnemyData> ArrayBase2 = Il2CppArrayBase<ParameterNpcEnemyData>.WrapNativeGenericArrayPointer(RefArray2.Pointer);
                Csvb<ParameterNpcEnemyData> csvb = new Csvb<ParameterNpcEnemyData>();
                csvb.m_params = ArrayBase2;
                __instance.m_NpcEnemyData = csvb;
                ParameterManagerPointer.NPCEnemyPointer = __instance.m_NpcEnemyData.Pointer;
                ParameterManagerLib.NPCEnemyDataList = __instance.m_NpcEnemyData.m_params.ToList();
                return true;
            }
        }
        else if (isEndLoad && __instance.m_NpcEnemyData != null)
        {
            ParameterManagerLib.NPCEnemyDataList = __instance.m_NpcEnemyData.m_params.ToList();

            if (ParameterManagerLib.CustomNPCEnemyDatas.ContainsKey(key))
            {
                for (int i = 0; i < ParameterManagerLib.CustomNPCEnemyDatas[key].Count; i++)
                    ParameterManagerLib.NPCEnemyDataList.Add(ParameterManagerLib.CustomNPCEnemyDatas[key][i]);
            }

            Il2CppReferenceArray<ParameterNpcEnemyData> RefArray = new Il2CppReferenceArray<ParameterNpcEnemyData>(ParameterManagerLib.NPCEnemyDataList.ToArray());
            Il2CppArrayBase<ParameterNpcEnemyData> ArrayBase = Il2CppArrayBase<ParameterNpcEnemyData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
            __instance.m_NpcEnemyData.m_params = ArrayBase;
            ParameterManagerPointer.NPCEnemyPointer = __instance.m_NpcEnemyData.Pointer;
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

    private static void LoadItemDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemDataList = m_parameters.m_csvbItemData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemDatas.Count; i++)
            ParameterManagerLib.ItemDataList.Add(ParameterManagerLib.CustomItemDatas[i]);

        Il2CppReferenceArray<ParameterItemData> RefArray = new Il2CppReferenceArray<ParameterItemData>(ParameterManagerLib.ItemDataList.ToArray());
        Il2CppArrayBase<ParameterItemData> ArrayBase = Il2CppArrayBase<ParameterItemData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemData.m_params = ArrayBase;
    }

    private static void LoadItemBattleDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemBattleDataList = m_parameters.m_csvbItemDataBattle.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemBattleDatas.Count; i++)
            ParameterManagerLib.ItemBattleDataList.Add(ParameterManagerLib.CustomItemBattleDatas[i]);

        Il2CppReferenceArray<ParameterItemDataBattle> RefArray = new Il2CppReferenceArray<ParameterItemDataBattle>(ParameterManagerLib.ItemBattleDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataBattle> ArrayBase = Il2CppArrayBase<ParameterItemDataBattle>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemDataBattle.m_params = ArrayBase;
    }

    private static void LoadItemEvolutionDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemEvolutionDataList = m_parameters.m_csvbItemDataEvolution.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemEvolutionDatas.Count; i++)
            ParameterManagerLib.ItemEvolutionDataList.Add(ParameterManagerLib.CustomItemEvolutionDatas[i]);

        Il2CppReferenceArray<ParameterItemDataEvolution> RefArray = new Il2CppReferenceArray<ParameterItemDataEvolution>(ParameterManagerLib.ItemEvolutionDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataEvolution> ArrayBase = Il2CppArrayBase<ParameterItemDataEvolution>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemDataEvolution.m_params = ArrayBase;
    }

    private static void LoadItemOtherDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemOtherDataList = m_parameters.m_csvbItemDataOther.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemOtherDatas.Count; i++)
            ParameterManagerLib.ItemOtherDataList.Add(ParameterManagerLib.CustomItemOtherDatas[i]);

        Il2CppReferenceArray<ParameterItemDataOther> RefArray = new Il2CppReferenceArray<ParameterItemDataOther>(ParameterManagerLib.ItemOtherDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataOther> ArrayBase = Il2CppArrayBase<ParameterItemDataOther>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemDataOther.m_params = ArrayBase;
    }

    private static void LoadItemRecoveryDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemRecoveryDataList = m_parameters.m_csvbItemDataRecovery.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemRecoveryDatas.Count; i++)
            ParameterManagerLib.ItemRecoveryDataList.Add(ParameterManagerLib.CustomItemRecoveryDatas[i]);

        Il2CppReferenceArray<ParameterItemDataRecovery> RefArray = new Il2CppReferenceArray<ParameterItemDataRecovery>(ParameterManagerLib.ItemRecoveryDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataRecovery> ArrayBase = Il2CppArrayBase<ParameterItemDataRecovery>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemDataRecovery.m_params = ArrayBase;
    }

    private static void LoadItemFoodDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemFoodDataList = m_parameters.m_csvbItemDataFood.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemFoodDatas.Count; i++)
            ParameterManagerLib.ItemFoodDataList.Add(ParameterManagerLib.CustomItemFoodDatas[i]);

        Il2CppReferenceArray<ParameterItemDataFood> RefArray = new Il2CppReferenceArray<ParameterItemDataFood>(ParameterManagerLib.ItemFoodDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataFood> ArrayBase = Il2CppArrayBase<ParameterItemDataFood>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemDataFood.m_params = ArrayBase;
    }

    private static void LoadItemMaterialDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemMaterialDataList = m_parameters.m_csvbMaterialData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemMaterialDatas.Count; i++)
            ParameterManagerLib.ItemMaterialDataList.Add(ParameterManagerLib.CustomItemMaterialDatas[i]);

        Il2CppReferenceArray<ParameterItemDataMaterial> RefArray = new Il2CppReferenceArray<ParameterItemDataMaterial>(ParameterManagerLib.ItemMaterialDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataMaterial> ArrayBase = Il2CppArrayBase<ParameterItemDataMaterial>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbMaterialData.m_params = ArrayBase;
    }

    private static void LoadItemKeyItemDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.ItemKeyItemDataList = m_parameters.m_csvbItemDataKeyItem.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomItemKeyItemDatas.Count; i++)
            ParameterManagerLib.ItemKeyItemDataList.Add(ParameterManagerLib.CustomItemKeyItemDatas[i]);

        Il2CppReferenceArray<ParameterItemDataKeyItem> RefArray = new Il2CppReferenceArray<ParameterItemDataKeyItem>(ParameterManagerLib.ItemKeyItemDataList.ToArray());
        Il2CppArrayBase<ParameterItemDataKeyItem> ArrayBase = Il2CppArrayBase<ParameterItemDataKeyItem>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbItemDataKeyItem.m_params = ArrayBase;
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

    private static void LoadJoglessDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.JoglessList = m_parameters.m_csvbJoglessData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomJoglessDatas.Count; i++)
        {
            ParameterJoglessData @params = ParameterManagerLib.JoglessList.Find(x => x.m_id == ParameterManagerLib.CustomJoglessDatas[i].m_id);
            if (@params != null)
                ParameterManagerLib.JoglessList.Remove(@params);
            
            ParameterManagerLib.JoglessList.Add(ParameterManagerLib.CustomJoglessDatas[i]);
        }  

        Il2CppReferenceArray<ParameterJoglessData> RefArray = new Il2CppReferenceArray<ParameterJoglessData>(ParameterManagerLib.JoglessList.ToArray());
        Il2CppArrayBase<ParameterJoglessData> ArrayBase = Il2CppArrayBase<ParameterJoglessData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbJoglessData.m_params = ArrayBase;
    }

    private static void LoadJoglessGroupDatas(ParameterManager m_parameters)
    {
        ParameterManagerLib.JoglessGroupList = m_parameters.m_csvbJoglessGroupData.m_params.ToList();

        for (int i = 0; i < ParameterManagerLib.CustomJoglessGroupDatas.Count; i++)
        {
            ParameterJoglessGroupData @params = ParameterManagerLib.JoglessGroupList.Find(x => x.m_index == ParameterManagerLib.CustomJoglessGroupDatas[i].m_index);
            if (@params != null)
                ParameterManagerLib.JoglessGroupList.Remove(@params);

            ParameterManagerLib.JoglessGroupList.Add(ParameterManagerLib.CustomJoglessGroupDatas[i]);
        }

        Il2CppReferenceArray<ParameterJoglessGroupData> RefArray = new Il2CppReferenceArray<ParameterJoglessGroupData>(ParameterManagerLib.JoglessGroupList.ToArray());
        Il2CppArrayBase<ParameterJoglessGroupData> ArrayBase = Il2CppArrayBase<ParameterJoglessGroupData>.WrapNativeGenericArrayPointer(RefArray.Pointer);
        m_parameters.m_csvbJoglessGroupData.m_params = ArrayBase;
    }
}
