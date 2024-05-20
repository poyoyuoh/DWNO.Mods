using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class EnemyManagerPatch
{
    [HarmonyPatch(typeof(EnemyManager), "InstantiateEnemy")]
    [HarmonyPostfix]
    private static void EnemyManager_InstantiateEnemy_Postfix(Csvb<ParameterPlacementNpc> _npcPlacement, Csvb<ParameterNpcEnemyData> _csvbNpcEnamy, EnemyManager __instance)
    {
        if (_npcPlacement != null && _csvbNpcEnamy != null)
        {
            System.Numerics.Vector2 key = new System.Numerics.Vector2(MainGameManager.m_instance.mapNo, MainGameManager.m_instance.areaNo);
            if (ParameterManagerLib.CustomNPCEnemyDatas.ContainsKey(key))
            {
                foreach (ParameterPlacementNpc @params in _npcPlacement.m_params)
                {
                    if (ParameterManagerLib.CustomNPCEnemyDatas[key].Find(x => x.m_ParamId == @params.m_NpcEnemyParamId) != null)
                        __instance.CreateNpcEnemy(@params.m_Name);
                        __instance.ActiveNpcEnemy(@params.m_Name, false);
                }
            }
        }
    }
}
