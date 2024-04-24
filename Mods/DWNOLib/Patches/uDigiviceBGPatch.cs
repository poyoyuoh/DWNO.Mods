using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class uDigiviceBGPatch
{
    /// <summary>
    /// DigiviceSoloCamera aren't loaded during the initial parameters load, it happen when it's needed, so load them here.
    /// </summary>
    [HarmonyPatch(typeof(uDigiviceBG), "Awake")]
    [HarmonyPostfix]
    private static void uDigiviceBG_Awake_Postfix(uDigiviceBG __instance)
    {
        ParameterManagerLoader.LoadDigiviceSoloCameraDatas(__instance);
    }
}
