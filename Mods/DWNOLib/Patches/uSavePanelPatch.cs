using DWNOLib.Library;
using HarmonyLib;

namespace DWNOLib.Patches;
internal class uSavePanelPatch
{
    [HarmonyPatch(typeof(uSavePanel.__c__DisplayClass27_1), "_CheckInputKey_b__1")]
    [HarmonyPostfix]
    private static void uSavePanel___c__DisplayClass27_1__CheckInputKey_b__1_Postfix(bool x, uSavePanel.__c__DisplayClass27_1 __instance)
    {
        if (!x)
            return;
        StorageDataLib.WriteSaveData(__instance.corsorindex);
    }
}
