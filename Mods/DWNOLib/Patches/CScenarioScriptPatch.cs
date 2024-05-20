using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class CScenarioScriptPatch
{
    [HarmonyPatch(typeof(CScenarioScript), "CallCmdBlockCommonSelectWindow")]
    [HarmonyPrefix]
    private static bool CScenarioScript_CallCmdBlockCommonSelectWindow_Prefix(ref ParameterCommonSelectWindow _param)
    {
        if (_param != null)
        {
            if (ParameterManagerLib.CustomScriptCommands.ContainsKey(_param.m_scriptCommand))
            {
                ParameterManagerLib.CustomScriptCommands[_param.m_scriptCommand]();
                return false;
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(CScenarioScript), "CallCmdBlockChapter")]
    [HarmonyPrefix]
    private static bool CScenarioScript_CallCmdBlockChapter_Prefix(string _cmdBlock)
    {
        if (ParameterManagerLib.CustomScriptCommands.ContainsKey(_cmdBlock))
        {
            ParameterManagerLib.CustomScriptCommands[_cmdBlock]();
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(CScenarioScript), "BattleEndCB")]
    [HarmonyPrefix]
    private static bool CScenarioScript_BattleEndCB_Prefix()
    {
        if (Commands.BattleEndCB != null)
        {
            if (MainGameComponent.Ref.battleResult == MainGameComponent.BATTLE_RESULT.Win)
            {
                Commands.BattleEndCB.Invoke();
                Commands.BattleEndCB = null;
                return false;
            }
        }
        return true;
    }
}
