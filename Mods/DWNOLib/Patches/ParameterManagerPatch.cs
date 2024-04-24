using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class ParameterManagerPatch
{
    [HarmonyPatch(typeof(ParameterManager._SetupParameterTask_d__298), "MoveNext")]
    [HarmonyPostfix]
    private static void ParameterManager__SetupParameterTask_d__298_MoveNext_Postfix(ParameterManager._SetupParameterTask_d__298 __instance)
    {
        switch (__instance.__1__state)
        {
            case -1:
                ParameterManagerLoader.LoadParameters(__instance.__4__this);
                break;
        }
    }
}
