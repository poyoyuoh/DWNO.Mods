using Il2CppSystem.Reflection;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DWNOLib.Library;
public class Commands
{
    public static Action BattleEndCB { get; set; } = null;

    public static void EnableCommonSelectWindowUI(bool enable, int window_type)
    {
        if (MainGameManager.Ref.commonSelectWindowUI != null)
        {
            MainGameManager.Ref.scenarioScript?.ClearCmdBlockCommonSelectWindow();
            if (enable)
                MainGameComponent.Ref.m_NextStep = MainGame.STEP.Event;
            else
                MainGameComponent.Ref.m_NextStep = MainGame.STEP.Field;
            MainGameManager.Ref.commonSelectWindowUI.SetEnablePanel(enable, (ParameterCommonSelectWindowMode.WindowType)window_type);
        }
    }

    public static void StartNPCBattle(uint[] array, Action _BattleEndCB = null)
    {
        if (array.Length == 0)
        {
            Logger.Log("Commands StartNPCBattle: Called with an empty array.", Logger.LogType.Error);
            return;
        }

        if (array.Length > 5)
        {
            Logger.Log("Commands StartNPCBattle: The array mustn't be bigger than 5.", Logger.LogType.Error);
            return;
        }

        ParameterPlacementNpc npc0 = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == array[0]);
        if (npc0 == null)
        {
            Logger.Log($"Commands StartNPCBattle: The array at index 0 with id {array[0]} doesn't exist in the active npc list.", Logger.LogType.Error);
            return;
        }
        else
        {
            if (ParameterManagerLib.NPCEnemyDataList.Find(x => x.m_ParamId == npc0.m_NpcEnemyParamId) == null)
            {
                Logger.Log($"Commands StartNPCBattle: The active NPCEnemyDataList doesn't contain the id {npc0.m_NpcEnemyParamId} of npc0.", Logger.LogType.Error);
                return;
            }
        }
        MainGameManager.Ref.enemyMgr.ReviveNpcEnemy(array[0]);
        MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[0]).enabled = true;
        MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[0]).gameObject.SetActive(true);
        MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[0]).actionState = UnitCtrlBase.ActionState.ActionState_Idle;

        if (array.Length > 1)
        {
            ParameterPlacementNpc npc1 = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == array[1]);
            if (npc1 == null)
            {
                Logger.Log($"Commands StartNPCBattle: The array at index 1 with id {array[1]} doesn't exist in the active npc list.", Logger.LogType.Error);
                return;
            }
            else
            {
                if (ParameterManagerLib.NPCEnemyDataList.Find(x => x.m_ParamId == npc1.m_NpcEnemyParamId) == null)
                {
                    Logger.Log($"Commands StartNPCBattle: The active NPCEnemyDataList doesn't contain the id {npc1.m_NpcEnemyParamId} of npc1.", Logger.LogType.Error);
                    return;
                }
            }
            MainGameManager.Ref.enemyMgr.ReviveNpcEnemy(array[1]);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[1]).enabled = true;
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[1]).gameObject.SetActive(true);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[1]).actionState = UnitCtrlBase.ActionState.ActionState_Idle;
        }

        if (array.Length > 2)
        {
            ParameterPlacementNpc npc2 = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == array[2]);
            if (npc2 == null)
            {
                Logger.Log($"Commands StartNPCBattle: The array at index 2 with id {array[2]} doesn't exist in the active npc list.", Logger.LogType.Error);
                return;
            }
            else
            {
                if (ParameterManagerLib.NPCEnemyDataList.Find(x => x.m_ParamId == npc2.m_NpcEnemyParamId) == null)
                {
                    Logger.Log($"Commands StartNPCBattle: The active NPCEnemyDataList doesn't contain the id {npc2.m_NpcEnemyParamId} of npc2.", Logger.LogType.Error);
                    return;
                }
            }
            MainGameManager.Ref.enemyMgr.ReviveNpcEnemy(array[2]);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[2]).enabled = true;
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[2]).gameObject.SetActive(true);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[2]).actionState = UnitCtrlBase.ActionState.ActionState_Idle;
        }

        if (array.Length > 3)
        {
            ParameterPlacementNpc npc3 = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == array[3]);
            if (npc3 == null)
            {
                Logger.Log($"Commands StartNPCBattle: The array at index 3 with id {array[3]} doesn't exist in the active npc list.", Logger.LogType.Error);
                return;
            }
            else
            {
                if (ParameterManagerLib.NPCEnemyDataList.Find(x => x.m_ParamId == npc3.m_NpcEnemyParamId) == null)
                {
                    Logger.Log($"Commands StartNPCBattle: The active NPCEnemyDataList doesn't contain the id {npc3.m_NpcEnemyParamId} of npc3.", Logger.LogType.Error);
                    return;
                }
            }
            MainGameManager.Ref.enemyMgr.ReviveNpcEnemy(array[3]);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[3]).enabled = true;
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[3]).gameObject.SetActive(true);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[3]).actionState = UnitCtrlBase.ActionState.ActionState_Idle;
        }

        if (array.Length > 4)
        {
            ParameterPlacementNpc npc4 = ParameterManagerLib.PlacementNPCDataList.Find(x => x.m_Name == array[4]);
            if (npc4 == null)
            {
                Logger.Log($"Commands StartNPCBattle: The array at index 4 with id {array[4]} doesn't exist in the active npc list.", Logger.LogType.Error);
                return;
            }
            else
            {
                if (ParameterManagerLib.NPCEnemyDataList.Find(x => x.m_ParamId == npc4.m_NpcEnemyParamId) == null)
                {
                    Logger.Log($"Commands StartNPCBattle: The active NPCEnemyDataList doesn't contain the id {npc4.m_NpcEnemyParamId} of npc4.", Logger.LogType.Error);
                    return;
                }
            }
            MainGameManager.Ref.enemyMgr.ReviveNpcEnemy(array[4]);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[4]).enabled = true;
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[4]).gameObject.SetActive(true);
            MainGameManager.Ref.enemyMgr._GetNpcEnemy(array[4]).actionState = UnitCtrlBase.ActionState.ActionState_Idle;
        }

        BattleEndCB = _BattleEndCB;
        MainGameManager.Ref.enemyMgr.RequestNpcBattleStart(array);
        MainGameComponent.Ref.m_StepProc[6].GetIl2CppType().GetMethod("_EventBattleStart", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(MainGameComponent.Ref.m_StepProc[6], new Il2CppSystem.Object[] { });
        MainGameComponent.Ref.m_StepProc[6]._ChangeMain(MainGame.STEP.Battle);
    }

    public static async Task RotatePlayerAndNPC(MainGameManager.UNITID npc_idx, float delay = 0.2f)
    {
        if ((int)npc_idx > (int)MainGameManager.UNITID.NpcIdEnd || (int)npc_idx < (int)MainGameManager.UNITID.NpcIdBegin)
        {
            Logger.Log("Commands RotatePlayerAndNPC: called with an invalid arguments.", Logger.LogType.Error);
            return;
        }

        Transform player = MainGameManager.GetPlayer().transform;
        Transform npc = MainGameManager.GetNpc((int)npc_idx - 3).gameObject.transform;

        Vector3 player_normalized = (player.position - npc.position).normalized;
        Vector3 npc_normalized = (npc.position - player.position).normalized;

        Quaternion player_target_rotation = Quaternion.LookRotation(npc_normalized);
        Quaternion npc_target_rotation = Quaternion.LookRotation(player_normalized);

        TweenRotation.Begin(player.gameObject, delay, new Quaternion(0f, player_target_rotation.y, 0f, player_target_rotation.w));
        TweenRotation.Begin(npc.gameObject, delay, new Quaternion(0f, npc_target_rotation.y, 0f, npc_target_rotation.w));
        await Task.Delay(TimeSpan.FromSeconds(delay));
    }
}
