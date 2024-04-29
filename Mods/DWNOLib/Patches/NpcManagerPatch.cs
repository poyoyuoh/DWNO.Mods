using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class NpcManagerPatch
{
    [HarmonyPatch(typeof(NpcManager), "InstantiateNpc")]
    [HarmonyPostfix]
    private static void NpcManager_InstantiateNpc_Postfix(NpcManager __instance)
    {
        System.Numerics.Vector2 key = new System.Numerics.Vector2(MainGameManager.m_instance.mapNo, MainGameManager.m_instance.areaNo);
        if (ParameterManagerLib.CustomPlacementNPCDatas.ContainsKey(key))
        {
            foreach (ParameterPlacementNpc npc in ParameterManagerLib.CustomPlacementNPCDatas[key])
            {
                __instance.CreateNpc(npc.id);
            }
        }
    }
}
