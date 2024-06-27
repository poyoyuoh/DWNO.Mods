using DWNOLib.Library;
using HarmonyLib;
using System;

namespace DWNOLib.Patches;
internal class PlayerDataPatch
{
    [HarmonyPatch(typeof(PlayerData), "CreateEvolutionSkillFlag")]
    [HarmonyPrefix]
    private static bool PlayerData_CreateEvolutionSkillFlag_Prefix(PlayerData __instance)
    {
        __instance.m_EvolutionFlag = new CBitFlag(StorageDataLib.StorageDataLimits.MAX_EVOLUTION_FLAG);
        return false;
    }

    [HarmonyPatch(typeof(PlayerData), "CreateEvolutionConditionFlag")]
    [HarmonyPrefix]
    private static bool PlayerData_CreateEvolutionConditionFlag_Prefix(PlayerData __instance)
    {
        __instance.m_EvolutionConditionFlag = new CBitFlag[StorageDataLib.StorageDataLimits.MAX_EVOLUTION_FLAG];
        for (int i = 0; i < StorageDataLib.StorageDataLimits.MAX_EVOLUTION_FLAG; i++)
        {
            __instance.m_EvolutionConditionFlag[i] = new CBitFlag(StorageDataLib.StorageDataLimits.max_condition_flag);
        }
        return false;
    }

    [HarmonyPatch(typeof(PlayerData), "InitializeParameter")]
    [HarmonyPrefix]
    private static void PlayerData_InitializeParameter_Prefix(PlayerData __instance)
    {
        __instance.CreateEvolutionSkillFlag();
        __instance.CreateEvolutionConditionFlag();

    }

    [HarmonyPatch(typeof(PlayerData), "ReadSaveData")]
    [HarmonyPrefix]
    private static void PlayerData_ReadSaveData_Prefix(PlayerData __instance)
    {
        StorageDataLib.StorageDataLimits.ResetLimits();
    }

    [HarmonyPatch(typeof(PlayerData), "ReadSaveData")]
    [HarmonyPostfix]
    private static void PlayerData_ReadSaveData_Postfix(PlayerData __instance)
    {
        StorageDataLib.StorageDataLimits.RestoreLimits();
    }

    [HarmonyPatch(typeof(PlayerData), "AddLevel")]
    [HarmonyPrefix]
    private static bool PlayerData_AddLevel_Prefix(int add, PlayerData __instance)
    {
        __instance.m_level += add;
        return false;
    }

    [HarmonyPatch(typeof(PlayerData), "AddSkillPoint")]
    [HarmonyPrefix]
    private static bool PlayerData_AddSkillPoint_Prefix(int addPoint, PlayerData __instance)
    {
        __instance.m_skillPoint += addPoint;
        return false;
    }
}
