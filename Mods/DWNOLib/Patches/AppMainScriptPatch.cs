using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class AppMainScriptPatch
{
    [HarmonyPatch(typeof(AppMainScript), "InitSetting")]
    [HarmonyPrefix]
    private static bool AppMainScript_InitSetting_Prefix()
    {
        if (StorageDataLib.IsExistSystemData())
            return false;
        return true;
    }
}
