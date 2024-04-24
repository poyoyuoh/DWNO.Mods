using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;

namespace DWNOLib.Library;
public class StorageDataLib
{
    public static class StorageDataLimits
    {
        public static uint MAX_EVOLUTION_FLAG { get; private set; } = 16384u;

        public static uint max_condition_flag { get; set; } = 14u;

        public static void ResetLimits()
        {
            MAX_EVOLUTION_FLAG = 320u;

            StorageData.m_playerData.CreateEvolutionSkillFlag();
            StorageData.m_playerData.CreateEvolutionConditionFlag();
        }

        public static void RestoreLimits()
        {
            MAX_EVOLUTION_FLAG = 16384u;

            CBitFlag m_EvolutionFlag = new CBitFlag(MAX_EVOLUTION_FLAG);
            for (uint i = 0; i < StorageData.m_playerData.m_EvolutionFlag.m_NumFlags; i++)
            {
                m_EvolutionFlag[i] = StorageData.m_playerData.m_EvolutionFlag[i];
            }
            StorageData.m_playerData.m_EvolutionFlag = m_EvolutionFlag;

            CBitFlag[] m_EvolutionConditionFlag = new CBitFlag[MAX_EVOLUTION_FLAG];
            for (int i = 0; i < MAX_EVOLUTION_FLAG; i++)
            {
                m_EvolutionConditionFlag[(uint)i] = new CBitFlag(max_condition_flag);

                if (i < StorageData.m_playerData.m_EvolutionConditionFlag.Length)
                {
                    for (uint j = 0; j < StorageData.m_playerData.m_EvolutionConditionFlag[i].m_NumFlags; j++)
                    {
                        m_EvolutionConditionFlag[(uint)i][j] = StorageData.m_playerData.m_EvolutionConditionFlag[i][j];
                    }
                }
            }
            StorageData.m_playerData.m_EvolutionConditionFlag = m_EvolutionConditionFlag;
        }
    }

    private static Dictionary<string, object> buffer = new Dictionary<string, object>();

    public static bool IsExistSystemData()
    {
        string savepath = AppMainScript.m_instance.m_unitySaveDataPath + "SystemSaveData.json";
        return File.Exists(savepath);
    }

    public static JsonNode ReadSystemData()
    {
        string savepath = AppMainScript.m_instance.m_unitySaveDataPath + "SystemSaveData.json";
        string file = File.ReadAllText(savepath);
        return JsonNode.Parse(file);
    }

    public static bool WriteSystemData()
    {
        string jsonstring = JsonSerializer.Serialize(buffer, new JsonSerializerOptions { WriteIndented = true });
        string savepath = AppMainScript.m_instance.m_unitySaveDataPath + "SystemSaveData.json";
        File.WriteAllText(savepath, jsonstring);
        return true;
    }

    public static bool LoadSystemData()
    {
        JsonNode data = ReadSystemData();

        StorageData.m_SystemDataVersion = 2;

        StorageData.m_optionData.m_OptionData.m_isAgreeEula = (bool)data["m_OptionData"]["Others"]["m_isAgreeEula"];
        StorageData.m_optionData.m_OptionData.m_isAgreeKPI = (bool)data["m_OptionData"]["Others"]["m_isAgreeKPI"];
        StorageData.m_optionData.m_OptionData.m_isAgreePrivacyPolicy = (bool)data["m_OptionData"]["Others"]["m_isAgreePrivacyPolicy"];
        StorageData.m_optionData.m_OptionData.m_isLoadedAgreeData = (bool)data["m_OptionData"]["Others"]["m_isLoadedAgreeData"];
        StorageData.m_optionData.m_OptionData.m_IsSavedFlag = (bool)data["m_OptionData"]["Others"]["m_IsSavedFlag"];
        StorageData.m_optionData.m_OptionData.m_LangCt = (OptionData.LanguageCT)(int)data["m_OptionData"]["Others"]["m_LangCt"];

        StorageData.m_optionData.m_OptionData.m_BgmVolume = (int)data["m_OptionData"]["SystemSettings"]["m_BgmVolume"];
        StorageData.m_optionData.m_OptionData.m_VoiceVolume = (int)data["m_OptionData"]["SystemSettings"]["m_VoiceVolume"];
        StorageData.m_optionData.m_OptionData.m_SeVolume = (int)data["m_OptionData"]["SystemSettings"]["m_SeVolume"];
        StorageData.m_optionData.m_OptionData.VoiceLangKind = (CriSoundManager.VoiceLanguageKind)(int)data["m_OptionData"]["SystemSettings"]["VoiceLangKind"];
        StorageData.m_optionData.m_OptionData.m_CameraUpDown = (int)data["m_OptionData"]["SystemSettings"]["m_CameraUpDown"];
        StorageData.m_optionData.m_OptionData.m_CameraLeftRight = (int)data["m_OptionData"]["SystemSettings"]["m_CameraLeftRight"];
        StorageData.m_optionData.m_OptionData.m_CameraSensitivity = (int)data["m_OptionData"]["SystemSettings"]["m_CameraSensitivity"];

        StorageData.m_optionData.m_OptionData.m_Resolution = (byte)data["m_OptionData"]["Graphics"]["m_Resolution"];
        StorageData.m_optionData.m_OptionData.m_ScreenMode = (byte)data["m_OptionData"]["Graphics"]["m_ScreenMode"];
        StorageData.m_optionData.m_OptionData.m_Antialiasing = (byte)data["m_OptionData"]["Graphics"]["m_Antialiasing"];
        StorageData.m_optionData.m_OptionData.m_DepthOfField = (byte)data["m_OptionData"]["Graphics"]["m_DepthOfField"];

        data["m_OptionData"]["KeyConfig"]["m_KeyboardType"] = StorageData.m_optionData.m_OptionData.m_KeyboardType;

        for (int i = 0; i < StorageData.m_optionData.m_OptionData.m_Key.Count; i++)
        {
            StorageData.m_optionData.m_OptionData.m_Key[i] = (short)data["m_OptionData"]["KeyConfig"]["m_key"][i.ToString()];
        }

        for (int i = 0; i < StorageData.m_optionData.m_OptionData.m_Mouse.Count; i++)
        {
            StorageData.m_optionData.m_OptionData.m_Mouse[i] = (short)data["m_OptionData"]["KeyConfig"]["m_Mouse"][i.ToString()];
        }

        return true;
    }

    public static void SaveSystemData()
    {
        buffer.Clear();
        
        Dictionary<string, object> m_OptionData = new Dictionary<string, object>();

        Dictionary<string, object> Others = new Dictionary<string, object>();
        Others["m_isAgreeEula"] = StorageData.m_optionData.m_OptionData.m_isAgreeEula;
        Others["m_isAgreeKPI"] = StorageData.m_optionData.m_OptionData.m_isAgreeKPI;
        Others["m_isAgreePrivacyPolicy"] = StorageData.m_optionData.m_OptionData.m_isAgreePrivacyPolicy;
        Others["m_isLoadedAgreeData"] = StorageData.m_optionData.m_OptionData.m_isLoadedAgreeData;
        Others["m_IsSavedFlag"] = StorageData.m_optionData.m_OptionData.m_IsSavedFlag;
        Others["m_LangCt"] = StorageData.m_optionData.m_OptionData.m_LangCt;
        m_OptionData["Others"] = Others;

        Dictionary<string, object> SystemSettings = new Dictionary<string, object>();
        SystemSettings["m_BgmVolume"] = StorageData.m_optionData.m_OptionData.m_BgmVolume;
        SystemSettings["m_VoiceVolume"] = StorageData.m_optionData.m_OptionData.m_VoiceVolume;
        SystemSettings["m_SeVolume"] = StorageData.m_optionData.m_OptionData.m_SeVolume;
        SystemSettings["VoiceLangKind"] = (int)StorageData.m_optionData.m_OptionData.VoiceLangKind;
        SystemSettings["m_CameraUpDown"] = StorageData.m_optionData.m_OptionData.m_CameraUpDown;
        SystemSettings["m_CameraLeftRight"] = StorageData.m_optionData.m_OptionData.m_CameraLeftRight;
        SystemSettings["m_CameraSensitivity"] = StorageData.m_optionData.m_OptionData.m_CameraSensitivity;
        m_OptionData["SystemSettings"] = SystemSettings;

        Dictionary<string, object> Graphics = new Dictionary<string, object>();
        Graphics["m_Resolution"] = StorageData.m_optionData.m_OptionData.m_Resolution;
        Graphics["m_ScreenMode"] = StorageData.m_optionData.m_OptionData.m_ScreenMode;
        Graphics["m_Antialiasing"] = StorageData.m_optionData.m_OptionData.m_Antialiasing;
        Graphics["m_DepthOfField"] = StorageData.m_optionData.m_OptionData.m_DepthOfField;
        m_OptionData["Graphics"] = Graphics;

        Dictionary<string, object> KeyConfig = new Dictionary<string, object>();
        KeyConfig["m_KeyboardType"] = StorageData.m_optionData.m_OptionData.m_KeyboardType;
        Dictionary<int, object> m_key = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_optionData.m_OptionData.m_Key.Count; i++)
        {
            m_key[i] = StorageData.m_optionData.m_OptionData.m_Key[i];
        }
        KeyConfig["m_key"] = m_key;
        Dictionary<int, object> m_Mouse = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_optionData.m_OptionData.m_Mouse.Count; i++)
        {
            m_Mouse[i] = StorageData.m_optionData.m_OptionData.m_Mouse[i];
        }
        KeyConfig["m_Mouse"] = m_Mouse;
        m_OptionData["KeyConfig"] = KeyConfig;

        buffer["m_OptionData"] = m_OptionData;
    }

    public static bool IsExistGameData(int slot_no)
    {
        string savepath = AppMainScript.m_instance.GetSaveFilePath(slot_no);
        savepath = savepath.Remove(savepath.Length - 4);
        return File.Exists(savepath + "json");
    }

    public static JsonNode ReadSaveData(int slot_no)
    {
        string savepath = AppMainScript.m_instance.GetSaveFilePath(slot_no);
        savepath = savepath.Remove(savepath.Length - 4);
        string file = File.ReadAllText(savepath + "json");
        return JsonNode.Parse(file);
    }

    public static bool WriteSaveData(int slot_no)
    {
        string jsonstring = JsonSerializer.Serialize(buffer, new JsonSerializerOptions { WriteIndented = true });
        string savepath = AppMainScript.m_instance.GetSaveFilePath(slot_no);
        savepath = savepath.Remove(savepath.Length - 4);
        File.WriteAllText(savepath + "json", jsonstring);
        return true;
    }

    public static bool LoadSaveData(int slot_no)
    {
        JsonNode data = ReadSaveData(slot_no);

        StorageData.m_GameDataVersion = 5;

        if (!LoadPlayerData(data))
            return false;

        for (int i = 0; i < StorageData.m_playerData.m_partners.Length; i++)
        {
            if (!LoadPartnerData(i, data))
                return false;
        }

        return true;
    }

    public static void SaveSaveData()
    {
        buffer.Clear();

        SavePlayerData(buffer);
        for (int i = 0; i < StorageData.m_playerData.m_partners.Length; i++)
        {
            SavePartnerData(i, buffer);
        }
    }

    private static bool LoadPlayerData(JsonNode data)
    {
        StorageData.m_playerData.m_Name = (string)data["playerData"]["m_Name"];
        StorageData.m_playerData.m_Gender = (int)data["playerData"]["m_Gender"];
        StorageData.m_playerData.m_Money = (uint)data["playerData"]["m_Money"];
        StorageData.m_playerData.m_DailyQuestPoint = (uint)data["playerData"]["m_DailyQuestPoint"];
        StorageData.m_playerData.m_Coin = (uint)data["playerData"]["m_Coin"];
        StorageData.m_playerData.m_exp = (int)data["playerData"]["m_exp"];
        StorageData.m_playerData.m_exp_walk = (int)data["playerData"]["m_exp_walk"];
        StorageData.m_playerData.m_exp_walk_distance = (float)data["playerData"]["m_exp_walk_distance"];
        StorageData.m_playerData.m_level = (int)data["playerData"]["m_level"];
        StorageData.m_playerData.m_skillPoint = (int)data["playerData"]["m_skillPoint"];
        StorageData.m_playerData.m_isUseExe = (bool)data["playerData"]["m_isUseExe"];
        StorageData.m_playerData.m_battleCameraType = (int)data["playerData"]["m_battleCameraType"];
        StorageData.m_playerData.m_extraJoglessWaitTime = (float)data["playerData"]["m_extraJoglessWaitTime"];
        StorageData.m_playerData.m_TownDevelopPoint = (uint)data["playerData"]["m_TownDevelopPoint"];
        StorageData.m_playerData.m_tent_item_id = (uint)data["playerData"]["m_tent_item_id"];
        StorageData.m_playerData.m_tent_hp = (int)data["playerData"]["m_tent_hp"];
        StorageData.m_playerData.m_tent_hp_max = (int)data["playerData"]["m_tent_hp_max"];
        StorageData.m_playerData.m_educationTotalCount = (int)data["playerData"]["m_educationTotalCount"];
        StorageData.m_playerData.m_bestHp = (int)data["playerData"]["m_bestHp"];
        StorageData.m_playerData.m_bestMp = (int)data["playerData"]["m_bestMp"];
        StorageData.m_playerData.m_bestForcefulness = (int)data["playerData"]["m_bestForcefulness"];
        StorageData.m_playerData.m_bestRobustness = (int)data["playerData"]["m_bestRobustness"];
        StorageData.m_playerData.m_bestCleverness = (int)data["playerData"]["m_bestCleverness"];
        StorageData.m_playerData.m_bestRapidity = (int)data["playerData"]["m_bestRapidity"];
        StorageData.m_playerData.m_bestAge = (int)data["playerData"]["m_bestAge"];
        StorageData.m_playerData.m_bestForcefulness = (int)data["playerData"]["m_bestForcefulness"];

        for (uint i = 0; i < StorageData.m_playerData.m_TamerSkillFlag.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_TamerSkillFlag[i] = (bool)data["playerData"]["m_TamerSkillFlag"][i.ToString()];
        }

        StorageData.m_playerData.m_partnersTrust = (int)data["playerData"]["m_partnersTrust"];
        StorageData.m_playerData.m_educationTime = (float)data["playerData"]["m_educationTime"];

        for (int i = 0; i < StorageData.m_playerData.m_useDlcEgs.Count; i++)
        {
            StorageData.m_playerData.m_useDlcEgs[i] = (bool)data["playerData"]["m_useDlcEgs"][i.ToString()];
        }

        for (uint i = 0; i < StorageData.m_playerData.m_AttacSkillkFlag.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_AttacSkillkFlag[i] = (bool)data["playerData"]["m_AttacSkillkFlag"][i.ToString()];
        }

        for (int i = 0; i < AppMainScript.parameterManager.m_DigimonID_LIST.Count; i++)
        {
            if (i >= StorageData.m_playerData.m_EvolutionConditionFlag.Length)
                break;

            for (uint j = 0; j < StorageDataLimits.max_condition_flag; j++)
            {
                if (data["playerData"]["m_EvolutionConditionFlag"][AppMainScript.parameterManager.m_DigimonID_LIST[i].ToString()] != null)
                    StorageData.m_playerData.m_EvolutionConditionFlag[i][j] = (bool)data["playerData"]["m_EvolutionConditionFlag"][AppMainScript.parameterManager.m_DigimonID_LIST[i].ToString()][j.ToString()];
            }
        }

        StorageData.m_playerData.m_RebornCt = (uint)data["playerData"]["m_RebornCt"];

        for (uint i = 0; i < StorageData.m_playerData.m_TutorialAlready.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_TutorialAlready[i] = (bool)data["playerData"]["m_TutorialAlready"][i.ToString()];
        }

        StorageData.m_playerData.m_NumReleaseEvoFlag = (uint)data["playerData"]["m_NumReleaseEvoFlag"];

        // Update001
        StorageData.m_playerData.BattlePanelItemSort = (int)data["playerData"]["BattlePanelItemSort"];
        StorageData.m_playerData.SalePanelItemSort = (int)data["playerData"]["SalePanelItemSort"];

        for (int i = 0; i < StorageData.m_playerData.CarePanelItemSort.Count; i++)
        {
            StorageData.m_playerData.CarePanelItemSort[i] = (int)data["playerData"]["CarePanelItemSort"][i.ToString()];
        }

        for (int i = 0; i < StorageData.m_playerData.StoragePanelItemSort.Count; i++)
        {
            StorageData.m_playerData.StoragePanelItemSort[i] = (int)data["playerData"]["StoragePanelItemSort"][i.ToString()];
        }

        // Update003
        StorageData.m_playerData.m_IsRetryNewGame = (bool)data["playerData"]["m_IsRetryNewGame"];

        // Complete
        StorageData.m_playerData.Difficulty = (AppInfo.Difficulty)(int)data["playerData"]["Difficulty"];
        StorageData.m_playerData.m_dayExECount = (uint)data["playerData"]["m_dayExECount"];
        StorageData.m_playerData.m_StealthTime = (float)data["playerData"]["m_StealthTime"];

        return true;
    }

    private static void SavePlayerData(Dictionary<string, object> buffer)
    {
        Dictionary<string, object> playerData = new Dictionary<string, object>();
        playerData["m_Name"] = StorageData.m_playerData.m_Name;
        playerData["m_Gender"] = StorageData.m_playerData.m_Gender;
        playerData["m_Money"] = StorageData.m_playerData.m_Money;
        playerData["m_DailyQuestPoint"] = StorageData.m_playerData.m_DailyQuestPoint;
        playerData["m_Coin"] = StorageData.m_playerData.m_Coin;
        playerData["m_exp"] = StorageData.m_playerData.m_exp;
        playerData["m_exp_walk"] = StorageData.m_playerData.m_exp_walk;
        playerData["m_exp_walk_distance"] = StorageData.m_playerData.m_exp_walk_distance;
        playerData["m_level"] = StorageData.m_playerData.m_level;
        playerData["m_skillPoint"] = StorageData.m_playerData.m_skillPoint;
        playerData["m_isUseExe"] = StorageData.m_playerData.m_isUseExe;
        playerData["m_battleCameraType"] = StorageData.m_playerData.m_battleCameraType;
        playerData["m_extraJoglessWaitTime"] = StorageData.m_playerData.m_extraJoglessWaitTime;
        playerData["m_TownDevelopPoint"] = StorageData.m_playerData.m_TownDevelopPoint;
        playerData["m_tent_item_id"] = StorageData.m_playerData.m_tent_item_id;
        playerData["m_tent_hp"] = StorageData.m_playerData.m_tent_hp;
        playerData["m_tent_hp_max"] = StorageData.m_playerData.m_tent_hp_max;
        playerData["m_educationTotalCount"] = StorageData.m_playerData.m_educationTotalCount;
        playerData["m_bestHp"] = StorageData.m_playerData.m_bestHp;
        playerData["m_bestMp"] = StorageData.m_playerData.m_bestMp;
        playerData["m_bestForcefulness"] = StorageData.m_playerData.m_bestForcefulness;
        playerData["m_bestRobustness"] = StorageData.m_playerData.m_bestRobustness;
        playerData["m_bestCleverness"] = StorageData.m_playerData.m_bestCleverness;
        playerData["m_bestRapidity"] = StorageData.m_playerData.m_bestRapidity;
        playerData["m_bestAge"] = StorageData.m_playerData.m_bestAge;
        playerData["m_bestForcefulness"] = StorageData.m_playerData.m_bestForcefulness;

        Dictionary<uint, bool> m_TamerSkillFlag = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_TamerSkillFlag.m_NumFlags; i++)
        {
            m_TamerSkillFlag[i] = StorageData.m_playerData.m_TamerSkillFlag[i];
        }
        playerData["m_TamerSkillFlag"] = m_TamerSkillFlag;

        playerData["m_partnersTrust"] = StorageData.m_playerData.m_partnersTrust;
        playerData["m_educationTime"] = StorageData.m_playerData.m_educationTime;

        Dictionary<int, bool> m_useDlcEgs = new Dictionary<int, bool>();
        for (int i = 0; i < StorageData.m_playerData.m_useDlcEgs.Length; i++)
        {
            m_useDlcEgs[i] = StorageData.m_playerData.m_useDlcEgs[i];
        }
        playerData["m_useDlcEgs"] = m_useDlcEgs;

        Dictionary<uint, bool> m_AttacSkillkFlag = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_AttacSkillkFlag.m_NumFlags; i++)
        {
            m_AttacSkillkFlag[i] = StorageData.m_playerData.m_AttacSkillkFlag[i];
        }
        playerData["m_AttacSkillkFlag"] = m_AttacSkillkFlag;

        Dictionary<uint, Dictionary<uint, bool>> m_EvolutionConditionFlag = new Dictionary<uint, Dictionary<uint, bool>>();
        for (int i = 0; i < AppMainScript.parameterManager.m_DigimonID_LIST.Count; i++)
        {
            if (i >= StorageData.m_playerData.m_EvolutionConditionFlag.Length)
                break;

            Dictionary<uint, bool> m_EvolutionConditionFlag2 = new Dictionary<uint, bool>();
            for (uint j = 0; j < StorageDataLimits.max_condition_flag; j++)
            {
                m_EvolutionConditionFlag2[j] = StorageData.m_playerData.m_EvolutionConditionFlag[i][j];
            }
            m_EvolutionConditionFlag[AppMainScript.parameterManager.m_DigimonID_LIST[i]] = m_EvolutionConditionFlag2;
        }
        playerData["m_EvolutionConditionFlag"] = m_EvolutionConditionFlag;

        playerData["m_RebornCt"] = StorageData.m_playerData.m_RebornCt;

        Dictionary<uint, bool> m_TutorialAlready = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_TutorialAlready.m_NumFlags; i++)
        {
            m_TutorialAlready[i] = StorageData.m_playerData.m_TutorialAlready[i];
        }
        playerData["m_TutorialAlready"] = m_TutorialAlready;

        playerData["m_NumReleaseEvoFlag"] = StorageData.m_playerData.m_NumReleaseEvoFlag;

        // Update001
        playerData["BattlePanelItemSort"] = StorageData.m_playerData.BattlePanelItemSort;
        playerData["SalePanelItemSort"] = StorageData.m_playerData.SalePanelItemSort;

        Dictionary<int, int> CarePanelItemSort = new Dictionary<int, int>();
        for (int i = 0; i < StorageData.m_playerData.CarePanelItemSort.Length; i++)
        {
            CarePanelItemSort[i] = StorageData.m_playerData.CarePanelItemSort[i];
        }
        playerData["CarePanelItemSort"] = CarePanelItemSort;

        Dictionary<int, int> StoragePanelItemSort = new Dictionary<int, int>();
        for (int i = 0; i < StorageData.m_playerData.StoragePanelItemSort.Length; i++)
        {
            StoragePanelItemSort[i] = StorageData.m_playerData.StoragePanelItemSort[i];
        }
        playerData["StoragePanelItemSort"] = StoragePanelItemSort;

        // Update003
        playerData["m_IsRetryNewGame"] = StorageData.m_playerData.m_IsRetryNewGame;

        // Complete
        playerData["Difficulty"] = (int)StorageData.m_playerData.Difficulty;
        playerData["m_dayExECount"] = StorageData.m_playerData.m_dayExECount;
        playerData["m_StealthTime"] = StorageData.m_playerData.m_StealthTime;

        buffer["playerData"] = playerData;
    }

    private static bool LoadPartnerData(int partner_no, JsonNode data)
    {
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_name = (string)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_name"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_baseID = (uint)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_baseID"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_weight = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_weight"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_hpMax = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_hpMax"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_hp = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_hp"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_mpMax = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_mpMax"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_mp = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_mp"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_forcefulness = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_forcefulness"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_robustness = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_robustness"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_cleverness = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_cleverness"];
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_rapidity = (int)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_rapidity"];

        ParameterDigimonData param = ParameterDigimonData.GetParam(StorageData.m_playerData.m_partners[partner_no].m_commonData.m_baseID);
        if (param != null)
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_personality = param.m_personality;

        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_commonData.m_attack.Length; i++)
        {
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_attack[i] = (uint)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_attack"][i.ToString()];
        }

        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas.Length; i++)
        {
            StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas[i].m_kind_index = (int)data["partnerData" + partner_no.ToString()]["m_educationDatas"][i.ToString()]["m_kind_index"];
            StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas[i].m_count = (int)data["partnerData" + partner_no.ToString()]["m_educationDatas"][i.ToString()]["m_count"];
        }

        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas.Length; i++)
        {
            StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_mapNo = (int)data["partnerData" + partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_mapNo"];
            StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_areaNo = (int)data["partnerData" + partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_areaNo"];
            float x = (float)data["partnerData" + partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_position.x"];
            float y = (float)data["partnerData" + partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_position.y"];
            float z = (float)data["partnerData" + partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_position.z"];
            StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_position = new Vector3(x, y, z);
        }

        // Lifecycle
        StorageData.m_playerData.m_partners[partner_no].m_bonds = (int)data["partnerData" + partner_no.ToString()]["m_bonds"];
        StorageData.m_playerData.m_partners[partner_no].m_age = (int)data["partnerData" + partner_no.ToString()]["m_age"];
        StorageData.m_playerData.m_partners[partner_no].m_time_from_age = (float)data["partnerData" + partner_no.ToString()]["m_time_from_age"];
        StorageData.m_playerData.m_partners[partner_no].m_time_from_birth = (float)data["partnerData" + partner_no.ToString()]["m_time_from_birth"];
        StorageData.m_playerData.m_partners[partner_no].m_curse = (int)data["partnerData" + partner_no.ToString()]["m_curse"];
        StorageData.m_playerData.m_partners[partner_no].m_lifetime = (float)data["partnerData" + partner_no.ToString()]["m_lifetime"];
        StorageData.m_playerData.m_partners[partner_no].m_breeding = (int)data["partnerData" + partner_no.ToString()]["m_breeding"];
        StorageData.m_playerData.m_partners[partner_no].m_mood = (int)data["partnerData" + partner_no.ToString()]["m_mood"];
        StorageData.m_playerData.m_partners[partner_no].m_satiety = (int)data["partnerData" + partner_no.ToString()]["m_satiety"];
        StorageData.m_playerData.m_partners[partner_no].m_fatigue = (int)data["partnerData" + partner_no.ToString()]["m_fatigue"];
        StorageData.m_playerData.m_partners[partner_no].m_FieldStatusEffect = (int)data["partnerData" + partner_no.ToString()]["m_FieldStatusEffect"];
        StorageData.m_playerData.m_partners[partner_no].m_trainingFailure = (int)data["partnerData" + partner_no.ToString()]["m_trainingFailure"];
        StorageData.m_playerData.m_partners[partner_no].m_battleWin = (int)data["partnerData" + partner_no.ToString()]["m_battleWin"];
        StorageData.m_playerData.m_partners[partner_no].m_mealTime = (float)data["partnerData" + partner_no.ToString()]["m_mealTime"];
        StorageData.m_playerData.m_partners[partner_no].m_toiletTime = (float)data["partnerData" + partner_no.ToString()]["m_toiletTime"];
        StorageData.m_playerData.m_partners[partner_no].m_sleepTime = (float)data["partnerData" + partner_no.ToString()]["m_sleepTime"];
        StorageData.m_playerData.m_partners[partner_no].m_isReqMeal = (bool)data["partnerData" + partner_no.ToString()]["m_isReqMeal"];
        StorageData.m_playerData.m_partners[partner_no].m_isReqToilet = (bool)data["partnerData" + partner_no.ToString()]["m_isReqToilet"];
        StorageData.m_playerData.m_partners[partner_no].m_isReqSleep = (bool)data["partnerData" + partner_no.ToString()]["m_isReqSleep"];
        StorageData.m_playerData.m_partners[partner_no].m_lastBaitTime = (float)data["partnerData" + partner_no.ToString()]["m_lastBaitTime"];
        StorageData.m_playerData.m_partners[partner_no].m_subSatietyTime = (float)data["partnerData" + partner_no.ToString()]["m_subSatietyTime"];
        StorageData.m_playerData.m_partners[partner_no].m_subSatietyCt = (int)data["partnerData" + partner_no.ToString()]["m_subSatietyCt"];
        StorageData.m_playerData.m_partners[partner_no].m_mealTimeZone = (PlayerData.PartnerData.MealTimeZone)(int)data["partnerData" + partner_no.ToString()]["m_mealTimeZone"];
        StorageData.m_playerData.m_partners[partner_no].m_putToSleepTime = (float)data["partnerData" + partner_no.ToString()]["m_putToSleepTime"];
        StorageData.m_playerData.m_partners[partner_no].m_wakeUpTime = (float)data["partnerData" + partner_no.ToString()]["m_wakeUpTime"];
        StorageData.m_playerData.m_partners[partner_no].m_GenerationNum = (uint)data["partnerData" + partner_no.ToString()]["m_GenerationNum"];
        StorageData.m_playerData.m_partners[partner_no].m_favoriteAddingId = (uint)data["partnerData" + partner_no.ToString()]["m_favoriteAddingId"];
        StorageData.m_playerData.m_partners[partner_no].m_TerrainImpactTimer = (float)data["partnerData" + partner_no.ToString()]["m_TerrainImpactTimer"];

        // Diathesis
        StorageData.m_playerData.m_partners[partner_no].m_diathesisHp = (int)data["partnerData" + partner_no.ToString()]["m_diathesisHp"];
        StorageData.m_playerData.m_partners[partner_no].m_diathesisMp = (int)data["partnerData" + partner_no.ToString()]["m_diathesisMp"];
        StorageData.m_playerData.m_partners[partner_no].m_diathesisForcefulness = (int)data["partnerData" + partner_no.ToString()]["m_diathesisForcefulness"];
        StorageData.m_playerData.m_partners[partner_no].m_diathesisRobustness = (int)data["partnerData" + partner_no.ToString()]["m_diathesisRobustness"];
        StorageData.m_playerData.m_partners[partner_no].m_diathesisCleverness = (int)data["partnerData" + partner_no.ToString()]["m_diathesisCleverness"];
        StorageData.m_playerData.m_partners[partner_no].m_diathesisRapidity = (int)data["partnerData" + partner_no.ToString()]["m_diathesisRapidity"];

        // Chip
        StorageData.m_playerData.m_partners[partner_no].m_chipHpMax = (int)data["partnerData" + partner_no.ToString()]["m_chipHpMax"];
        StorageData.m_playerData.m_partners[partner_no].m_chipMpMax = (int)data["partnerData" + partner_no.ToString()]["m_chipMpMax"];
        StorageData.m_playerData.m_partners[partner_no].m_chipForcefulness = (int)data["partnerData" + partner_no.ToString()]["m_chipForcefulness"];
        StorageData.m_playerData.m_partners[partner_no].m_chipRobustness = (int)data["partnerData" + partner_no.ToString()]["m_chipRobustness"];
        StorageData.m_playerData.m_partners[partner_no].m_chipCleverness = (int)data["partnerData" + partner_no.ToString()]["m_chipCleverness"];
        StorageData.m_playerData.m_partners[partner_no].m_chipRapidity = (int)data["partnerData" + partner_no.ToString()]["m_chipRapidity"];

        // EvolutionBefore
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeBaseId = (uint)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeBaseId"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeHp = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeHp"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeMp = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeMp"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeForcefulness = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeForcefulness"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRobustness = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeRobustness"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeCleverness = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeCleverness"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRapidity = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforeRapidity"];
        StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforemWeight = (int)data["partnerData" + partner_no.ToString()]["m_EvolutionBeforemWeight"];

        // MealCorrection
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHp = (int)data["partnerData" + partner_no.ToString()]["m_mealCorrectionHp"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMp = (int)data["partnerData" + partner_no.ToString()]["m_mealCorrectionMp"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulness = (int)data["partnerData" + partner_no.ToString()]["m_mealCorrectionForcefulness"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustness = (int)data["partnerData" + partner_no.ToString()]["m_mealCorrectionRobustness"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionVleverness = (int)data["partnerData" + partner_no.ToString()]["m_mealCorrectionVleverness"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidity = (int)data["partnerData" + partner_no.ToString()]["m_mealCorrectionRapidity"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHpEffectTime = (float)data["partnerData" + partner_no.ToString()]["m_mealCorrectionHpEffectTime"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMpEffectTime = (float)data["partnerData" + partner_no.ToString()]["m_mealCorrectionMpEffectTime"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulnessEffectTime = (float)data["partnerData" + partner_no.ToString()]["m_mealCorrectionForcefulnessEffectTime"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustnessEffectTime = (float)data["partnerData" + partner_no.ToString()]["m_mealCorrectionRobustnessEffectTime"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionClevernessEffectTime = (float)data["partnerData" + partner_no.ToString()]["m_mealCorrectionClevernessEffectTime"];
        StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidityEffectTime = (float)data["partnerData" + partner_no.ToString()]["m_mealCorrectionRapidityEffectTime"];

        // Battle
        StorageData.m_playerData.m_partners[partner_no].m_BattleRange = (PlayerData.PartnerData.BattleRangeType)(int)data["partnerData" + partner_no.ToString()]["m_BattleRange"];
        StorageData.m_playerData.m_partners[partner_no].m_BattlePolicy = (PlayerData.PartnerData.BattlePolicyType)(int)data["partnerData" + partner_no.ToString()]["m_BattlePolicy"];
        StorageData.m_playerData.m_partners[partner_no].m_BattleUseMpPolicy = (PlayerData.PartnerData.BattleUseMpType)(int)data["partnerData" + partner_no.ToString()]["m_BattleUseMpPolicy"];

        // Genealogy
        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_HistoryArray.Length; i++)
        {
            int year = (int)data["partnerData" + partner_no.ToString()]["m_HistoryArray"][i.ToString()]["m_Year"];
            int day = (int)data["partnerData" + partner_no.ToString()]["m_HistoryArray"][i.ToString()]["m_Day"];
            uint id = (uint)data["partnerData" + partner_no.ToString()]["m_HistoryArray"][i.ToString()]["m_DigimonID"];
            StorageData.m_playerData.m_partners[partner_no].m_HistoryArray[i] = new PlayerData.PartnerData.HistoryData(year, day, id);
        }

        for (uint i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock[i] = (bool)data["partnerData" + partner_no.ToString()]["m_EvolutionBlock"][i.ToString()];
        }

        // Update001
        StorageData.m_playerData.m_partners[partner_no].m_prevLifeCycleUpdateTime = (float)data["partnerData" + partner_no.ToString()]["m_prevLifeCycleUpdateTime"];
        StorageData.m_playerData.m_partners[partner_no].m_LifeCycleMessageFlag = (int)data["partnerData" + partner_no.ToString()]["m_LifeCycleMessageFlag"];

        // Complete
        StorageData.m_playerData.m_partners[partner_no].m_commonData.m_IsDefaultName = (bool)data["partnerData" + partner_no.ToString()]["m_commonData"]["m_IsDefaultName"];

        return true;
    }

    private static void SavePartnerData(int partner_no, Dictionary<string, object> buffer)
    {
        Dictionary<string, object> partnerData = new Dictionary<string, object>();

        Dictionary<string, object> m_commonData = new Dictionary<string, object>();
        m_commonData["m_name"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_name;
        m_commonData["m_baseID"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_baseID;
        m_commonData["m_weight"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_weight;
        m_commonData["m_hpMax"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_hpMax;
        m_commonData["m_hp"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_hp;
        m_commonData["m_mpMax"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_mpMax;
        m_commonData["m_mp"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_mp;
        m_commonData["m_forcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_forcefulness;
        m_commonData["m_robustness"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_robustness;
        m_commonData["m_cleverness"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_cleverness;
        m_commonData["m_rapidity"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_rapidity;

        Dictionary<int, uint> m_attack = new Dictionary<int, uint>();
        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_commonData.m_attack.Length; i++)
        {
            m_attack[i] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_attack[i];
        }
        m_commonData["m_attack"] = m_attack;

        Dictionary<int, object> m_educationDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas.Length; i++)
        {
            Dictionary<string, object> educationData = new Dictionary<string, object>();
            educationData["m_kind_index"] = StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas[i].m_kind_index;
            educationData["m_count"] = StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas[i].m_count;
            m_educationDatas[i] = educationData;
        }
        partnerData["m_educationDatas"] = m_educationDatas;

        Dictionary<int, object> m_nokusoData = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas.Length; i++)
        {
            Dictionary<string, object> nokusoData = new Dictionary<string, object>();
            nokusoData["m_mapNo"] = StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_mapNo;
            nokusoData["m_areaNo"] = StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_areaNo;
            nokusoData["m_position.x"] = StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_position.x;
            nokusoData["m_position.y"] = StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_position.y;
            nokusoData["m_position.z"] = StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_position.z;
            m_nokusoData[i] = nokusoData;
        }
        partnerData["m_nokusoData"] = m_nokusoData;

        // Lifecycle
        partnerData["m_bonds"] = StorageData.m_playerData.m_partners[partner_no].m_bonds;
        partnerData["m_age"] = StorageData.m_playerData.m_partners[partner_no].m_age;
        partnerData["m_time_from_age"] = StorageData.m_playerData.m_partners[partner_no].m_time_from_age;
        partnerData["m_time_from_birth"] = StorageData.m_playerData.m_partners[partner_no].m_time_from_birth;
        partnerData["m_curse"] = StorageData.m_playerData.m_partners[partner_no].m_curse;
        partnerData["m_lifetime"] = StorageData.m_playerData.m_partners[partner_no].m_lifetime;
        partnerData["m_breeding"] = StorageData.m_playerData.m_partners[partner_no].m_breeding;
        partnerData["m_mood"] = StorageData.m_playerData.m_partners[partner_no].m_mood;
        partnerData["m_satiety"] = StorageData.m_playerData.m_partners[partner_no].m_satiety;
        partnerData["m_fatigue"] = StorageData.m_playerData.m_partners[partner_no].m_fatigue;
        partnerData["m_FieldStatusEffect"] = StorageData.m_playerData.m_partners[partner_no].m_FieldStatusEffect;
        partnerData["m_trainingFailure"] = StorageData.m_playerData.m_partners[partner_no].m_trainingFailure;
        partnerData["m_battleWin"] = StorageData.m_playerData.m_partners[partner_no].m_battleWin;
        partnerData["m_mealTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealTime;
        partnerData["m_toiletTime"] = StorageData.m_playerData.m_partners[partner_no].m_toiletTime;
        partnerData["m_sleepTime"] = StorageData.m_playerData.m_partners[partner_no].m_sleepTime;
        partnerData["m_isReqMeal"] = StorageData.m_playerData.m_partners[partner_no].m_isReqMeal;
        partnerData["m_isReqToilet"] = StorageData.m_playerData.m_partners[partner_no].m_isReqToilet;
        partnerData["m_isReqSleep"] = StorageData.m_playerData.m_partners[partner_no].m_isReqSleep;
        partnerData["m_lastBaitTime"] = StorageData.m_playerData.m_partners[partner_no].m_lastBaitTime;
        partnerData["m_subSatietyTime"] = StorageData.m_playerData.m_partners[partner_no].m_subSatietyTime;
        partnerData["m_subSatietyCt"] = StorageData.m_playerData.m_partners[partner_no].m_subSatietyCt;
        partnerData["m_mealTimeZone"] = (int)StorageData.m_playerData.m_partners[partner_no].m_mealTimeZone;
        partnerData["m_putToSleepTime"] = StorageData.m_playerData.m_partners[partner_no].m_putToSleepTime;
        partnerData["m_wakeUpTime"] = StorageData.m_playerData.m_partners[partner_no].m_wakeUpTime;
        partnerData["m_GenerationNum"] = StorageData.m_playerData.m_partners[partner_no].m_GenerationNum;
        partnerData["m_favoriteAddingId"] = StorageData.m_playerData.m_partners[partner_no].m_favoriteAddingId;
        partnerData["m_TerrainImpactTimer"] = StorageData.m_playerData.m_partners[partner_no].m_TerrainImpactTimer;

        // Diathesis
        partnerData["m_diathesisHp"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisHp;
        partnerData["m_diathesisMp"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisMp;
        partnerData["m_diathesisForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisForcefulness;
        partnerData["m_diathesisRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisRobustness;
        partnerData["m_diathesisCleverness"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisCleverness;
        partnerData["m_diathesisRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisRapidity;

        // Chip
        partnerData["m_chipHpMax"] = StorageData.m_playerData.m_partners[partner_no].m_chipHpMax;
        partnerData["m_chipMpMax"] = StorageData.m_playerData.m_partners[partner_no].m_chipMpMax;
        partnerData["m_chipForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_chipForcefulness;
        partnerData["m_chipRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_chipRobustness;
        partnerData["m_chipCleverness"] = StorageData.m_playerData.m_partners[partner_no].m_chipCleverness;
        partnerData["m_chipRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_chipRapidity;

        // EvolutionBefore
        partnerData["m_EvolutionBeforeBaseId"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeBaseId;
        partnerData["m_EvolutionBeforeHp"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeHp;
        partnerData["m_EvolutionBeforeMp"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeMp;
        partnerData["m_EvolutionBeforeForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeForcefulness;
        partnerData["m_EvolutionBeforeRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRobustness;
        partnerData["m_EvolutionBeforeCleverness"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeCleverness;
        partnerData["m_EvolutionBeforeRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRapidity;
        partnerData["m_EvolutionBeforemWeight"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforemWeight;

        // MealCorrection
        partnerData["m_mealCorrectionHp"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHp;
        partnerData["m_mealCorrectionMp"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMp;
        partnerData["m_mealCorrectionForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulness;
        partnerData["m_mealCorrectionRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustness;
        partnerData["m_mealCorrectionVleverness"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionVleverness;
        partnerData["m_mealCorrectionRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidity;
        partnerData["m_mealCorrectionHpEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHpEffectTime;
        partnerData["m_mealCorrectionMpEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMpEffectTime;
        partnerData["m_mealCorrectionForcefulnessEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulnessEffectTime;
        partnerData["m_mealCorrectionRobustnessEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustnessEffectTime;
        partnerData["m_mealCorrectionClevernessEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionClevernessEffectTime;
        partnerData["m_mealCorrectionRapidityEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidityEffectTime;

        // Battle
        partnerData["m_BattleRange"] = (int)StorageData.m_playerData.m_partners[partner_no].m_BattleRange;
        partnerData["m_BattlePolicy"] = (int)StorageData.m_playerData.m_partners[partner_no].m_BattlePolicy;
        partnerData["m_BattleUseMpPolicy"] = (int)StorageData.m_playerData.m_partners[partner_no].m_BattleUseMpPolicy;

        // Genealogy
        Dictionary<int, object> m_HistoryArray = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_HistoryArray.Length; i++)
        {
            Dictionary<string, object> historyData = new Dictionary<string, object>();
            historyData["m_Year"] = StorageData.m_playerData.m_partners[partner_no].m_HistoryArray[i].m_Year;
            historyData["m_Day"] = StorageData.m_playerData.m_partners[partner_no].m_HistoryArray[i].m_Day;
            historyData["m_DigimonID"] = StorageData.m_playerData.m_partners[partner_no].m_HistoryArray[i].m_DigimonID;
            m_HistoryArray[i] = historyData;
        }
        partnerData["m_HistoryArray"] = m_HistoryArray;

        Dictionary<uint, bool> m_EvolutionBlock = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock.m_NumFlags; i++)
        {
            m_EvolutionBlock[i] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock[i];
        }
        partnerData["m_EvolutionBlock"] = m_EvolutionBlock;

        // Update001
        partnerData["m_prevLifeCycleUpdateTime"] = StorageData.m_playerData.m_partners[partner_no].m_prevLifeCycleUpdateTime;
        partnerData["m_LifeCycleMessageFlag"] = StorageData.m_playerData.m_partners[partner_no].m_LifeCycleMessageFlag;

        // Complete
        m_commonData["m_IsDefaultName"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_IsDefaultName;
        partnerData["m_commonData"] = m_commonData;

        buffer["partnerData" + partner_no.ToString()] = partnerData;
    }
}
