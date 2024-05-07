using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AI;
using static uPartnerAttackPanel;
using static uPartnerHistoryPanel;
using static uPartnerStatusPanel;
using static uPartnerTacticsPanel;
using static uTrainingPanelCommand;

namespace OnePartner;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
[BepInDependency("poyoyuoh.DWNO.SkipTutorials")]
[BepInIncompatibility("poyoyuoh.DWNO.DeathSync")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.OnePartner";
    internal const string PluginName = "OnePartner";
    internal const string PluginVersion = "1.0.0";

    public static bool is_new_game = false;

    public static bool enable_other_partner = false;

    public static bool is_battle = false;

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    #region Logic stuffs

    [HarmonyPatch(typeof(NavMeshAgent), "ResetPath")]
    [HarmonyPrefix]
    public static bool NavMeshAgent_ResetPath_Prefix(NavMeshAgent __instance)
    {
        if (__instance.isActiveAndEnabled == false)
            return false;
        return true;
    }

    [HarmonyPatch(typeof(MainGameManager), "GetPartnerCtrl")]
    [HarmonyPrefix]
    public static bool MainGameManager_GetPartnerCtrl_Prefix(ref int _idx)
    {
        if (!is_new_game && !enable_other_partner)
        {
            if (_idx == 1)
                _idx = 0;
        }

        return true;
    }

    [HarmonyPatch(typeof(PartnerCtrl), "GetOtherPartner")]
    [HarmonyPrefix]
    public static bool PartnerCtrl_GetOtherPartner_Prefix(ref PartnerCtrl __result, PartnerCtrl __instance)
    {
        if (!enable_other_partner)
        {
            PartnerCtrl partnerCtrl = AppInfo.Ref.battleInfo.partnerList[0];
            if (partnerCtrl != null)
            {
                __result = partnerCtrl;
                return false;
            }
        }

        return true;
    }

    [HarmonyPatch(typeof(MainTitle), "Update")]
    [HarmonyPrefix]
    public static void MainTitle_Update_Prefix(MainTitle __instance)
    {
        if (__instance.m_State == MainTitle.State.NameEntry)
        {
            if (__instance.m_name_entry != null && __instance.m_name_entry.IsFinished())
            {
                is_new_game = true;
            }
        }
    }

    [HarmonyPatch(typeof(MainGameDigitama), "_End")]
    [HarmonyPostfix]
    public static void MainGameDigitama__End_Postfix(MainGameDigitama __instance)
    {
        if (is_new_game)
        {
            if (__instance.m_Step.subStep == 2)
            {
                MainGameManager.GetPartnerCtrl(1).gameObject.SetActive(false);
                is_new_game = false;
            }
        }
    }

    [HarmonyPatch(typeof(EvolutionMain), "Start")]
    [HarmonyPrefix]
    public static void EvolutionMain_Start_Prefix() => enable_other_partner = true;

    [HarmonyPatch(typeof(EvolutionBase), "OnDestroy")]
    [HarmonyPrefix]
    public static void EvolutionBase_OnDestroy_Prefix() => enable_other_partner = false;

    [HarmonyPatch(typeof(PartnerCtrl), "AddOrderPower")]
    [HarmonyPrefix]
    public static void PartnerCtrl_AddOrderPower_Prefix(ref int orderPower) => orderPower /= 2;

    [HarmonyPatch(typeof(MainGameBattle), "_Start")]
    [HarmonyPrefix]
    public static void MainGameBattle__Start_Prefix() => is_battle = true;

    [HarmonyPatch(typeof(MainGameBattle), "_Win")]
    [HarmonyPrefix]
    public static void MainGameBattle__Win_Prefix() => is_battle = false;

    [HarmonyPatch(typeof(MainGameBattle), "_Lose")]
    [HarmonyPrefix]
    public static void MainGameBattle__Lose_Prefix() => is_battle = false;

    [HarmonyPatch(typeof(MainGameBattle), "_Miracle")]
    [HarmonyPrefix]
    public static void MainGameBattle__Miracle_Prefix() => is_battle = true;

    [HarmonyPatch(typeof(MainGameBattle), "_Escape")]
    [HarmonyPrefix]
    public static void MainGameBattle__Escape_Prefix() => is_battle = false;

    [HarmonyPatch(typeof(MainGameBattle), "_BattleBreak")]
    [HarmonyPrefix]
    public static void MainGameBattle__BattleBreak_Prefix() => is_battle = false;

    [HarmonyPatch(typeof(uBattlePanelItemBox), "UseItemCB")]
    [HarmonyPrefix]
    public static void uBattlePanelItemBox_UseItemCB_Prefix() => enable_other_partner = true;

    [HarmonyPatch(typeof(uBattlePanelItemBox), "UseItemCB")]
    [HarmonyPostfix]
    public static void uBattlePanelItemBox_UseItemCB_Postfix() => enable_other_partner = false;

    [HarmonyPatch(typeof(MainGameManager), "MapStart")]
    [HarmonyPrefix]
    public static void MainGameManager_MapStart_Prefix() => enable_other_partner = true;

    [HarmonyPatch(typeof(MainGameManager), "MapStart")]
    [HarmonyPostfix]
    public static void MainGameManager_MapStart_Postfix()
    {
        enable_other_partner = true;
        MainGameManager.GetPartnerCtrl(1).gameObject.SetActive(false);
        enable_other_partner = false;
    }

    [HarmonyPatch(typeof(PartnerCtrl), "IsPossibleMiracle")]
    [HarmonyPrefix]
    public static bool PartnerCtrl_IsPossibleMiracle_Prefix(ref bool __result)
    {
        __result = false;
        return false;
    }
    #endregion

    #region Sleepyhead

    [HarmonyPatch(typeof(uCarePanel), "SleepRequest")]
    [HarmonyPrefix]
    public static bool uCarePanel_SleepRequest_Prefix(uCarePanel __instance)
    {
        if (StorageData.m_playerData.m_partners[0].IsReqSleep())
        {
            MainGameManager.SetMenuResult_CareCommand(PartnerAIManager.PartnerCommand.SelectCommandPutToSleep, 0u);
        }
        __instance.m_state = uCarePanel.State.PutToSleepWait;
        return false;
    }

    [HarmonyPatch(typeof(uCarePanel), "PutToSleep")]
    [HarmonyPrefix]
    public static bool uCarePanel_PutToSleep_Prefix(uCarePanel __instance)
    {
        if (StorageData.m_playerData.m_partners[0].IsReqSleep())
        {
            __instance.m_result = uCarePanel.Result.Sleep;
            __instance.enablePanel(false);
            return false;
        }
        if (__instance.m_commandPanel != null)
        {
            __instance.m_commandPanel.enablePanel(false);
        }
        __instance.m_captionPanel.enablePanel(false);
        MainGameManager.Ref.MessageManager.GetCenter().SetMessage(string.Format(Language.GetString("CARE_NO_SLEEP"), __instance.m_digimonCtrl[0].Name, __instance.m_digimonCtrl[0].Name));
        __instance.m_state = uCarePanel.State.Message;
        __instance.StartMoveOut();
        return false;
    }

    [HarmonyPatch(typeof(uCarePanel), "UpdatePutToSleepWait")]
    [HarmonyPrefix]
    public static bool uCarePanel_UpdatePutToSleepWait_Prefix(uCarePanel __instance)
    {
        GameObject partner = MainGameManager.GetPartner(0);
        if (!(partner == null) && partner.activeSelf)
        {
            PartnerCtrl component = partner.GetComponent<PartnerCtrl>();
            if (component != null && component.CareCmd == PartnerAIManager.PartnerCommand.SelectCommandPresent)
            {
                return false;
            }
        }
        __instance.m_state = uCarePanel.State.PutToSleep;
        return false;
    }

    [HarmonyPatch(typeof(MainGameSleep), "_Start")]
    [HarmonyPrefix]
    public static void MainGameSleep__Start_Prefix() => enable_other_partner = true;

    [HarmonyPatch(typeof(MainGameSleep), "_End")]
    [HarmonyPrefix]
    public static void MainGameSleep__End_Prefix() => enable_other_partner = false;

    [HarmonyPatch(typeof(PartnerCtrl), "_UpdateSleepCmd")]
    [HarmonyPrefix]
    public static void PartnerCtrl__UpdateSleepCmd_Prefix() => enable_other_partner = true;

    [HarmonyPatch(typeof(PartnerCtrl), "_UpdateSleepCmd")]
    [HarmonyPostfix]
    public static void PartnerCtrl__UpdateSleepCmd_Postfix() => enable_other_partner = false;
    #endregion

    #region Default and Input stuffs

    [HarmonyPatch(typeof(uCarePanel), "enablePanel")]
    [HarmonyPrefix]
    public static void uCarePanel_enablePanel_Prefix(ref MainGameManager.ORDER_UNIT orderUnit) => orderUnit = MainGameManager.ORDER_UNIT.Partner00;

    [HarmonyPatch(typeof(uCarePanel), "SetAllTargetIcon")]
    [HarmonyPrefix]
    public static void uCarePanel_SetAllTargetIcon_Prefix(ref MainGameManager.ORDER_UNIT set_target) => set_target = MainGameManager.ORDER_UNIT.Partner00;

    [HarmonyPatch(typeof(uCarePanel), "TargetDigimonSelect")]
    [HarmonyPrefix]
    public static bool uCarePanel_TargetDigimonSelect_Prefix() => false;

    [HarmonyPatch(typeof(uPartnerTopPanelInfoCtrl), "Initialize")]
    [HarmonyPrefix]
    public static bool uPartnerTopPanelInfoCtrl_Initialize_Prefix(uPartnerTopPanelInfoCtrl __instance)
    {
        __instance.m_SelectPartnerNo = 1;
        return true;
    }

    [HarmonyPatch(typeof(uPartnerTopPanelInfoCtrl), "PartnerSelect")]
    [HarmonyPrefix]
    public static bool uPartnerTopPanelInfoCtrl_PartnerSelect_Prefix() => false;

    [HarmonyPatch(typeof(uPartnerStatusPanel), "InputCheck")]
    [HarmonyPrefix]
    public static bool uPartnerStatusPanel_InputCheck_Prefix(uPartnerStatusPanel __instance)
    {
        if (PadManager.IsTrigger(PadManager.BUTTON.srLeft))
        {
            __instance.m_State = PartnerStatusState.PrevPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.srRight))
        {
            __instance.m_State = PartnerStatusState.NextPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bCross))
        {
            __instance.m_State = PartnerStatusState.BackTopPanelPrepare;
            CriSoundManager.PlayCommonSe("S_008");
            CriSoundManager.PlayCommonSe("S_002");
            CriSoundManager.PlayCommonSe("S_014");
        }
        return false;
    }

    [HarmonyPatch(typeof(uPartnerTacticsPanel), "InputCheck")]
    [HarmonyPrefix]
    public static bool uPartnerTacticsPanel_InputCheck_Prefix(uPartnerTacticsPanel __instance)
    {
        if (PadManager.IsTrigger(PadManager.BUTTON.srLeft))
        {
            __instance.m_State = PartnerTacticsState.PrevPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.srRight))
        {
            __instance.m_State = PartnerTacticsState.NextPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bCross))
        {
            __instance.m_State = PartnerTacticsState.BackTopPanelPrepare;
            CriSoundManager.PlayCommonSe("S_008");
            CriSoundManager.PlayCommonSe("S_002");
            CriSoundManager.PlayCommonSe("S_014");
        }
        return false;
    }

    [HarmonyPatch(typeof(uPartnerHistoryPanel), "InputCheck")]
    [HarmonyPrefix]
    public static bool uPartnerHistoryPanel_InputCheck_Prefix(uPartnerHistoryPanel __instance)
    {
        if (!__instance.m_IsUseRapidChange && PadManager.IsTrigger(PadManager.BUTTON.srLeft))
        {
            __instance.m_State = PartnerHistoryState.PrevPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (!__instance.m_IsUseRapidChange && PadManager.IsTrigger(PadManager.BUTTON.srRight))
        {
            __instance.m_State = PartnerHistoryState.NextPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bCross))
        {
            __instance.m_State = PartnerHistoryState.BackTopPanelPrepare;
            CriSoundManager.PlayCommonSe("S_008");
            CriSoundManager.PlayCommonSe("S_002");
            CriSoundManager.PlayCommonSe("S_014");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bSquare))
        {
            __instance.m_Genelogy.InputSquare();
        }
        return false;
    }

    [HarmonyPatch(typeof(uPartnerAttackPanel), "InputCheck_Normal")]
    [HarmonyPrefix]
    public static bool uPartnerAttackPanel_InputCheck_Normal_Prefix(uPartnerAttackPanel __instance)
    {
        if (PadManager.IsTrigger(PadManager.BUTTON.srLeft))
        {
            __instance.m_State = PartnerAttackState.PrevPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.srRight))
        {
            __instance.m_State = PartnerAttackState.NextPanelPrepare;
            CriSoundManager.PlayCommonSe("S_001");
            CriSoundManager.PlayCommonSe("S_013");
        }
        else if (!AssetBundleManager.isNowLoading && PadManager.IsTrigger(PadManager.BUTTON.bCross))
        {
            __instance.m_State = PartnerAttackState.BackTopPanelPrepare;
            CriSoundManager.PlayCommonSe("S_008");
            CriSoundManager.PlayCommonSe("S_002");
            CriSoundManager.PlayCommonSe("S_014");
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bCircle))
        {
            if (__instance.m_Command.InputOK_Normal())
            {
                __instance.m_State = PartnerAttackState.SelectSkillPrepare;
                CriSoundManager.PlayCommonSe("S_007");
                CriSoundManager.PlayCommonSe("S_001");
            }
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bSquare))
        {
            __instance.m_Command.InputSquare();
        }
        return false;
    }

    [HarmonyPatch(typeof(uEvolutionDojoPanel), "InputCheck")]
    [HarmonyPrefix]
    public static bool uEvolutionDojoPanel_InputCheck_Prefix(uEvolutionDojoPanel __instance)
    {
        if (__instance.m_Genelogy.m_DialogOpen)
        {
            return false;
        }
        if (PadManager.IsTrigger(PadManager.BUTTON.bCircle))
        {
            __instance.m_Genelogy.InputOK();
        }
        else if (PadManager.IsTrigger(PadManager.BUTTON.bCross))
        {
            if (__instance.m_Genelogy.m_DialogOpen_inputNG)
            {
                __instance.m_Genelogy.m_DialogOpen_inputNG = false;
                return false;
            }
            __instance.enablePanel(false);
            __instance.CallCloseCallBack();
        }
        return false;
    }

    [HarmonyPatch(typeof(TrainingCursor), "Update")]
    [HarmonyPrefix]
    public static bool TrainingCursor_Update_Prefix(TrainingCursor __instance)
    {
        if (__instance.m_digimonIcon.m_image.name == "Icon_L")
        {
            __instance.m_digimonIcon.m_image.transform.GetParent().gameObject.SetActive(false);

            if (__instance.index != ParameterTrainingData.TrainingKindIndex.Rest)
                __instance.isMove = true;
            else
                __instance.isMove = false;
            __instance.index = ParameterTrainingData.TrainingKindIndex.Rest;
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(PadManager), "IsInput")]
    [HarmonyPrefix]
    public static bool PadManager_IsInput_Prefix(PadManager.BUTTON button, ref bool __result, PadManager __instance)
    {
        if (is_battle)
        {
            if (button == PadManager.BUTTON.bL || button == (PadManager.BUTTON.bL & PadManager.BUTTON.bR))
            {
                __result = false;
                return false;
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(PadManager), "IsTrigger")]
    [HarmonyPrefix]
    public static bool PadManager_IsTrigger_Prefix(PadManager.BUTTON button, ref bool __result, PadManager __instance)
    {
        if (is_battle)
        {
            if (button == PadManager.BUTTON.bL || button == (PadManager.BUTTON.bL & PadManager.BUTTON.bR))
            {
                __result = false;
                return false;
            }
        }
        return true;
    }

    [HarmonyPatch(typeof(PadManager), "GetInput")]
    [HarmonyPostfix]
    public static void PadManager_GetInput_Postfix(ref PadManager.BUTTON __result, PadManager __instance)
    {
        if (is_battle)
        {
            if (__result == PadManager.BUTTON.bL || __result == (PadManager.BUTTON.bL & PadManager.BUTTON.bR))
            {
                __result = PadManager.BUTTON._Non;
            }
        }
    }

    [HarmonyPatch(typeof(PadManager), "GetTrigger")]
    [HarmonyPostfix]
    public static void PadManager_GetTrigger_Postfix(ref PadManager.BUTTON __result, PadManager __instance)
    {
        if (is_battle)
        {

            if (__result == PadManager.BUTTON.bL || __result == (PadManager.BUTTON.bL & PadManager.BUTTON.bR))
            {
                __result = PadManager.BUTTON._Non;
            }
        }
    }

    [HarmonyPatch(typeof(CameraScriptBattle), "_StepItemBox")]
    [HarmonyPrefix]
    public static bool PadManager__StepItemBox_Prefix(CameraScriptBattle __instance)
    {
        switch (__instance.m_step.subStep)
        {
            case 0:
                __instance.m_buttonBit = 0u;
                __instance.m_itemThrowTarget = MainGameManager.ORDER_UNIT.Partner00;
                if (!__instance.InitBattleCameraItemBoxPartner(0))
                {
                    return false;
                }
                break;
            case 1:
                __instance.BattleCameraItemBoxPartner();
                break;
        }
        __instance._PlayCamPamraCur();
        return false;
    }
    #endregion

    #region Menu hiding stuffs

    [HarmonyPatch(typeof(uPartnerTopPanelInfo), "enablePanel")]
    [HarmonyPrefix]
    public static bool uPartnerTopPanelInfo_enablePanel_Prefix(uPartnerTopPanelInfo __instance)
    {
        if (__instance.m_LR == uPartnerTopPanelInfo.PartnerLR.L)
        {
            __instance.gameObject.SetActive(false);
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(uDigiviceBG), "SetCameraState")]
    [HarmonyPostfix]
    public static void uDigiviceBG_SetCameraState_Postfix(uDigiviceBG __instance)
    {
        __instance.m_PartnerModel[1].SetActive(false);
    }

    [HarmonyPatch(typeof(uDigiviceTopPanelKizuna), "Initialize")]
    [HarmonyPostfix]
    public static void uDigiviceTopPanelKizuna_Initialize_Postfix(uDigiviceTopPanelKizuna __instance)
    {
        __instance.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        __instance.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        __instance.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(uCarePanelKizuna), "enablePanel")]
    [HarmonyPostfix]
    public static void uCarePanelKizuna_enablePanel_Postfix(uCarePanelKizuna __instance)
    {
        __instance.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        __instance.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        __instance.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        __instance.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
        __instance.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(uSavePanelItemSaveItem), "Load")]
    [HarmonyPostfix]
    public static void uSavePanelItemSaveItem_Load_Postfix(uSavePanelItemSaveItem __instance)
    {
        __instance.m_pertnersIcons[1].gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(uFieldDigimonPanel), "Initialize")]
    [HarmonyPostfix]
    public static void uFieldDigimonPanel_Initialize_Postfix(uFieldDigimonPanel __instance)
    {
        if (__instance.name == "Status_L")
            __instance.gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(TrainingInformation), "Initialize")]
    [HarmonyPostfix]
    public static void TrainingInformation_Initialize_Postfix(TrainingInformation __instance)
    {
        if (__instance.name == "Information_L")
            __instance._refAnim.SetActive(false);
    }

    [HarmonyPatch(typeof(uTrainingPanelResult), "enablePanel")]
    [HarmonyPostfix]
    public static void uTrainingPanelResult_enablePanel_Postfix(uTrainingPanelResult __instance)
    {
        __instance.m_trainingResultPanelDigimons[1].gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(uResultPanelDigimon), "enablePanel")]
    [HarmonyPostfix]
    public static void uResultPanelDigimon_enablePanel_Postfix(uResultPanelDigimon __instance)
    {
        if (__instance.name == "Partner_L")
            __instance.gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(uBattlePanelDigimon), "enablePanel")]
    [HarmonyPostfix]
    public static void uBattlePanelDigimon_enablePanel_Postfix(uBattlePanelDigimon __instance)
    {
        if (__instance.name == "Status_Panel_L")
            __instance.gameObject.SetActive(false);
    }
    #endregion
}
