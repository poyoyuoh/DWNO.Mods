using DWNOLib.Library;
using HarmonyLib;
using static MainGameManager;

namespace DWNOLib.Patches;
internal class MainGameManagerPatch
{
    [HarmonyPatch(typeof(MainGameManager), "_LoadNpcPlacementData")]
    [HarmonyPostfix]
    private static void MainGameManager__LoadNpcPlacementData_Postfix(int _mapNo, int _areaNo, ref bool __result, MainGameManager __instance)
    {
        __result = ParameterManagerLoader.LoadNPCPlacementDatas(false, _mapNo, _areaNo, __result, __instance);
        __result = ParameterManagerLoader.LoadNPCEnemyDatas(false, _mapNo, _areaNo, __result, __instance);
    }

    [HarmonyPatch(typeof(MainGameManager), "_LoadEnemyPlacementData")]
    [HarmonyPostfix]
    private static void MainGameManager__LoadEnemyPlacementData_Postfix(int _mapNo, int _areaNo, ref bool __result, MainGameManager __instance)
    {
        __result = ParameterManagerLoader.LoadEnemyPlacementDatas(false, _mapNo, _areaNo, __result, __instance);
    }

    [HarmonyPatch(typeof(MainGameManager.__WaitLoadPlacement_d__13), "MoveNext")]
    [HarmonyPrefix]
    private static void MainGameManager___WaitLoadPlacement_d__13__MoveNext_Prefix(MainGameManager.__WaitLoadPlacement_d__13 __instance)
    {
        if ((__instance.state & AssetPlacementLoadState.LoadStateNpcPlacementData) == AssetPlacementLoadState.LoadStateNpcPlacementData)
        {
            if (__instance.__4__this._IsLoadEndNpcPlacementData())
            {
                if (__instance.__4__this.m_nextMapReq)
                {
                    ParameterManagerLoader.LoadNPCPlacementDatas(true, __instance.__4__this.nextMapNo, __instance.__4__this.nextAreaNo, true, __instance.__4__this);
                    ParameterManagerLoader.LoadNPCEnemyDatas(true, __instance.__4__this.nextMapNo, __instance.__4__this.nextAreaNo, true, __instance.__4__this);
                }
                else
                {
                    ParameterManagerLoader.LoadNPCPlacementDatas(true, __instance.__4__this.mapNo, __instance.__4__this.areaNo, true, __instance.__4__this);
                    ParameterManagerLoader.LoadNPCEnemyDatas(true, __instance.__4__this.mapNo, __instance.__4__this.areaNo, true, __instance.__4__this);
                }
            }
        }
        if ((__instance.state & AssetPlacementLoadState.LoadStateEnemyPlacementData) == AssetPlacementLoadState.LoadStateEnemyPlacementData)
        {
            if (__instance.__4__this._IsLoadEndEnemyPlacementData())
            {
                if (__instance.__4__this.m_nextMapReq)
                    ParameterManagerLoader.LoadEnemyPlacementDatas(true, __instance.__4__this.nextMapNo, __instance.__4__this.nextAreaNo, true, __instance.__4__this);
                else
                    ParameterManagerLoader.LoadEnemyPlacementDatas(true, __instance.__4__this.mapNo, __instance.__4__this.areaNo, true, __instance.__4__this);
            }
        }
    }
}
