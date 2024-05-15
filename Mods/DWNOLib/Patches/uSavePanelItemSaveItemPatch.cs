using DWNOLib.Library;
using HarmonyLib;
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
            __instance.m_playerNameText.text = (string)data["SaveFileInfo"]["m_name"];
            __instance.m_playerIcon.sprite = __instance.m_playerIconSprites[(int)data["SaveFileInfo"]["m_gender"]];
            __instance.m_tamarLavelText.text = ((int)data["SaveFileInfo"]["m_level"]).ToString();

            ParameterDigimonData @params0 = ParameterDigimonData.GetParam((uint)data["SaveFileInfo"]["m_partnerR"]);
            System.Action<Texture, int> action0 = __instance.IconCallback;
            __instance.LoadIcon(0, @params0.m_mdlName, action0);
            ParameterDigimonData @params1 = ParameterDigimonData.GetParam((uint)data["SaveFileInfo"]["m_partnerL"]);
            System.Action<Texture, int> action1 = __instance.IconCallback;
            __instance.LoadIcon(1, @params1.m_mdlName, action1);

            AppInfo.MAP _mapNo = (AppInfo.MAP)(int)data["SaveFileInfo"]["m_mapNo"];
            uint m_areaNo = (uint)data["SaveFileInfo"]["m_areaNo"];
            ParameterAreaName param = ParameterAreaName.GetParam(_mapNo, m_areaNo);
            uint lang_code = param.m_LanguageCode;
            if (_mapNo == AppInfo.MAP.TOWN && m_areaNo == 10)
            {
                if ((bool)data["SaveFileInfo"]["TownGradeUp"])
                    lang_code = Language.makeHash("area_name_0110_2");
                else
                    lang_code = Language.makeHash("area_name_0110_1");
            }
            __instance.m_areaText.text = Language.GetString(lang_code);
            
            double playtime = (double)data["SaveFileInfo"]["m_PlayTime"];
            int _hour = (int)(playtime / 3600.0);
            int _minute = (int)((playtime % 3600.0) / 60.0);
            int _second = (int)(playtime % 60.0);
            __instance.m_playTimeText.text = Language.GetString("tamer_play_time") + "  " + _hour.ToString("D4") + ":" + _minute.ToString("D2") + ":" + _second.ToString("D2");

            System.DateTime timestamp = System.DateTime.FromBinary((long)data["SaveFileInfo"]["m_timeStamp"]);
            __instance.m_timeStampText.text = string.Format("{0:0000} / {1:00} / {2:00}  {3:00} : {4:00}", timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute);

            __instance.m_isExistData = true;
        }

        return false;
    }
}
