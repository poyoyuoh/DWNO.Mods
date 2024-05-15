using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;
using static CFlagSetTimer;
using static ShopItemData;

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

    public const int SYSTEM_SAVE_FILE_VERSION = 1;

    public const int GAME_SAVE_FILE_VERSION = 1;

    public static int LoadedSystemSaveFileVersion { get; set; } = GAME_SAVE_FILE_VERSION;

    public static int LoadedGameSaveFileVersion { get; set; } = GAME_SAVE_FILE_VERSION;

    private static Dictionary<string, object> system_buffer { get; set; } = new Dictionary<string, object>();

    private static Dictionary<string, object> game_buffer { get; set; } = new Dictionary<string, object>();

    #region System
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
        string jsonstring = JsonSerializer.Serialize(system_buffer, new JsonSerializerOptions { WriteIndented = true });
        string savepath = AppMainScript.m_instance.m_unitySaveDataPath + "SystemSaveData.json";
        File.WriteAllText(savepath, jsonstring);
        return true;
    }

    public static bool LoadSystemData()
    {
        JsonNode data = ReadSystemData();

        StorageData.m_SystemDataVersion = 2;

        LoadedSystemSaveFileVersion = (int)data["DWNOLib"]["SystemSaveFileVersion"];

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

        ScreenResolution.SetResolution((ScreenResolution.Resolution)StorageData.m_optionData.m_OptionData.m_Resolution, StorageData.m_optionData.m_OptionData.m_ScreenMode);
        return true;
    }

    public static void SaveSystemData()
    {
        system_buffer.Clear();

        Dictionary<string, object> dwnolib = new Dictionary<string, object>();
        dwnolib["PluginVersion"] = DWNOLib.PluginVersion;
        dwnolib["SystemSaveFileVersion"] = SYSTEM_SAVE_FILE_VERSION;
        system_buffer["DWNOLib"] = dwnolib;

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

        system_buffer["m_OptionData"] = m_OptionData;
    }
    #endregion

    #region Game
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
        string jsonstring = JsonSerializer.Serialize(game_buffer, new JsonSerializerOptions { WriteIndented = true });
        string savepath = AppMainScript.m_instance.GetSaveFilePath(slot_no);
        savepath = savepath.Remove(savepath.Length - 4);
        File.WriteAllText(savepath + "json", jsonstring);
        return true;
    }

    public static bool LoadSaveData(int slot_no)
    {
        JsonNode data = ReadSaveData(slot_no);

        StorageData.m_GameDataVersion = 5;

        LoadedGameSaveFileVersion = (int)data["DWNOLib"]["GameSaveFileVersion"];

        if (!LoadPlayerData(data))
            return false;

        if (!LoadPartnerData(data))
            return false;

        if (!LoadWorldData(data))
            return false;

        if (!LoadMapData(data))
            return false;

        if (!LoadItemPickPointData(data))
            return false;

        if (!LoadScenarioProgressData(data))
            return false;

        if (!LoadScenaioFlagSetTimer(data))
            return false;

        if (!LoadItemStorageData(data))
            return false;

        if (!LoadFixedTimeData(data))
            return false;

        if (!LoadGradeUpData(data))
            return false;

        if (!LoadMaterialData(data))
            return false;

        if (!LoadColosseumData(data))
            return false;

        if (!LoadFarmData(data))
            return false;

        if (!LoadTradeData(data))
            return false;

        if (!LoadTrainingData(data))
            return false;

        if (!LoadTrainingMenuData(data))
            return false;

        if (!LoadDigitalMessangerData(data))
            return false;

        if (!LoadPlayTimeData(data))
            return false;

        return true;
    }

    public static void SaveSaveData()
    {
        game_buffer.Clear();

        Dictionary<string, object> dwnolib = new Dictionary<string, object>();
        dwnolib["PluginVersion"] = DWNOLib.PluginVersion;
        dwnolib["GameSaveFileVersion"] = GAME_SAVE_FILE_VERSION;
        game_buffer["DWNOLib"] = dwnolib;

        SaveSaveFileInfo(game_buffer);
        SavePlayerData(game_buffer);
        SavePartnerData(game_buffer);
        SaveWorldData(game_buffer);
        SaveMapData(game_buffer);
        SaveItemPickPointData(game_buffer);
        SaveScenarioProgressData(game_buffer);
        SaveScenaioFlagSetTimer(game_buffer);
        SaveItemStorageData(game_buffer);
        SaveFixedTimeData(game_buffer);
        SaveGradeUpData(game_buffer);
        SaveMaterialData(game_buffer);
        SaveColosseumData(game_buffer);
        SaveFarmData(game_buffer);
        SaveTradeData(game_buffer);
        SaveTrainingData(game_buffer);
        SaveTrainingMenuData(game_buffer);
        SaveDigitalMessangerData(game_buffer);
        SavePlayTimeData(game_buffer);
    }
    #endregion

    /// <summary>
    /// This is just a code example to copy-paste.
    /// </summary>
    private static bool LoadExampleData(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    /// <summary>
    /// This is just a code example to copy-paste.
    /// </summary>
    private static void SaveExampleData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }

    private static void SaveSaveFileInfo(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> SaveFileInfo = new Dictionary<string, object>();

        SaveFileInfo["m_name"] = StorageData.m_playerData.m_Name;
        SaveFileInfo["m_gender"] = StorageData.m_playerData.m_Gender;
        SaveFileInfo["m_level"] = StorageData.m_playerData.m_level;
        SaveFileInfo["m_partnerR"] = StorageData.m_playerData.m_partners[0].m_commonData.m_baseID;
        SaveFileInfo["m_partnerL"] = StorageData.m_playerData.m_partners[1].m_commonData.m_baseID;
        SaveFileInfo["m_mapNo"] = StorageData.m_mapData.m_mapNo;
        SaveFileInfo["m_areaNo"] = StorageData.m_mapData.m_areaNo;

        bool flag = false;
        if (StorageData.m_mapData.m_mapNo == 1 && StorageData.m_mapData.m_areaNo == 10)
        {
            TownGradeUpDataAccess gradeUpData = StorageData.m_gradeUpData;
            if (gradeUpData != null)
            {
                TownGradeUpData townGradeUpData = gradeUpData.GetTownGradeUpData(ParameterTownGradeUpData.TownGradeUpKindIndex.CenterGradeUp);
                if (townGradeUpData != null)
                {
                    flag = townGradeUpData.m_current_grade_imp > 1;
                }
            }
        }
        SaveFileInfo["TownGradeUp"] = flag;

        SaveFileInfo["m_PlayTime"] = StorageData.m_PlayTimeData.m_PlayTime;
        SaveFileInfo["m_timeStamp"] = System.DateTime.Now.ToBinary();

        game_buffer["SaveFileInfo"] = SaveFileInfo;
    }

    #region PlayerData
    private static bool LoadPlayerData(JsonNode data)
    {
        StorageData.m_playerData.m_Name = (string)data["m_playerData"]["m_Name"];
        StorageData.m_playerData.m_Gender = (int)data["m_playerData"]["m_Gender"];
        StorageData.m_playerData.m_Money = (uint)data["m_playerData"]["m_Money"];
        StorageData.m_playerData.m_DailyQuestPoint = (uint)data["m_playerData"]["m_DailyQuestPoint"];
        StorageData.m_playerData.m_Coin = (uint)data["m_playerData"]["m_Coin"];
        StorageData.m_playerData.m_exp = (int)data["m_playerData"]["m_exp"];
        StorageData.m_playerData.m_exp_walk = (int)data["m_playerData"]["m_exp_walk"];
        StorageData.m_playerData.m_exp_walk_distance = (float)data["m_playerData"]["m_exp_walk_distance"];
        StorageData.m_playerData.m_level = (int)data["m_playerData"]["m_level"];
        StorageData.m_playerData.m_skillPoint = (int)data["m_playerData"]["m_skillPoint"];
        StorageData.m_playerData.m_isUseExe = (bool)data["m_playerData"]["m_isUseExe"];
        StorageData.m_playerData.m_battleCameraType = (int)data["m_playerData"]["m_battleCameraType"];
        StorageData.m_playerData.m_extraJoglessWaitTime = (float)data["m_playerData"]["m_extraJoglessWaitTime"];
        StorageData.m_playerData.m_TownDevelopPoint = (uint)data["m_playerData"]["m_TownDevelopPoint"];
        StorageData.m_playerData.m_tent_item_id = (uint)data["m_playerData"]["m_tent_item_id"];
        StorageData.m_playerData.m_tent_hp = (int)data["m_playerData"]["m_tent_hp"];
        StorageData.m_playerData.m_tent_hp_max = (int)data["m_playerData"]["m_tent_hp_max"];
        StorageData.m_playerData.m_educationTotalCount = (int)data["m_playerData"]["m_educationTotalCount"];
        StorageData.m_playerData.m_bestHp = (int)data["m_playerData"]["m_bestHp"];
        StorageData.m_playerData.m_bestMp = (int)data["m_playerData"]["m_bestMp"];
        StorageData.m_playerData.m_bestForcefulness = (int)data["m_playerData"]["m_bestForcefulness"];
        StorageData.m_playerData.m_bestRobustness = (int)data["m_playerData"]["m_bestRobustness"];
        StorageData.m_playerData.m_bestCleverness = (int)data["m_playerData"]["m_bestCleverness"];
        StorageData.m_playerData.m_bestRapidity = (int)data["m_playerData"]["m_bestRapidity"];
        StorageData.m_playerData.m_bestAge = (int)data["m_playerData"]["m_bestAge"];
        StorageData.m_playerData.m_bestForcefulness = (int)data["m_playerData"]["m_bestForcefulness"];

        for (uint i = 0; i < StorageData.m_playerData.m_TamerSkillFlag.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_TamerSkillFlag[i] = (bool)data["m_playerData"]["m_TamerSkillFlag"][i.ToString()];
        }

        StorageData.m_playerData.m_partnersTrust = (int)data["m_playerData"]["m_partnersTrust"];
        StorageData.m_playerData.m_educationTime = (float)data["m_playerData"]["m_educationTime"];

        for (int i = 0; i < StorageData.m_playerData.m_useDlcEgs.Count; i++)
        {
            StorageData.m_playerData.m_useDlcEgs[i] = (bool)data["m_playerData"]["m_useDlcEgs"][i.ToString()];
        }

        for (uint i = 0; i < StorageData.m_playerData.m_AttacSkillkFlag.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_AttacSkillkFlag[i] = (bool)data["m_playerData"]["m_AttacSkillkFlag"][i.ToString()];
        }

        for (int i = 0; i < AppMainScript.parameterManager.m_DigimonID_LIST.Count; i++)
        {
            if (i >= StorageData.m_playerData.m_EvolutionConditionFlag.Length)
                break;

            for (uint j = 0; j < StorageDataLimits.max_condition_flag; j++)
            {
                if (data["m_playerData"]["m_EvolutionConditionFlag"][AppMainScript.parameterManager.m_DigimonID_LIST[i].ToString()] != null)
                    StorageData.m_playerData.m_EvolutionConditionFlag[i][j] = (bool)data["m_playerData"]["m_EvolutionConditionFlag"][AppMainScript.parameterManager.m_DigimonID_LIST[i].ToString()][j.ToString()];
            }
        }

        StorageData.m_playerData.m_RebornCt = (uint)data["m_playerData"]["m_RebornCt"];

        for (uint i = 0; i < StorageData.m_playerData.m_TutorialAlready.m_NumFlags; i++)
        {
            StorageData.m_playerData.m_TutorialAlready[i] = (bool)data["m_playerData"]["m_TutorialAlready"][i.ToString()];
        }

        StorageData.m_playerData.m_NumReleaseEvoFlag = (uint)data["m_playerData"]["m_NumReleaseEvoFlag"];

        // Update001
        StorageData.m_playerData.BattlePanelItemSort = (int)data["m_playerData"]["BattlePanelItemSort"];
        StorageData.m_playerData.SalePanelItemSort = (int)data["m_playerData"]["SalePanelItemSort"];

        for (int i = 0; i < StorageData.m_playerData.CarePanelItemSort.Count; i++)
        {
            StorageData.m_playerData.CarePanelItemSort[i] = (int)data["m_playerData"]["CarePanelItemSort"][i.ToString()];
        }

        for (int i = 0; i < StorageData.m_playerData.StoragePanelItemSort.Count; i++)
        {
            StorageData.m_playerData.StoragePanelItemSort[i] = (int)data["m_playerData"]["StoragePanelItemSort"][i.ToString()];
        }

        // Update003
        StorageData.m_playerData.m_IsRetryNewGame = (bool)data["m_playerData"]["m_IsRetryNewGame"];

        // Complete
        StorageData.m_playerData.Difficulty = (AppInfo.Difficulty)(int)data["m_playerData"]["Difficulty"];
        StorageData.m_playerData.m_dayExECount = (uint)data["m_playerData"]["m_dayExECount"];
        StorageData.m_playerData.m_StealthTime = (float)data["m_playerData"]["m_StealthTime"];

        return true;
    }

    private static void SavePlayerData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_playerData = new Dictionary<string, object>();
        m_playerData["m_Name"] = StorageData.m_playerData.m_Name;
        m_playerData["m_Gender"] = StorageData.m_playerData.m_Gender;
        m_playerData["m_Money"] = StorageData.m_playerData.m_Money;
        m_playerData["m_DailyQuestPoint"] = StorageData.m_playerData.m_DailyQuestPoint;
        m_playerData["m_Coin"] = StorageData.m_playerData.m_Coin;
        m_playerData["m_exp"] = StorageData.m_playerData.m_exp;
        m_playerData["m_exp_walk"] = StorageData.m_playerData.m_exp_walk;
        m_playerData["m_exp_walk_distance"] = StorageData.m_playerData.m_exp_walk_distance;
        m_playerData["m_level"] = StorageData.m_playerData.m_level;
        m_playerData["m_skillPoint"] = StorageData.m_playerData.m_skillPoint;
        m_playerData["m_isUseExe"] = StorageData.m_playerData.m_isUseExe;
        m_playerData["m_battleCameraType"] = StorageData.m_playerData.m_battleCameraType;
        m_playerData["m_extraJoglessWaitTime"] = StorageData.m_playerData.m_extraJoglessWaitTime;
        m_playerData["m_TownDevelopPoint"] = StorageData.m_playerData.m_TownDevelopPoint;
        m_playerData["m_tent_item_id"] = StorageData.m_playerData.m_tent_item_id;
        m_playerData["m_tent_hp"] = StorageData.m_playerData.m_tent_hp;
        m_playerData["m_tent_hp_max"] = StorageData.m_playerData.m_tent_hp_max;
        m_playerData["m_educationTotalCount"] = StorageData.m_playerData.m_educationTotalCount;
        m_playerData["m_bestHp"] = StorageData.m_playerData.m_bestHp;
        m_playerData["m_bestMp"] = StorageData.m_playerData.m_bestMp;
        m_playerData["m_bestForcefulness"] = StorageData.m_playerData.m_bestForcefulness;
        m_playerData["m_bestRobustness"] = StorageData.m_playerData.m_bestRobustness;
        m_playerData["m_bestCleverness"] = StorageData.m_playerData.m_bestCleverness;
        m_playerData["m_bestRapidity"] = StorageData.m_playerData.m_bestRapidity;
        m_playerData["m_bestAge"] = StorageData.m_playerData.m_bestAge;
        m_playerData["m_bestForcefulness"] = StorageData.m_playerData.m_bestForcefulness;

        Dictionary<uint, bool> m_TamerSkillFlag = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_TamerSkillFlag.m_NumFlags; i++)
        {
            m_TamerSkillFlag[i] = StorageData.m_playerData.m_TamerSkillFlag[i];
        }
        m_playerData["m_TamerSkillFlag"] = m_TamerSkillFlag;

        m_playerData["m_partnersTrust"] = StorageData.m_playerData.m_partnersTrust;
        m_playerData["m_educationTime"] = StorageData.m_playerData.m_educationTime;

        Dictionary<int, bool> m_useDlcEgs = new Dictionary<int, bool>();
        for (int i = 0; i < StorageData.m_playerData.m_useDlcEgs.Length; i++)
        {
            m_useDlcEgs[i] = StorageData.m_playerData.m_useDlcEgs[i];
        }
        m_playerData["m_useDlcEgs"] = m_useDlcEgs;

        Dictionary<uint, bool> m_AttacSkillkFlag = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_AttacSkillkFlag.m_NumFlags; i++)
        {
            m_AttacSkillkFlag[i] = StorageData.m_playerData.m_AttacSkillkFlag[i];
        }
        m_playerData["m_AttacSkillkFlag"] = m_AttacSkillkFlag;

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
        m_playerData["m_EvolutionConditionFlag"] = m_EvolutionConditionFlag;

        m_playerData["m_RebornCt"] = StorageData.m_playerData.m_RebornCt;

        Dictionary<uint, bool> m_TutorialAlready = new Dictionary<uint, bool>();
        for (uint i = 0; i < StorageData.m_playerData.m_TutorialAlready.m_NumFlags; i++)
        {
            m_TutorialAlready[i] = StorageData.m_playerData.m_TutorialAlready[i];
        }
        m_playerData["m_TutorialAlready"] = m_TutorialAlready;

        m_playerData["m_NumReleaseEvoFlag"] = StorageData.m_playerData.m_NumReleaseEvoFlag;

        // Update001
        m_playerData["BattlePanelItemSort"] = StorageData.m_playerData.BattlePanelItemSort;
        m_playerData["SalePanelItemSort"] = StorageData.m_playerData.SalePanelItemSort;

        Dictionary<int, int> CarePanelItemSort = new Dictionary<int, int>();
        for (int i = 0; i < StorageData.m_playerData.CarePanelItemSort.Length; i++)
        {
            CarePanelItemSort[i] = StorageData.m_playerData.CarePanelItemSort[i];
        }
        m_playerData["CarePanelItemSort"] = CarePanelItemSort;

        Dictionary<int, int> StoragePanelItemSort = new Dictionary<int, int>();
        for (int i = 0; i < StorageData.m_playerData.StoragePanelItemSort.Length; i++)
        {
            StoragePanelItemSort[i] = StorageData.m_playerData.StoragePanelItemSort[i];
        }
        m_playerData["StoragePanelItemSort"] = StoragePanelItemSort;

        // Update003
        m_playerData["m_IsRetryNewGame"] = StorageData.m_playerData.m_IsRetryNewGame;

        // Complete
        m_playerData["Difficulty"] = (int)StorageData.m_playerData.Difficulty;
        m_playerData["m_dayExECount"] = StorageData.m_playerData.m_dayExECount;
        m_playerData["m_StealthTime"] = StorageData.m_playerData.m_StealthTime;

        game_buffer["m_playerData"] = m_playerData;
    }
    #endregion

    #region PartnerData
    private static bool LoadPartnerData(JsonNode data)
    {
        for (int partner_no = 0; partner_no < 2; partner_no++)
        {
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_name = (string)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_name"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_baseID = (uint)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_baseID"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_weight = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_weight"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_hpMax = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_hpMax"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_hp = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_hp"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_mpMax = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_mpMax"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_mp = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_mp"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_forcefulness = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_forcefulness"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_robustness = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_robustness"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_cleverness = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_cleverness"];
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_rapidity = (int)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_rapidity"];

            ParameterDigimonData param = ParameterDigimonData.GetParam(StorageData.m_playerData.m_partners[partner_no].m_commonData.m_baseID);
            if (param != null)
                StorageData.m_playerData.m_partners[partner_no].m_commonData.m_personality = param.m_personality;

            for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_commonData.m_attack.Length; i++)
            {
                StorageData.m_playerData.m_partners[partner_no].m_commonData.m_attack[i] = (uint)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_attack"][i.ToString()];
            }

            for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas.Length; i++)
            {
                StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas[i].m_kind_index = (int)data["m_partners"][partner_no.ToString()]["m_educationDatas"][i.ToString()]["m_kind_index"];
                StorageData.m_playerData.m_partners[partner_no].m_educationDatas.m_educationDatas[i].m_count = (int)data["m_partners"][partner_no.ToString()]["m_educationDatas"][i.ToString()]["m_count"];
            }

            for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas.Length; i++)
            {
                StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_mapNo = (int)data["m_partners"][partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_mapNo"];
                StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_areaNo = (int)data["m_partners"][partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_areaNo"];
                float x = (float)data["m_partners"][partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_position.x"];
                float y = (float)data["m_partners"][partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_position.y"];
                float z = (float)data["m_partners"][partner_no.ToString()]["m_nokusoData"][i.ToString()]["m_position.z"];
                StorageData.m_playerData.m_partners[partner_no].m_nokusoData.m_nokusoDatas[i].m_position = new Vector3(x, y, z);
            }

            // Lifecycle
            StorageData.m_playerData.m_partners[partner_no].m_bonds = (int)data["m_partners"][partner_no.ToString()]["m_bonds"];
            StorageData.m_playerData.m_partners[partner_no].m_age = (int)data["m_partners"][partner_no.ToString()]["m_age"];
            StorageData.m_playerData.m_partners[partner_no].m_time_from_age = (float)data["m_partners"][partner_no.ToString()]["m_time_from_age"];
            StorageData.m_playerData.m_partners[partner_no].m_time_from_birth = (float)data["m_partners"][partner_no.ToString()]["m_time_from_birth"];
            StorageData.m_playerData.m_partners[partner_no].m_curse = (int)data["m_partners"][partner_no.ToString()]["m_curse"];
            StorageData.m_playerData.m_partners[partner_no].m_lifetime = (float)data["m_partners"][partner_no.ToString()]["m_lifetime"];
            StorageData.m_playerData.m_partners[partner_no].m_breeding = (int)data["m_partners"][partner_no.ToString()]["m_breeding"];
            StorageData.m_playerData.m_partners[partner_no].m_mood = (int)data["m_partners"][partner_no.ToString()]["m_mood"];
            StorageData.m_playerData.m_partners[partner_no].m_satiety = (int)data["m_partners"][partner_no.ToString()]["m_satiety"];
            StorageData.m_playerData.m_partners[partner_no].m_fatigue = (int)data["m_partners"][partner_no.ToString()]["m_fatigue"];
            StorageData.m_playerData.m_partners[partner_no].m_FieldStatusEffect = (int)data["m_partners"][partner_no.ToString()]["m_FieldStatusEffect"];
            StorageData.m_playerData.m_partners[partner_no].m_trainingFailure = (int)data["m_partners"][partner_no.ToString()]["m_trainingFailure"];
            StorageData.m_playerData.m_partners[partner_no].m_battleWin = (int)data["m_partners"][partner_no.ToString()]["m_battleWin"];
            StorageData.m_playerData.m_partners[partner_no].m_mealTime = (float)data["m_partners"][partner_no.ToString()]["m_mealTime"];
            StorageData.m_playerData.m_partners[partner_no].m_toiletTime = (float)data["m_partners"][partner_no.ToString()]["m_toiletTime"];
            StorageData.m_playerData.m_partners[partner_no].m_sleepTime = (float)data["m_partners"][partner_no.ToString()]["m_sleepTime"];
            StorageData.m_playerData.m_partners[partner_no].m_isReqMeal = (bool)data["m_partners"][partner_no.ToString()]["m_isReqMeal"];
            StorageData.m_playerData.m_partners[partner_no].m_isReqToilet = (bool)data["m_partners"][partner_no.ToString()]["m_isReqToilet"];
            StorageData.m_playerData.m_partners[partner_no].m_isReqSleep = (bool)data["m_partners"][partner_no.ToString()]["m_isReqSleep"];
            StorageData.m_playerData.m_partners[partner_no].m_lastBaitTime = (float)data["m_partners"][partner_no.ToString()]["m_lastBaitTime"];
            StorageData.m_playerData.m_partners[partner_no].m_subSatietyTime = (float)data["m_partners"][partner_no.ToString()]["m_subSatietyTime"];
            StorageData.m_playerData.m_partners[partner_no].m_subSatietyCt = (int)data["m_partners"][partner_no.ToString()]["m_subSatietyCt"];
            StorageData.m_playerData.m_partners[partner_no].m_mealTimeZone = (PlayerData.PartnerData.MealTimeZone)(int)data["m_partners"][partner_no.ToString()]["m_mealTimeZone"];
            StorageData.m_playerData.m_partners[partner_no].m_putToSleepTime = (float)data["m_partners"][partner_no.ToString()]["m_putToSleepTime"];
            StorageData.m_playerData.m_partners[partner_no].m_wakeUpTime = (float)data["m_partners"][partner_no.ToString()]["m_wakeUpTime"];
            StorageData.m_playerData.m_partners[partner_no].m_GenerationNum = (uint)data["m_partners"][partner_no.ToString()]["m_GenerationNum"];
            StorageData.m_playerData.m_partners[partner_no].m_favoriteAddingId = (uint)data["m_partners"][partner_no.ToString()]["m_favoriteAddingId"];
            StorageData.m_playerData.m_partners[partner_no].m_TerrainImpactTimer = (float)data["m_partners"][partner_no.ToString()]["m_TerrainImpactTimer"];

            // Diathesis
            StorageData.m_playerData.m_partners[partner_no].m_diathesisHp = (int)data["m_partners"][partner_no.ToString()]["m_diathesisHp"];
            StorageData.m_playerData.m_partners[partner_no].m_diathesisMp = (int)data["m_partners"][partner_no.ToString()]["m_diathesisMp"];
            StorageData.m_playerData.m_partners[partner_no].m_diathesisForcefulness = (int)data["m_partners"][partner_no.ToString()]["m_diathesisForcefulness"];
            StorageData.m_playerData.m_partners[partner_no].m_diathesisRobustness = (int)data["m_partners"][partner_no.ToString()]["m_diathesisRobustness"];
            StorageData.m_playerData.m_partners[partner_no].m_diathesisCleverness = (int)data["m_partners"][partner_no.ToString()]["m_diathesisCleverness"];
            StorageData.m_playerData.m_partners[partner_no].m_diathesisRapidity = (int)data["m_partners"][partner_no.ToString()]["m_diathesisRapidity"];

            // Chip
            StorageData.m_playerData.m_partners[partner_no].m_chipHpMax = (int)data["m_partners"][partner_no.ToString()]["m_chipHpMax"];
            StorageData.m_playerData.m_partners[partner_no].m_chipMpMax = (int)data["m_partners"][partner_no.ToString()]["m_chipMpMax"];
            StorageData.m_playerData.m_partners[partner_no].m_chipForcefulness = (int)data["m_partners"][partner_no.ToString()]["m_chipForcefulness"];
            StorageData.m_playerData.m_partners[partner_no].m_chipRobustness = (int)data["m_partners"][partner_no.ToString()]["m_chipRobustness"];
            StorageData.m_playerData.m_partners[partner_no].m_chipCleverness = (int)data["m_partners"][partner_no.ToString()]["m_chipCleverness"];
            StorageData.m_playerData.m_partners[partner_no].m_chipRapidity = (int)data["m_partners"][partner_no.ToString()]["m_chipRapidity"];

            // EvolutionBefore
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeBaseId = (uint)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeBaseId"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeHp = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeHp"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeMp = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeMp"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeForcefulness = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeForcefulness"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRobustness = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeRobustness"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeCleverness = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeCleverness"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRapidity = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforeRapidity"];
            StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforemWeight = (int)data["m_partners"][partner_no.ToString()]["m_EvolutionBeforemWeight"];

            // MealCorrection
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHp = (int)data["m_partners"][partner_no.ToString()]["m_mealCorrectionHp"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMp = (int)data["m_partners"][partner_no.ToString()]["m_mealCorrectionMp"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulness = (int)data["m_partners"][partner_no.ToString()]["m_mealCorrectionForcefulness"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustness = (int)data["m_partners"][partner_no.ToString()]["m_mealCorrectionRobustness"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionVleverness = (int)data["m_partners"][partner_no.ToString()]["m_mealCorrectionVleverness"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidity = (int)data["m_partners"][partner_no.ToString()]["m_mealCorrectionRapidity"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHpEffectTime = (float)data["m_partners"][partner_no.ToString()]["m_mealCorrectionHpEffectTime"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMpEffectTime = (float)data["m_partners"][partner_no.ToString()]["m_mealCorrectionMpEffectTime"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulnessEffectTime = (float)data["m_partners"][partner_no.ToString()]["m_mealCorrectionForcefulnessEffectTime"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustnessEffectTime = (float)data["m_partners"][partner_no.ToString()]["m_mealCorrectionRobustnessEffectTime"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionClevernessEffectTime = (float)data["m_partners"][partner_no.ToString()]["m_mealCorrectionClevernessEffectTime"];
            StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidityEffectTime = (float)data["m_partners"][partner_no.ToString()]["m_mealCorrectionRapidityEffectTime"];

            // Battle
            StorageData.m_playerData.m_partners[partner_no].m_BattleRange = (PlayerData.PartnerData.BattleRangeType)(int)data["m_partners"][partner_no.ToString()]["m_BattleRange"];
            StorageData.m_playerData.m_partners[partner_no].m_BattlePolicy = (PlayerData.PartnerData.BattlePolicyType)(int)data["m_partners"][partner_no.ToString()]["m_BattlePolicy"];
            StorageData.m_playerData.m_partners[partner_no].m_BattleUseMpPolicy = (PlayerData.PartnerData.BattleUseMpType)(int)data["m_partners"][partner_no.ToString()]["m_BattleUseMpPolicy"];

            // Genealogy
            for (int i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_HistoryArray.Length; i++)
            {
                int year = (int)data["m_partners"][partner_no.ToString()]["m_HistoryArray"][i.ToString()]["m_Year"];
                int day = (int)data["m_partners"][partner_no.ToString()]["m_HistoryArray"][i.ToString()]["m_Day"];
                uint id = (uint)data["m_partners"][partner_no.ToString()]["m_HistoryArray"][i.ToString()]["m_DigimonID"];
                StorageData.m_playerData.m_partners[partner_no].m_HistoryArray[i] = new PlayerData.PartnerData.HistoryData(year, day, id);
            }

            for (uint i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock.m_NumFlags; i++)
            {
                StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock[i] = (bool)data["m_partners"][partner_no.ToString()]["m_EvolutionBlock"][i.ToString()];
            }

            // Update001
            StorageData.m_playerData.m_partners[partner_no].m_prevLifeCycleUpdateTime = (float)data["m_partners"][partner_no.ToString()]["m_prevLifeCycleUpdateTime"];
            StorageData.m_playerData.m_partners[partner_no].m_LifeCycleMessageFlag = (int)data["m_partners"][partner_no.ToString()]["m_LifeCycleMessageFlag"];

            // Complete
            StorageData.m_playerData.m_partners[partner_no].m_commonData.m_IsDefaultName = (bool)data["m_partners"][partner_no.ToString()]["m_commonData"]["m_IsDefaultName"];
        }

        return true;
    }

    private static void SavePartnerData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_partners = new Dictionary<string, object>();

        for (int partner_no = 0; partner_no < 2; partner_no++)
        {
            Dictionary<string, object> partner = new Dictionary<string, object>();

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
            partner["m_educationDatas"] = m_educationDatas;

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
            partner["m_nokusoData"] = m_nokusoData;

            // Lifecycle
            partner["m_bonds"] = StorageData.m_playerData.m_partners[partner_no].m_bonds;
            partner["m_age"] = StorageData.m_playerData.m_partners[partner_no].m_age;
            partner["m_time_from_age"] = StorageData.m_playerData.m_partners[partner_no].m_time_from_age;
            partner["m_time_from_birth"] = StorageData.m_playerData.m_partners[partner_no].m_time_from_birth;
            partner["m_curse"] = StorageData.m_playerData.m_partners[partner_no].m_curse;
            partner["m_lifetime"] = StorageData.m_playerData.m_partners[partner_no].m_lifetime;
            partner["m_breeding"] = StorageData.m_playerData.m_partners[partner_no].m_breeding;
            partner["m_mood"] = StorageData.m_playerData.m_partners[partner_no].m_mood;
            partner["m_satiety"] = StorageData.m_playerData.m_partners[partner_no].m_satiety;
            partner["m_fatigue"] = StorageData.m_playerData.m_partners[partner_no].m_fatigue;
            partner["m_FieldStatusEffect"] = StorageData.m_playerData.m_partners[partner_no].m_FieldStatusEffect;
            partner["m_trainingFailure"] = StorageData.m_playerData.m_partners[partner_no].m_trainingFailure;
            partner["m_battleWin"] = StorageData.m_playerData.m_partners[partner_no].m_battleWin;
            partner["m_mealTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealTime;
            partner["m_toiletTime"] = StorageData.m_playerData.m_partners[partner_no].m_toiletTime;
            partner["m_sleepTime"] = StorageData.m_playerData.m_partners[partner_no].m_sleepTime;
            partner["m_isReqMeal"] = StorageData.m_playerData.m_partners[partner_no].m_isReqMeal;
            partner["m_isReqToilet"] = StorageData.m_playerData.m_partners[partner_no].m_isReqToilet;
            partner["m_isReqSleep"] = StorageData.m_playerData.m_partners[partner_no].m_isReqSleep;
            partner["m_lastBaitTime"] = StorageData.m_playerData.m_partners[partner_no].m_lastBaitTime;
            partner["m_subSatietyTime"] = StorageData.m_playerData.m_partners[partner_no].m_subSatietyTime;
            partner["m_subSatietyCt"] = StorageData.m_playerData.m_partners[partner_no].m_subSatietyCt;
            partner["m_mealTimeZone"] = (int)StorageData.m_playerData.m_partners[partner_no].m_mealTimeZone;
            partner["m_putToSleepTime"] = StorageData.m_playerData.m_partners[partner_no].m_putToSleepTime;
            partner["m_wakeUpTime"] = StorageData.m_playerData.m_partners[partner_no].m_wakeUpTime;
            partner["m_GenerationNum"] = StorageData.m_playerData.m_partners[partner_no].m_GenerationNum;
            partner["m_favoriteAddingId"] = StorageData.m_playerData.m_partners[partner_no].m_favoriteAddingId;
            partner["m_TerrainImpactTimer"] = StorageData.m_playerData.m_partners[partner_no].m_TerrainImpactTimer;

            // Diathesis
            partner["m_diathesisHp"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisHp;
            partner["m_diathesisMp"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisMp;
            partner["m_diathesisForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisForcefulness;
            partner["m_diathesisRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisRobustness;
            partner["m_diathesisCleverness"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisCleverness;
            partner["m_diathesisRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_diathesisRapidity;

            // Chip
            partner["m_chipHpMax"] = StorageData.m_playerData.m_partners[partner_no].m_chipHpMax;
            partner["m_chipMpMax"] = StorageData.m_playerData.m_partners[partner_no].m_chipMpMax;
            partner["m_chipForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_chipForcefulness;
            partner["m_chipRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_chipRobustness;
            partner["m_chipCleverness"] = StorageData.m_playerData.m_partners[partner_no].m_chipCleverness;
            partner["m_chipRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_chipRapidity;

            // EvolutionBefore
            partner["m_EvolutionBeforeBaseId"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeBaseId;
            partner["m_EvolutionBeforeHp"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeHp;
            partner["m_EvolutionBeforeMp"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeMp;
            partner["m_EvolutionBeforeForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeForcefulness;
            partner["m_EvolutionBeforeRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRobustness;
            partner["m_EvolutionBeforeCleverness"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeCleverness;
            partner["m_EvolutionBeforeRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforeRapidity;
            partner["m_EvolutionBeforemWeight"] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBeforemWeight;

            // MealCorrection
            partner["m_mealCorrectionHp"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHp;
            partner["m_mealCorrectionMp"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMp;
            partner["m_mealCorrectionForcefulness"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulness;
            partner["m_mealCorrectionRobustness"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustness;
            partner["m_mealCorrectionVleverness"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionVleverness;
            partner["m_mealCorrectionRapidity"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidity;
            partner["m_mealCorrectionHpEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionHpEffectTime;
            partner["m_mealCorrectionMpEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionMpEffectTime;
            partner["m_mealCorrectionForcefulnessEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionForcefulnessEffectTime;
            partner["m_mealCorrectionRobustnessEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRobustnessEffectTime;
            partner["m_mealCorrectionClevernessEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionClevernessEffectTime;
            partner["m_mealCorrectionRapidityEffectTime"] = StorageData.m_playerData.m_partners[partner_no].m_mealCorrectionRapidityEffectTime;

            // Battle
            partner["m_BattleRange"] = (int)StorageData.m_playerData.m_partners[partner_no].m_BattleRange;
            partner["m_BattlePolicy"] = (int)StorageData.m_playerData.m_partners[partner_no].m_BattlePolicy;
            partner["m_BattleUseMpPolicy"] = (int)StorageData.m_playerData.m_partners[partner_no].m_BattleUseMpPolicy;

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
            partner["m_HistoryArray"] = m_HistoryArray;

            Dictionary<uint, bool> m_EvolutionBlock = new Dictionary<uint, bool>();
            for (uint i = 0; i < StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock.m_NumFlags; i++)
            {
                m_EvolutionBlock[i] = StorageData.m_playerData.m_partners[partner_no].m_EvolutionBlock[i];
            }
            partner["m_EvolutionBlock"] = m_EvolutionBlock;

            // Update001
            partner["m_prevLifeCycleUpdateTime"] = StorageData.m_playerData.m_partners[partner_no].m_prevLifeCycleUpdateTime;
            partner["m_LifeCycleMessageFlag"] = StorageData.m_playerData.m_partners[partner_no].m_LifeCycleMessageFlag;

            // Complete
            m_commonData["m_IsDefaultName"] = StorageData.m_playerData.m_partners[partner_no].m_commonData.m_IsDefaultName;
            partner["m_commonData"] = m_commonData;

            m_partners[partner_no.ToString()] = partner;
        }

        game_buffer["m_partners"] = m_partners;
    }
    #endregion

    #region WorldData
    private static bool LoadWorldData(JsonNode data)
    {
        StorageData.m_worldData.m_time = (float)data["m_worldData"]["m_time"];
        StorageData.m_worldData.m_weak = (int)data["m_worldData"]["m_weak"];
        StorageData.m_worldData.m_season = (int)data["m_worldData"]["m_season"];
        StorageData.m_worldData.m_year = (int)data["m_worldData"]["m_year"];
        StorageData.m_worldData.m_daySpent = (int)data["m_worldData"]["m_daySpent"];
        StorageData.m_worldData.m_totalDay = (uint)data["m_worldData"]["m_totalDay"];

        return true;
    }

    private static void SaveWorldData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_worldData = new Dictionary<string, object>();

        m_worldData["m_time"] = StorageData.m_worldData.m_time;
        m_worldData["m_weak"] = StorageData.m_worldData.m_weak;
        m_worldData["m_season"] = StorageData.m_worldData.m_season;
        m_worldData["m_year"] = StorageData.m_worldData.m_year;
        m_worldData["m_daySpent"] = StorageData.m_worldData.m_daySpent;
        m_worldData["m_totalDay"] = StorageData.m_worldData.m_totalDay;

        game_buffer["m_worldData"] = m_worldData;
    }
    #endregion

    #region MapData
    private static bool LoadMapData(JsonNode data)
    {
        StorageData.m_mapData.m_mapNo = (int)data["m_mapData"]["m_mapNo"];
        StorageData.m_mapData.m_areaNo = (int)data["m_mapData"]["m_areaNo"];
        StorageData.m_mapData.m_mapEnterTime = (float)data["m_mapData"]["m_mapEnterTime"];

        for (int i = 0; (long)i < 64L; i++)
        {
            StorageData.m_mapData.m_spawnPointBuffer[i] = (byte)data["m_mapData"]["m_spawnPointBuffer"][i.ToString()];
        }
        StorageData.m_mapData.m_spawnPoint = Encoding.UTF8.GetString(StorageData.m_mapData.m_spawnPointBuffer);

        StorageData.m_mapData.m_player.m_pos_x = (float)data["m_mapData"]["m_player"]["m_pos_x"];
        StorageData.m_mapData.m_player.m_pos_y = (float)data["m_mapData"]["m_player"]["m_pos_y"];
        StorageData.m_mapData.m_player.m_pos_z = (float)data["m_mapData"]["m_player"]["m_pos_z"];
        StorageData.m_mapData.m_player.m_eulerAngleY = (float)data["m_mapData"]["m_player"]["m_eulerAngleY"];

        for (int i = 0; i < 2; i++)
        {
            StorageData.m_mapData.m_pertners[i].m_pos_x = (float)data["m_mapData"]["m_pertners"][i.ToString()]["m_pos_x"];
            StorageData.m_mapData.m_pertners[i].m_pos_y = (float)data["m_mapData"]["m_pertners"][i.ToString()]["m_pos_y"];
            StorageData.m_mapData.m_pertners[i].m_pos_z = (float)data["m_mapData"]["m_pertners"][i.ToString()]["m_pos_z"];
        }

        for (int i = 0; i < 20; i++)
        {
            StorageData.m_mapData.m_enemies[i].m_pos_x = (float)data["m_mapData"]["m_enemies"][i.ToString()]["m_pos_x"];
            StorageData.m_mapData.m_enemies[i].m_pos_y = (float)data["m_mapData"]["m_enemies"][i.ToString()]["m_pos_y"];
            StorageData.m_mapData.m_enemies[i].m_pos_z = (float)data["m_mapData"]["m_enemies"][i.ToString()]["m_pos_z"];
            StorageData.m_mapData.m_enemies[i].m_hp = (int)data["m_mapData"]["m_enemies"][i.ToString()]["m_hp"];
            StorageData.m_mapData.m_enemies[i].m_isCreate = (bool)data["m_mapData"]["m_enemies"][i.ToString()]["m_isCreate"];
        }

        //for (int i = 0; i < 16; i++)
        //{
        //    StorageData.m_mapData.m_npcs[i].m_pos_x = (float)data["m_mapData"]["m_npcs"][i.ToString()]["m_pos_x"];
        //    StorageData.m_mapData.m_npcs[i].m_pos_y = (float)data["m_mapData"]["m_npcs"][i.ToString()]["m_pos_y"];
        //    StorageData.m_mapData.m_npcs[i].m_pos_z = (float)data["m_mapData"]["m_npcs"][i.ToString()]["m_pos_z"];
        //    StorageData.m_mapData.m_npcs[i].m_eulerAngleY = (float)data["m_mapData"]["m_npcs"][i.ToString()]["m_eulerAngleY"];
        //}

        StorageData.m_mapData.m_isUseSaveData = true;

        return true;
    }

    private static void SaveMapData(Dictionary<string, object> game_buffer)
    {
        StorageData.m_mapData.ConvertSaveData();
        Dictionary<string, object> m_mapData = new Dictionary<string, object>();

        m_mapData["m_mapNo"] = StorageData.m_mapData.m_mapNo;
        m_mapData["m_areaNo"] = StorageData.m_mapData.m_areaNo;
        m_mapData["m_mapEnterTime"] = StorageData.m_mapData.m_mapEnterTime;

        if (StorageData.m_mapData.m_spawnPoint == null)
        {
            StorageData.m_mapData.m_spawnPoint = string.Empty;
        }
        byte[] bytes = Encoding.UTF8.GetBytes(StorageData.m_mapData.m_spawnPoint);
        for (int i = 0; i < bytes.Length; i++)
        {
            StorageData.m_mapData.m_spawnPointBuffer[i] = bytes[i];
        }
        Dictionary<int, object> m_spawnPointBuffer = new Dictionary<int, object>();
        for (int j = 0; (long)j < 64L; j++)
        {
            m_spawnPointBuffer[j] = StorageData.m_mapData.m_spawnPointBuffer[j];
        }
        m_mapData["m_spawnPointBuffer"] = m_spawnPointBuffer;

        Dictionary<string, object> m_player = new Dictionary<string, object>();

        m_player["m_pos_x"] = StorageData.m_mapData.m_player.m_pos_x;
        m_player["m_pos_y"] = StorageData.m_mapData.m_player.m_pos_y;
        m_player["m_pos_z"] = StorageData.m_mapData.m_player.m_pos_z;
        m_player["m_eulerAngleY"] = StorageData.m_mapData.m_player.m_eulerAngleY;

        m_mapData["m_player"] = m_player;

        Dictionary<string, object> m_pertners = new Dictionary<string, object>();

        for (int k = 0; k < StorageData.m_mapData.m_pertners.Length; k++)
        {
            Dictionary<string, object> partner = new Dictionary<string, object>();

            partner["m_pos_x"] = StorageData.m_mapData.m_pertners[k].m_pos_x;
            partner["m_pos_y"] = StorageData.m_mapData.m_pertners[k].m_pos_y;
            partner["m_pos_z"] = StorageData.m_mapData.m_pertners[k].m_pos_z;

            m_pertners[k.ToString()] = partner;
        }

        m_mapData["m_pertners"] = m_pertners;

        Dictionary<string, object> m_enemies = new Dictionary<string, object>();

        for (int l = 0; l < StorageData.m_mapData.m_enemies.Length; l++)
        {
            Dictionary<string, object> enemy = new Dictionary<string, object>();

            enemy["m_pos_x"] = StorageData.m_mapData.m_enemies[l].m_pos_x;
            enemy["m_pos_y"] = StorageData.m_mapData.m_enemies[l].m_pos_y;
            enemy["m_pos_z"] = StorageData.m_mapData.m_enemies[l].m_pos_z;
            enemy["m_hp"] = StorageData.m_mapData.m_enemies[l].m_hp;
            enemy["m_isCreate"] = StorageData.m_mapData.m_enemies[l].m_isCreate;

            m_enemies[l.ToString()] = enemy;
        }

        m_mapData["m_enemies"] = m_enemies;

        // the m_npcs doesn't seems to be used, all value were the default one, even on map with many npc such as town.

        //Dictionary<string, object> m_npcs = new Dictionary<string, object>();

        //for (int m = 0; m < StorageData.m_mapData.m_npcs.Length; m++)
        //{
        //    Dictionary<string, object> npc = new Dictionary<string, object>();

        //    npc["m_pos_x"] = StorageData.m_mapData.m_npcs[m].m_pos_x;
        //    npc["m_pos_y"] = StorageData.m_mapData.m_npcs[m].m_pos_y;
        //    npc["m_pos_z"] = StorageData.m_mapData.m_npcs[m].m_pos_z;
        //    npc["m_eulerAngleY"] = StorageData.m_mapData.m_npcs[m].m_eulerAngleY;

        //    m_npcs[m.ToString()] = npc;
        //}

        //m_mapData["m_npcs"] = m_npcs;

        game_buffer["m_mapData"] = m_mapData;
    }
    #endregion

    #region ItemPickPointData
    private static bool LoadItemPickPointData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_itemPickPointData.m_itemPickPointDatas.Length; i++)
        {
            StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_id = (uint)data["m_itemPickPointData"]["m_itemPickPointDatas"][i.ToString()]["m_id"];
            for (int j = 0; j < StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_itemDatas.Length; j++)
            {
                StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_itemDatas[j].m_id = (uint)data["m_itemPickPointData"]["m_itemPickPointDatas"][i.ToString()]["m_itemDatas"][j.ToString()]["m_id"];
                StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_itemDatas[j].m_num = (int)data["m_itemPickPointData"]["m_itemPickPointDatas"][i.ToString()]["m_itemDatas"][j.ToString()]["m_num"];
                StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_itemDatas[j].m_pos_x = (float)data["m_itemPickPointData"]["m_itemPickPointDatas"][i.ToString()]["m_itemDatas"][j.ToString()]["m_pos_x"];
                StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_itemDatas[j].m_pos_y = (float)data["m_itemPickPointData"]["m_itemPickPointDatas"][i.ToString()]["m_itemDatas"][j.ToString()]["m_pos_y"];
                StorageData.m_itemPickPointData.m_itemPickPointDatas[i].m_itemDatas[j].m_pos_z = (float)data["m_itemPickPointData"]["m_itemPickPointDatas"][i.ToString()]["m_itemDatas"][j.ToString()]["m_pos_z"];
            }
        }

        for (int i = 0; i < StorageData.m_itemPickPointData.m_materialPickPointDatas.Length; i++)
        {
            StorageData.m_itemPickPointData.m_materialPickPointDatas[i].m_id = (uint)data["m_itemPickPointData"]["m_materialPickPointDatas"][i.ToString()]["m_id"];
            StorageData.m_itemPickPointData.m_materialPickPointDatas[i].m_remainderPickCount = (int)data["m_itemPickPointData"]["m_materialPickPointDatas"][i.ToString()]["m_remainderPickCount"];
            StorageData.m_itemPickPointData.m_materialPickPointDatas[i].m_isTodayUpdate = (bool)data["m_itemPickPointData"]["m_materialPickPointDatas"][i.ToString()]["m_isTodayUpdate"];
        }

        StorageData.m_itemPickPointData.m_isUseSaveData = true;

        return true;
    }

    private static void SaveItemPickPointData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_itemPickPointData = new Dictionary<string, object>();

        Dictionary<string, object> m_itemPickPointDatas = new Dictionary<string, object>();

        for (int i = 0; i < StorageData.m_itemPickPointData.m_itemPickPointDatas.Length; i++)
        {
            Dictionary<string, object> m_itemDatas = new Dictionary<string, object>();

            ItemPickPointData data = StorageData.m_itemPickPointData.m_itemPickPointDatas[i];
            for (int j = 0; j < data.m_itemDatas.Length; j++)
            {
                Dictionary<string, object> Datas = new Dictionary<string, object>();
                Datas["m_id"] = data.m_itemDatas[j].m_id;
                Datas["m_num"] = data.m_itemDatas[j].m_num;
                Datas["m_pos_x"] = data.m_itemDatas[j].m_pos_x;
                Datas["m_pos_y"] = data.m_itemDatas[j].m_pos_y;
                Datas["m_pos_z"] = data.m_itemDatas[j].m_pos_z;
                m_itemDatas[j.ToString()] = Datas;
            }

            Dictionary<string, object> pickPointData = new Dictionary<string, object>();
            pickPointData["m_id"] = data.m_id;
            pickPointData["m_itemDatas"] = m_itemDatas;
            m_itemPickPointDatas[i.ToString()] = pickPointData;
        }

        m_itemPickPointData["m_itemPickPointDatas"] = m_itemPickPointDatas;

        Dictionary<string, object> m_materialPickPointDatas = new Dictionary<string, object>();

        for (int i = 0; i < StorageData.m_itemPickPointData.m_materialPickPointDatas.Length; i++)
        {
            Dictionary<string, object> Datas = new Dictionary<string, object>();
            MaterialPickPointData data = StorageData.m_itemPickPointData.m_materialPickPointDatas[i];
            Datas["m_id"] = data.m_id;
            Datas["m_remainderPickCount"] = data.m_remainderPickCount;
            Datas["m_isTodayUpdate"] = data.m_isTodayUpdate;
            m_materialPickPointDatas[i.ToString()] = Datas;
        }

        m_itemPickPointData["m_materialPickPointDatas"] = m_materialPickPointDatas;

        game_buffer["m_itemPickPointData"] = m_itemPickPointData;
    }
    #endregion

    #region ScenarioProgressData
    private static bool LoadScenarioProgressData(JsonNode data)
    {
        for (uint i = 0; i < StorageData.m_ScenarioProgressData.m_ScenarioFlag.GetNumWork(); i++)
        {
            StorageData.m_ScenarioProgressData.m_ScenarioFlag.SetBitFlagWark(i, (ulong)data["m_ScenarioProgressData"]["m_ScenarioFlag"][i.ToString()]);
        }

        for (int i = 0; i < StorageData.m_ScenarioProgressData.m_ScenarioCounter.Length; i++)
        {
            StorageData.m_ScenarioProgressData.m_ScenarioCounter[i] = (int)data["m_ScenarioProgressData"]["m_ScenarioCounter"][i.ToString()];
        }

        for (int i = 0; i < StorageData.m_ScenarioProgressData.m_ScenarioFloatValue.Length; i++)
        {
            StorageData.m_ScenarioProgressData.m_ScenarioFloatValue[i] = (int)data["m_ScenarioProgressData"]["m_ScenarioFloatValue"][i.ToString()];
        }

        StorageData.m_ScenarioProgressData.m_Chapter = (CScenarioProgressData.Chapter)(uint)data["m_ScenarioProgressData"]["m_ChapterNo"];

        return true;
    }

    private static void SaveScenarioProgressData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_ScenarioProgressData = new Dictionary<string, object>();

        Dictionary<uint, object> m_ScenarioFlag = new Dictionary<uint, object>();
        for (uint i = 0; i < StorageData.m_ScenarioProgressData.m_ScenarioFlag.GetNumWork(); i++)
        {
            m_ScenarioFlag[i] = StorageData.m_ScenarioProgressData.m_ScenarioFlag.GetBitFlagWark(i);
        }
        m_ScenarioProgressData["m_ScenarioFlag"] = m_ScenarioFlag;

        Dictionary<int, object> m_ScenarioCounter = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_ScenarioProgressData.m_ScenarioCounter.Length; i++)
        {
            m_ScenarioCounter[i] = StorageData.m_ScenarioProgressData.m_ScenarioCounter[i];
        }
        m_ScenarioProgressData["m_ScenarioCounter"] = m_ScenarioCounter;

        Dictionary<int, object> m_ScenarioFloatValue = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_ScenarioProgressData.m_ScenarioFloatValue.Length; i++)
        {
            m_ScenarioFloatValue[i] = StorageData.m_ScenarioProgressData.m_ScenarioFloatValue[i];
        }
        m_ScenarioProgressData["m_ScenarioFloatValue"] = m_ScenarioFloatValue;

        m_ScenarioProgressData["m_ChapterNo"] = (uint)StorageData.m_ScenarioProgressData.m_ChapterNo;

        game_buffer["m_ScenarioProgressData"] = m_ScenarioProgressData;
    }
    #endregion

    #region ScenaioFlagSetTimer
    private static bool LoadScenaioFlagSetTimer(JsonNode data)
    {
        StorageData.m_ScenaioFlagSetTimer.ClearTimer();

        for (int i = 0; i < StorageData.m_ScenaioFlagSetTimer.m_numTimerMax; i++)
        {
            CTimerData cTimerData = new CTimerData();
            cTimerData.m_TimeLeft = (float)data["m_ScenaioFlagSetTimer"][i.ToString()]["m_TimeLeft"];
            cTimerData.m_FlagSetId = (uint)data["m_ScenaioFlagSetTimer"][i.ToString()]["m_FlagSetId"];
            cTimerData.m_Operation = (bool)data["m_ScenaioFlagSetTimer"][i.ToString()]["m_Operation"];
            cTimerData.m_InterVal = (float)data["m_ScenaioFlagSetTimer"][i.ToString()]["m_InterVal"];
            StorageData.m_ScenaioFlagSetTimer.AddTimer(cTimerData);
        }

        return true;
    }

    private static void SaveScenaioFlagSetTimer(Dictionary<string, object> game_buffer)
    {
        Dictionary<uint, object> m_ScenaioFlagSetTimer = new Dictionary<uint, object>();

        uint num = 0u;
        foreach (Il2CppSystem.Collections.Generic.KeyValuePair<uint, CTimerData> activeTimer in StorageData.m_ScenaioFlagSetTimer.m_ActiveTimers)
        {
            CTimerData value = activeTimer.Value;
            Dictionary<string, object> timer = new Dictionary<string, object>();
            timer["m_TimeLeft"] = value.m_TimeLeft;
            timer["m_FlagSetId"] = value.m_FlagSetId;
            timer["m_Operation"] = value.m_Operation;
            timer["m_InterVal"] = value.m_InterVal;
            m_ScenaioFlagSetTimer[num] = timer;
            num++;
        }

        uint num2 = StorageData.m_ScenaioFlagSetTimer.m_numTimerMax - num;
        CTimerData cTimerData = new CTimerData();
        for (uint i = 0; i < num2; i++)
        {
            Dictionary<string, object> timerData = new Dictionary<string, object>();
            timerData["m_TimeLeft"] = cTimerData.m_TimeLeft;
            timerData["m_FlagSetId"] = cTimerData.m_FlagSetId;
            timerData["m_Operation"] = cTimerData.m_Operation;
            timerData["m_InterVal"] = cTimerData.m_InterVal;
            m_ScenaioFlagSetTimer[num + i] = timerData;
        }

        game_buffer["m_ScenaioFlagSetTimer"] = m_ScenaioFlagSetTimer;
    }
    #endregion

    #region ItemStorageData
    private static bool LoadItemStorageData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_ItemStorageData.m_itemDataListTbl.Length; i++)
        {
            if (StorageData.m_ItemStorageData.m_itemDataListTbl[i] == null)
            {
                continue;
            }

            for (int j = 0; j < StorageData.m_ItemStorageData.m_itemDataListTbl[i].Count; j++)
            {
                if (StorageData.m_ItemStorageData.m_itemDataListTbl[i][j] != null)
                {
                    if (data["m_ItemStorageData"]["m_itemDataListTbl"][i.ToString()][j.ToString()] != null)
                    {
                        StorageData.m_ItemStorageData.m_itemDataListTbl[i][j].m_itemID = (uint)data["m_ItemStorageData"]["m_itemDataListTbl"][i.ToString()][j.ToString()]["m_itemID"];
                        StorageData.m_ItemStorageData.m_itemDataListTbl[i][j].m_itemNum = (int)data["m_ItemStorageData"]["m_itemDataListTbl"][i.ToString()][j.ToString()]["m_itemNum"];
                    }
                }
            }
        }

        List<int> keys = new List<int>();
        foreach (int key in StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList.Keys)
            keys.Add(key);

        for (int i = 0; i < StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList.Values.Count; i++)
        {
            if (data["m_ItemStorageData"]["m_shopItemData"][i.ToString()] == null)
                continue;

            int key = keys[i];

            if (StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData == null)
                continue;

            for (int j = 0; j < StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData.Count; j++)
            {
                if (data["m_ItemStorageData"]["m_shopItemData"][i.ToString()][j.ToString()] == null)
                    continue;

                if (StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData[j] == null)
                    continue;

                for (uint k = 0; k < StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData[j].Count; k++)
                {
                    if (data["m_ItemStorageData"]["m_shopItemData"][i.ToString()][j.ToString()][k.ToString()] == null)
                        continue;

                    uint id = (uint)data["m_ItemStorageData"]["m_shopItemData"][i.ToString()][j.ToString()][k.ToString()]["id"];
                    if (StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData[j] != null && StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData[j].ContainsKey(id))
                    {
                        StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData[j][id].Active = (bool)data["m_ItemStorageData"]["m_shopItemData"][i.ToString()][j.ToString()][k.ToString()]["Active"];
                        StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList[key].m_productsData[j][id].SoldOutTimeCount = (int)data["m_ItemStorageData"]["m_shopItemData"][i.ToString()][j.ToString()][k.ToString()]["SoldOutTimeCount"];
                    }
                }
            }
        }

        return true;
    }

    private static void SaveItemStorageData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_ItemStorageData = new Dictionary<string, object>();

        Dictionary<int, object> m_itemDataListTbl = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_ItemStorageData.m_itemDataListTbl.Length; i++)
        {
            if (StorageData.m_ItemStorageData.m_itemDataListTbl[i] == null)
            {
                continue;
            }

            Dictionary<int, object> items = new Dictionary<int, object>();
            for (int j = 0; j < StorageData.m_ItemStorageData.m_itemDataListTbl[i].Count; j++)
            {
                if (StorageData.m_ItemStorageData.m_itemDataListTbl[i][j] != null)
                {
                    if (StorageData.m_ItemStorageData.m_itemDataListTbl[i][j].m_itemID != System.UInt32.MaxValue)
                    {
                        Dictionary<string, object> item = new Dictionary<string, object>();
                        item["m_itemID"] = StorageData.m_ItemStorageData.m_itemDataListTbl[i][j].m_itemID;
                        item["m_itemNum"] = StorageData.m_ItemStorageData.m_itemDataListTbl[i][j].m_itemNum;
                        items[j] = item;
                    }
                }
            }
            m_itemDataListTbl[i] = items;
        }
        m_ItemStorageData["m_itemDataListTbl"] = m_itemDataListTbl;

        Dictionary<int, object> m_shopItemData = new Dictionary<int, object>();

        if (StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList != null)
        {
            foreach (SellerActiveInfo value in StorageData.m_ItemStorageData.m_shopItemData.m_sellerActiveInfoList.Values)
            {
                Dictionary<int, object> sellerActiveInfo = new Dictionary<int, object>();
                for (int i = 0; i < value.m_productsData.Count; i++)
                {
                    Dictionary<int, object> m_productsData = new Dictionary<int, object>();
                    List<ShopItemData.SellerActiveInfo.ShopItemData> array = new List<SellerActiveInfo.ShopItemData>();
                    if (value.m_productsData[i] != null)
                    {
                        foreach (ShopItemData.SellerActiveInfo.ShopItemData data in value.m_productsData[i].Values)
                            array.Add(data);

                    }

                    if (array.Count == 0)
                        continue;

                    for (int j = 0; j < array.Count; j++)
                    {
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data["id"] = array[j].MasterData.id;
                        data["Active"] = array[j].Active;
                        data["SoldOutTimeCount"] = array[j].SoldOutTimeCount;
                        m_productsData[j] = data;
                    }
                    sellerActiveInfo[i] = m_productsData;
                }
                m_shopItemData[m_shopItemData.Count] = sellerActiveInfo;
            }
            m_ItemStorageData["m_shopItemData"] = m_shopItemData;
        }

        game_buffer["m_ItemStorageData"] = m_ItemStorageData;
    }
    #endregion

    #region FixedTimeData
    private static bool LoadFixedTimeData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_FixedTimeData.OnTimeList.Count; i++)
        {
            FixedOnTimeData fixedOnTimeData = StorageData.m_FixedTimeData.OnTimeList[i];
            if (fixedOnTimeData != null)
            {
                fixedOnTimeData.m_actionID = (FixedTimeData.ActionID)(int)data["m_FixedTimeData"][i.ToString()]["m_actionID"];
                fixedOnTimeData.m_nowDay = (WorldData.DAY)(int)data["m_FixedTimeData"][i.ToString()]["m_nowDay"];
                fixedOnTimeData.m_nowWeak = (WorldData.WEAK)(int)data["m_FixedTimeData"][i.ToString()]["m_nowWeak"];
                fixedOnTimeData.m_nowSeason = (WorldData.SEASON)(int)data["m_FixedTimeData"][i.ToString()]["m_nowSeason"];
                fixedOnTimeData.m_setHour = (int)data["m_FixedTimeData"][i.ToString()]["m_setHour"];
                fixedOnTimeData.m_setMinutes = (int)data["m_FixedTimeData"][i.ToString()]["m_setMinutes"];
                fixedOnTimeData.m_loopFunc = (bool)data["m_FixedTimeData"][i.ToString()]["m_loopFunc"];
                fixedOnTimeData.m_state = (FixedOnTimeData.State)(int)data["m_FixedTimeData"][i.ToString()]["m_state"];
            }
        }

        return true;
    }

    private static void SaveFixedTimeData(Dictionary<string, object> game_buffer)
    {
        Dictionary<int, object> m_FixedTimeData = new Dictionary<int, object>();

        for (int i = 0; i < StorageData.m_FixedTimeData.OnTimeList.Count; i++)
        {
            FixedOnTimeData fixedOnTimeData = StorageData.m_FixedTimeData.OnTimeList[i];
            if (fixedOnTimeData != null)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data["m_actionID"] = (int)fixedOnTimeData.m_actionID;
                data["m_nowDay"] = (int)fixedOnTimeData.m_nowDay;
                data["m_nowWeak"] = (int)fixedOnTimeData.m_nowWeak;
                data["m_nowSeason"] = (int)fixedOnTimeData.m_nowSeason;
                data["m_setHour"] = fixedOnTimeData.m_setHour;
                data["m_setMinutes"] = fixedOnTimeData.m_setMinutes;
                data["m_loopFunc"] = fixedOnTimeData.m_loopFunc;
                data["m_state"] = (int)fixedOnTimeData.m_state;
                m_FixedTimeData[i] = data;
            }
        }

        game_buffer["m_FixedTimeData"] = m_FixedTimeData;
    }
    #endregion

    #region GradeUpData
    private static bool LoadGradeUpData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_gradeUpData.m_gradeUpDatas.Length; i++)
        {
            TownGradeUpData townGradeUpData = StorageData.m_gradeUpData.m_gradeUpDatas[i];
            townGradeUpData.m_id = (uint)data["m_gradeUpData"][i.ToString()]["m_id"];
            townGradeUpData.m_current_grade = (int)data["m_gradeUpData"][i.ToString()]["m_current_grade"];
            townGradeUpData.m_next_grade = (int)data["m_gradeUpData"][i.ToString()]["m_next_grade"];
            townGradeUpData.m_grade_up_time = (float)data["m_gradeUpData"][i.ToString()]["m_grade_up_time"];
        }
        return true;
    }

    private static void SaveGradeUpData(Dictionary<string, object> game_buffer)
    {
        Dictionary<int, object> m_gradeUpData = new Dictionary<int, object>();

        for (int i = 0; i < StorageData.m_gradeUpData.m_gradeUpDatas.Length; i++)
        {
            TownGradeUpData townGradeUpData = StorageData.m_gradeUpData.m_gradeUpDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_id"] = townGradeUpData.m_id;
            data["m_current_grade"] = townGradeUpData.m_current_grade;
            data["m_next_grade"] = townGradeUpData.m_next_grade;
            data["m_grade_up_time"] = townGradeUpData.m_grade_up_time;
            m_gradeUpData[i] = data;
        }

        game_buffer["m_gradeUpData"] = m_gradeUpData;
    }
    #endregion

    #region MaterialData
    private static bool LoadMaterialData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_materialData.m_materialDatas.Length; i++)
        {
            MaterialData materialData = StorageData.m_materialData.m_materialDatas[i];
            materialData.m_id = (uint)data["m_materialData"][i.ToString()]["m_id"];
            materialData.m_material_num = (int)data["m_materialData"][i.ToString()]["m_material_num"];
            materialData.m_is_get = (bool)data["m_materialData"][i.ToString()]["m_is_get"];
        }
        return true;
    }

    private static void SaveMaterialData(Dictionary<string, object> game_buffer)
    {
        Dictionary<int, object> m_materialData = new Dictionary<int, object>();

        for (int i = 0; i < StorageData.m_materialData.m_materialDatas.Length; i++)
        {
            MaterialData materialData = StorageData.m_materialData.m_materialDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_id"] = materialData.m_id;
            data["m_material_num"] = materialData.m_material_num;
            data["m_is_get"] = materialData.m_is_get;
            m_materialData[i] = data;
        }

        game_buffer["m_materialData"] = m_materialData;
    }
    #endregion

    #region ColosseumData
    private static bool LoadColosseumData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_colosseumData.m_colosseumDatas.Length; i++)
        {
            ColosseumData colosseumData = StorageData.m_colosseumData.m_colosseumDatas[i];
            colosseumData.m_id = (uint)data["m_colosseumData"]["m_colosseumDatas"][i.ToString()]["m_id"];
            colosseumData.m_is_clear = (bool)data["m_colosseumData"]["m_colosseumDatas"][i.ToString()]["m_is_clear"];
            colosseumData.m_is_clear_today = (bool)data["m_colosseumData"]["m_colosseumDatas"][i.ToString()]["m_is_clear_today"];
        }

        StorageData.m_colosseumData.m_colosseumTotalWinCount = (int)data["m_colosseumData"]["m_colosseumTotalWinCount"];
        return true;
    }

    private static void SaveColosseumData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_colosseumData = new Dictionary<string, object>();

        Dictionary<int, object> m_colosseumDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_colosseumData.m_colosseumDatas.Length; i++)
        {
            ColosseumData colosseumData = StorageData.m_colosseumData.m_colosseumDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_id"] = colosseumData.m_id;
            data["m_is_clear"] = colosseumData.m_is_clear;
            data["m_is_clear_today"] = colosseumData.m_is_clear_today;
            m_colosseumDatas[i] = data;
        }
        m_colosseumData["m_colosseumDatas"] = m_colosseumDatas;
        m_colosseumData["m_colosseumTotalWinCount"] = StorageData.m_colosseumData.m_colosseumTotalWinCount;

        game_buffer["m_colosseumData"] = m_colosseumData;
    }
    #endregion

    #region FarmData
    private static bool LoadFarmData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_farmData.m_farmDatas.Length; i++)
        {
            FarmData farmData = StorageData.m_farmData.m_farmDatas[i];
            farmData.m_id = (uint)data["m_farmData"]["m_farmDatas"][i.ToString()]["m_id"];
            farmData.m_condition = (int)data["m_farmData"]["m_farmDatas"][i.ToString()]["m_condition"];
            farmData.m_pick_time = (float)data["m_farmData"]["m_farmDatas"][i.ToString()]["m_pick_time"];
            farmData.m_pick_num = (int)data["m_farmData"]["m_farmDatas"][i.ToString()]["m_pick_num"];
            farmData.m_pick_day_bonus = (bool)data["m_farmData"]["m_farmDatas"][i.ToString()]["m_pick_day_bonus"];
        }

        for (int i = 0; i < StorageData.m_farmData.m_farmItemDatas.Length; i++)
        {
            FarmItemData farmItemData = StorageData.m_farmData.m_farmItemDatas[i];
            farmItemData.m_id = (uint)data["m_farmData"]["m_farmItemDatas"][i.ToString()]["m_id"];
            farmItemData.m_is_get = (bool)data["m_farmData"]["m_farmItemDatas"][i.ToString()]["m_is_get"];
        }

        return true;
    }

    private static void SaveFarmData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_farmData = new Dictionary<string, object>();

        Dictionary<int, object> m_farmDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_farmData.m_farmDatas.Length; i++)
        {
            FarmData farmData = StorageData.m_farmData.m_farmDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_id"] = farmData.m_id;
            data["m_condition"] = farmData.m_condition;
            data["m_pick_time"] = farmData.m_pick_time;
            data["m_pick_num"] = farmData.m_pick_num;
            data["m_pick_day_bonus"] = farmData.m_pick_day_bonus;
            m_farmDatas[i] = data;
        }
        m_farmData["m_farmDatas"] = m_farmDatas;

        Dictionary<int, object> m_farmItemDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_farmData.m_farmItemDatas.Length; i++)
        {
            FarmItemData farmItemData = StorageData.m_farmData.m_farmItemDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_id"] = farmItemData.m_id;
            data["m_is_get"] = farmItemData.m_is_get;
            m_farmItemDatas[i] = data;
        }
        m_farmData["m_farmItemDatas"] = m_farmItemDatas;

        game_buffer["m_farmData"] = m_farmData;
    }
    #endregion

    #region TradeData
    private static bool LoadTradeData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_tradeData.m_tradeDatas.Length; i++)
        {
            TradeData tradeData = StorageData.m_tradeData.m_tradeDatas[i];

            tradeData.m_id = (uint)data["m_tradeData"]["m_tradeDatas"][i.ToString()]["m_id"];

            for (int j = 0; j < tradeData.m_correction_kinds.Length; j++)
            {
                tradeData.m_correction_kinds[j] = (int)data["m_tradeData"]["m_tradeDatas"][i.ToString()]["m_correction_kinds"][j.ToString()];
            }

            for (int j = 0; j < tradeData.m_market_history.Length; j++)
            {
                tradeData.m_market_history[j] = (int)data["m_tradeData"]["m_tradeDatas"][i.ToString()]["m_market_history"][j.ToString()];
            }

            tradeData.m_num = (int)data["m_tradeData"]["m_tradeDatas"][i.ToString()]["m_num"];
            tradeData.m_is_today_buy = (bool)data["m_tradeData"]["m_tradeDatas"][i.ToString()]["m_is_today_buy"];
        }

        StorageData.m_tradeData.m_totalTradeSale = (uint)data["m_tradeData"]["m_totalTradeSale"];
        StorageData.m_tradeData.m_maxTradeSale = (uint)data["m_tradeData"]["m_maxTradeSale"];

        return true;
    }

    private static void SaveTradeData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_tradeData = new Dictionary<string, object>();

        Dictionary<int, object> m_tradeDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_tradeData.m_tradeDatas.Length; i++)
        {
            TradeData tradeData = StorageData.m_tradeData.m_tradeDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();

            data["m_id"] = tradeData.m_id;

            Dictionary<int, object> m_correction_kinds = new Dictionary<int, object>();
            for (int j = 0; j < tradeData.m_correction_kinds.Length; j++)
            {
                m_correction_kinds[j] = tradeData.m_correction_kinds[j];
            }
            data["m_correction_kinds"] = m_correction_kinds;

            Dictionary<int, object> m_market_history = new Dictionary<int, object>();
            for (int j = 0; j < tradeData.m_market_history.Length; j++)
            {
                m_market_history[j] = tradeData.m_market_history[j];
            }
            data["m_market_history"] = m_market_history;

            data["m_num"] = tradeData.m_num;
            data["m_is_today_buy"] = tradeData.m_is_today_buy;

            m_tradeDatas[i] = data;
        }
        m_tradeData["m_tradeDatas"] = m_tradeDatas;

        m_tradeData["m_totalTradeSale"] = StorageData.m_tradeData.m_totalTradeSale;
        m_tradeData["m_maxTradeSale"] = StorageData.m_tradeData.m_maxTradeSale;

        game_buffer["m_tradeData"] = m_tradeData;
    }
    #endregion

    #region TrainingData
    private static bool LoadTrainingData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_trainingData.m_trainingDatas.Length; i++)
        {
            TrainingData trainingData = StorageData.m_trainingData.m_trainingDatas[i];
            trainingData.m_id = (uint)data["m_trainingData"]["m_trainingDatas"][i.ToString()]["m_id"];
            trainingData.m_training_exp = (int)data["m_trainingData"]["m_trainingDatas"][i.ToString()]["m_training_exp"];
            trainingData.m_current_grade = (int)data["m_trainingData"]["m_trainingDatas"][i.ToString()]["m_current_grade"];
            trainingData.m_next_grade = (int)data["m_trainingData"]["m_trainingDatas"][i.ToString()]["m_next_grade"];
            trainingData.m_grade_up_time = (float)data["m_trainingData"]["m_trainingDatas"][i.ToString()]["m_grade_up_time"];
        }

        StorageData.m_trainingData.m_isLastBouns = (bool)data["m_trainingData"]["m_isLastBouns"];
        StorageData.m_trainingData.m_trainingTotalCount = (int)data["m_trainingData"]["m_trainingTotalCount"];
        return true;
    }

    private static void SaveTrainingData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_trainingData = new Dictionary<string, object>();

        Dictionary<int, object> m_trainingDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_trainingData.m_trainingDatas.Length; i++)
        {
            TrainingData trainingData = StorageData.m_trainingData.m_trainingDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_id"] = trainingData.m_id;
            data["m_training_exp"] = trainingData.m_training_exp;
            data["m_current_grade"] = trainingData.m_current_grade;
            data["m_next_grade"] = trainingData.m_next_grade;
            data["m_grade_up_time"] = trainingData.m_grade_up_time;
            m_trainingDatas[i] = data;
        }
        m_trainingData["m_trainingDatas"] = m_trainingDatas;

        m_trainingData["m_isLastBouns"] = StorageData.m_trainingData.m_isLastBouns;
        m_trainingData["m_trainingTotalCount"] = StorageData.m_trainingData.m_trainingTotalCount;

        game_buffer["m_trainingData"] = m_trainingData;
    }
    #endregion

    #region TrainingMenuData
    private static bool LoadTrainingMenuData(JsonNode data)
    {
        for (int i = 0; i < StorageData.m_trainingMenuData.m_trainingMenuDatas.Length; i++)
        {
            TrainingMenuData trainingMenuData = StorageData.m_trainingMenuData.m_trainingMenuDatas[i];
            trainingMenuData.m_ids[0] = (uint)data["m_trainingMenuData"]["m_trainingMenuDatas"][i.ToString()]["m_ids"]["0"];
            trainingMenuData.m_ids[1] = (uint)data["m_trainingMenuData"]["m_trainingMenuDatas"][i.ToString()]["m_ids"]["1"];
            trainingMenuData.m_bit = (int)data["m_trainingMenuData"]["m_trainingMenuDatas"][i.ToString()]["m_bit"];
            trainingMenuData.m_time = (float)data["m_trainingMenuData"]["m_trainingMenuDatas"][i.ToString()]["m_time"];
            trainingMenuData.m_weight = (int)data["m_trainingMenuData"]["m_trainingMenuDatas"][i.ToString()]["m_weight"];
        }

        StorageData.m_trainingMenuData.m_keep_time = (float)data["m_trainingMenuData"]["m_keep_time"];

        return true;
    }

    private static void SaveTrainingMenuData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_trainingMenuData = new Dictionary<string, object>();

        Dictionary<int, object> m_trainingMenuDatas = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_trainingMenuData.m_trainingMenuDatas.Length; i++)
        {
            TrainingMenuData trainingMenuData = StorageData.m_trainingMenuData.m_trainingMenuDatas[i];
            Dictionary<string, object> data = new Dictionary<string, object>();

            Dictionary<int, object> m_ids = new Dictionary<int, object>();
            m_ids[0] = trainingMenuData.m_ids[0];
            m_ids[1] = trainingMenuData.m_ids[1];
            data["m_ids"] = m_ids;

            data["m_bit"] = trainingMenuData.m_bit;
            data["m_time"] = trainingMenuData.m_time;
            data["m_weight"] = trainingMenuData.m_weight;
            m_trainingMenuDatas[i] = data;
        }
        m_trainingMenuData["m_trainingMenuDatas"] = m_trainingMenuDatas;

        m_trainingMenuData["m_keep_time"] = StorageData.m_trainingMenuData.m_keep_time;

        game_buffer["m_trainingMenuData"] = m_trainingMenuData;
    }
    #endregion

    #region DigitalMessangerData
    private static bool LoadDigitalMessangerData(JsonNode data)
    {
        StorageData.m_digitalMessangerData.m_DigitalMessengerLastSendData.m_LatestMailID = (int)data["m_digitalMessangerData"]["m_LatestMailID"];

        for (int i = 0; i < StorageData.m_digitalMessangerData.m_DigitalMessengerData.Length; i++)
        {
            DigitalMessengerData digitalMessengerData = StorageData.m_digitalMessangerData.m_DigitalMessengerData[i];
            digitalMessengerData.m_State = (int)data["m_digitalMessangerData"]["m_DigitalMessengerData"][i.ToString()]["m_State"];
            digitalMessengerData.m_AttachedItem1 = (uint)data["m_digitalMessangerData"]["m_DigitalMessengerData"][i.ToString()]["m_AttachedItem1"];
            digitalMessengerData.m_AttachedItem2 = (uint)data["m_digitalMessangerData"]["m_DigitalMessengerData"][i.ToString()]["m_AttachedItem2"];
            digitalMessengerData.m_AttachedItem3 = (uint)data["m_digitalMessangerData"]["m_DigitalMessengerData"][i.ToString()]["m_AttachedItem3"];
            digitalMessengerData.m_AttachedPoint = (uint)data["m_digitalMessangerData"]["m_DigitalMessengerData"][i.ToString()]["m_AttachedPoint"];
            digitalMessengerData.m_ReceiptTime = (float)data["m_digitalMessangerData"]["m_DigitalMessengerData"][i.ToString()]["m_ReceiptTime"];
        }

        return true;
    }

    private static void SaveDigitalMessangerData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_digitalMessangerData = new Dictionary<string, object>();

        m_digitalMessangerData["m_LatestMailID"] = StorageData.m_digitalMessangerData.m_DigitalMessengerLastSendData.m_LatestMailID;

        Dictionary<int, object> m_DigitalMessengerData = new Dictionary<int, object>();
        for (int i = 0; i < StorageData.m_digitalMessangerData.m_DigitalMessengerData.Length; i++)
        {
            DigitalMessengerData digitalMessengerData = StorageData.m_digitalMessangerData.m_DigitalMessengerData[i];
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["m_State"] = digitalMessengerData.m_State;
            data["m_AttachedItem1"] = digitalMessengerData.m_AttachedItem1;
            data["m_AttachedItem2"] = digitalMessengerData.m_AttachedItem2;
            data["m_AttachedItem3"] = digitalMessengerData.m_AttachedItem3;
            data["m_AttachedPoint"] = digitalMessengerData.m_AttachedPoint;
            data["m_ReceiptTime"] = digitalMessengerData.m_ReceiptTime;
            m_DigitalMessengerData[i] = data;
        }
        m_digitalMessangerData["m_DigitalMessengerData"] = m_DigitalMessengerData;

        game_buffer["m_digitalMessangerData"] = m_digitalMessangerData;
    }
    #endregion

    #region DigiviceMapData
    private static bool LoadDigiviceMapData(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveDigiviceMapData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region AreaArrivalFlag
    private static bool LoadAreaArrivalFlag(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveAreaArrivalFlag(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region DailyQuestData
    private static bool LoadDailyQuestData(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveDailyQuestData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region IjigenBoxData
    private static bool LoadIjigenBoxData(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveIjigenBoxData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region DigimonCardFlag
    private static bool LoadDigimonCardFlag(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveDigimonCardFlag(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region BattleRecord
    private static bool LoadBattleRecord(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveBattleRecord(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region QuestItemCounter
    private static bool LoadQuestItemCounter(JsonNode data)
    {
        string example = (string)data["example"];
        return true;
    }

    private static void SaveQuestItemCounter(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> exampleData = new Dictionary<string, object>();
        exampleData["example"] = "example";
        game_buffer["exampleData"] = exampleData;
    }
    #endregion

    #region PlayTimeData
    private static bool LoadPlayTimeData(JsonNode data)
    {
        StorageData.m_PlayTimeData.m_PlayTime = (double)data["m_PlayTimeData"]["m_PlayTime"];
        return true;
    }

    private static void SavePlayTimeData(Dictionary<string, object> game_buffer)
    {
        Dictionary<string, object> m_PlayTimeData = new Dictionary<string, object>();
        m_PlayTimeData["m_PlayTime"] = StorageData.m_PlayTimeData.m_PlayTime;
        game_buffer["m_PlayTimeData"] = m_PlayTimeData;
    }
    #endregion
}
