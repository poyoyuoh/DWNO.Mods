using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DWNOLib.Extras;
internal class MainTitleLibVersionText
{
    public static Text LibVersionText { get; private set; }

    [HarmonyPatch(typeof(uTitlePanel), "Update")]
    [HarmonyPostfix]
    private static void uTitlePanel_Update_Postfix(uTitlePanel __instance)
    {
        if (LibVersionText != null && __instance.m_VersionText != null)
            LibVersionText.color = __instance.m_VersionText.color;
    }

    [HarmonyPatch(typeof(uTitlePanel), "enablePanel")]
    [HarmonyPostfix]
    private static void uTitlePanel_enablePanel_Postfix(bool enable, uTitlePanel __instance)
    {
        if (enable)
        {
            LibVersionText = UnityEngine.UI.Text.Instantiate(__instance.m_VersionText);
            LibVersionText.name = "LibVersionText";
            LibVersionText.transform.SetParent(__instance.m_VersionText.transform.parent);
            LibVersionText.transform.localScale = new Vector3(0.28f, 0.28f);
            LibVersionText.transform.localPosition = new Vector3(-__instance.m_VersionText.transform.localPosition.x, __instance.m_VersionText.transform.localPosition.y, 0);
            LibVersionText.alignment = TextAnchor.UpperLeft;
            LibVersionText.text = DWNOLib.PluginName + " " + DWNOLib.PluginVersion.ToString();
        }
    }
}
