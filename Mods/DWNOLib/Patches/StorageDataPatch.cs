using DWNOLib.Library;
using HarmonyLib;
using System;
using System.Runtime.InteropServices;

namespace DWNOLib.Patches;
internal class StorageDataPatch
{
    [HarmonyPatch(typeof(StorageData), "OpenLoadGameMenu")]
    [HarmonyPrefix]
    private static bool StorageData_OpenLoadGameMenu_Prefix(ref bool __result, [Optional] StorageData.SaveDataLoadEndCb _loadEndCb)
    {
        if (StorageDataLib.IsExistGameData(StorageData.m_saveSlot))
        {
            __result = StorageDataLib.LoadSaveData(StorageData.m_saveSlot);
            StorageData.CSaveDataLoadResult csaveDataLoadResult = new StorageData.CSaveDataLoadResult();
            csaveDataLoadResult.m_IsSuccess = true;
            _loadEndCb?.Invoke(csaveDataLoadResult);
            return false;
        }
        return true;
    }

    [HarmonyPatch(typeof(StorageData), "OpenSaveGameMenu")]
    [HarmonyPrefix]
    private static bool StorageData_OpenSaveGameMenu_Prefix([Optional] StorageData.SaveDataSaveEndCb _saveEndCb)
    {
        StorageDataLib.SaveSaveData();

        StorageData.CSaveDataSaveResult csaveDataSaveResult = new StorageData.CSaveDataSaveResult();
        Action<bool> action = delegate (bool isCancel)
        {
            if (isCancel)
            {
                csaveDataSaveResult.m_ErrorType = StorageData.CSaveDataSaveResult.ErrorType.Cancel;
                _saveEndCb?.Invoke(csaveDataSaveResult);
            }
        };

        StorageData.m_uSavePanel.InitializeSave(action);
        StorageData.m_uSavePanel.enablePanel(true);
        return false;
    }

    [HarmonyPatch(typeof(StorageData), "LoadSystemData")]
    [HarmonyPrefix]
    private static bool StorageData_LoadSystemData_Prefix(ref bool __result, [Optional] StorageData.SaveDataLoadEndCb _loadEndCb)
    {
        if (StorageDataLib.IsExistSystemData())
        {
            __result = StorageDataLib.LoadSystemData();
            StorageData.CSaveDataLoadResult csaveDataLoadResult = new StorageData.CSaveDataLoadResult();
            csaveDataLoadResult.m_IsSuccess = true;
            _loadEndCb?.Invoke(csaveDataLoadResult);
            return false;
        }
        return true;
    }
}
