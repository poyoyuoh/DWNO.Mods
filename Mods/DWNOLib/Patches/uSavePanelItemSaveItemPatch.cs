using DWNOLib.Library;
using HarmonyLib;
using System;
using System.Text.Json.Nodes;
using UnityEngine;

namespace DWNOLib.Patches;
internal class uSavePanelItemSaveItemPatch
{
    [HarmonyPatch(typeof(uSavePanelItemSaveItem), "Load")]
    [HarmonyPrefix]
    private static bool uSavePanelItemSaveItem_Load_Prefix(uSavePanelItemSaveItem __instance)
    {
        int i = __instance.transform.GetSiblingIndex();

        if (!StorageDataLib.IsExistGameData(i))
        {
            __instance.m_item[1].SetActive(true);
            __instance.m_item[0].SetActive(false);
            __instance.m_isExistData = false;
            return true;
        }
        else
        {
            __instance.m_item[0].SetActive(true);
            __instance.m_item[1].SetActive(false);

            JsonNode data = StorageDataLib.ReadSaveData(i);
            __instance.m_playerNameText.text = (string)data["playerData"]["m_Name"];
            //__instance.m_areaText.text = (string)data["playerData"]["m_Name"];
            //__instance.m_playTimeText.text = (string)data["playerData"]["m_Name"];
            //__instance.m_timeStampText.text = (string)data["playerData"]["m_Name"];
            __instance.m_tamarLavelText.text = ((int)data["playerData"]["m_level"]).ToString();
            __instance.m_playerIcon.sprite = __instance.m_playerIconSprites[(int)data["playerData"]["m_Gender"]];
            ParameterDigimonData @params0 = ParameterDigimonData.GetParam((uint)data["partnerData0"]["m_commonData"]["m_baseID"]);
            Action<Texture, int> action0 = __instance.IconCallback;
            __instance.LoadIcon(0, @params0.m_mdlName, action0);
            ParameterDigimonData @params1 = ParameterDigimonData.GetParam((uint)data["partnerData1"]["m_commonData"]["m_baseID"]);
            Action<Texture, int> action1 = __instance.IconCallback;
            __instance.LoadIcon(1, @params1.m_mdlName, action1);
            __instance.m_isExistData = true;
        }

        return false;
    }
}
