using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace DescriptiveEnemies;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.DescriptiveEnemies";
    internal const string PluginName = "DescriptiveEnemies";
    internal const string PluginVersion = "1.0.0";

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(uEnemyName), "StartSign", new System.Type[] { typeof(GameObject), typeof(bool), typeof(float) })]
    [HarmonyPostfix]
    public static void uEnemyName_StartSign_Postfix(GameObject unit, uEnemyName __instance)
    {
        DigimonCtrl component = unit.GetComponent<DigimonCtrl>();
        if (component != null)
        {
            string text = "";
            text += "HP: " + component.gameData.m_commonData.m_hpMax.ToString() + "\n";
            text += "MP: " + component.gameData.m_commonData.m_mpMax.ToString() + "\n";
            text += "STR: " + component.gameData.m_commonData.m_forcefulness.ToString() + "\n";
            text += "STA: " + component.gameData.m_commonData.m_robustness.ToString() + "\n";
            text += "WIS: " + component.gameData.m_commonData.m_cleverness.ToString() + "\n";
            text += "SPD: " + component.gameData.m_commonData.m_rapidity.ToString() + "\n";
            text += "\n";
            text += string.Format(Language.GetString("field_enemy_label"), component.gameData.m_level + 1, component.Name);
            __instance.m_text.text = text;
            __instance.m_text.anchor = TextAnchor.LowerCenter;
            __instance.m_text.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            __instance.m_text.anchor = TextAnchor.UpperCenter;
            __instance.m_text.transform.localPosition = new Vector3(0, 0.145f, 0);
        }
    }
}
