using DWNOLib.Library;
using HarmonyLib;
using UnityEngine;

namespace DWNOLib.Patches;
internal class SystemDataSaveLoadPatch
{
    [HarmonyPatch(typeof(SystemDataSaveLoad), "_SaveStartSystemData")]
    [HarmonyPrefix]
    private static bool StorageData__SaveStartSystemData_Prefix(SystemDataSaveLoad __instance)
    {
        StorageDataLib.SaveSystemData();
        StorageDataLib.WriteSystemData();

        StorageData.CSaveDataSaveResult csaveDataSaveResult = new StorageData.CSaveDataSaveResult();
        csaveDataSaveResult.m_IsSuccess = true;
        __instance._SystemSaveDataSaveEndCb(csaveDataSaveResult);

        string @string = Language.GetString("systemdata_save");
        uCommonMessageWindow center = AppMainScript.Ref.MessageManager.GetCenter();
        center.SetMessage(@string);
        center.SetButtonActive(uCommonMessageWindow.ButtonType.All, false);
        __instance.m_SysMsgDispStartTime = Time.unscaledTime;
        __instance.m_State = SystemDataSaveLoad.State.Save;

        return false;
    }
}
