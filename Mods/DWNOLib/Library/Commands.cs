using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DWNOLib.Library;
public class Commands
{
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

    public static async Task RotatePlayerAndNPC(MainGameManager.UNITID npc_idx, float delay = 0.2f)
    {
        if ((int)npc_idx > (int)MainGameManager.UNITID.NpcIdEnd || (int)npc_idx < (int)MainGameManager.UNITID.NpcIdBegin)
        {
            Logger.Log("Commands: RotatePlayerAndNPC called with an invalid arguments.", Logger.LogType.Error);
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
