﻿using System;
using System.Collections.Generic;
using System.Numerics;
using static ParameterCommonSelectWindow;
using static uDigiviceBG;

namespace DWNOLib.Library;
public class ParameterManagerLib
{
    public static Dictionary<UnityEngine.SystemLanguage, Dictionary<uint, string>> LanguageDatas { get; private set; } = new Dictionary<UnityEngine.SystemLanguage, Dictionary<uint, string>>
    {
        { UnityEngine.SystemLanguage.English, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.French, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.German, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.Italian, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.Japanese, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.Korean, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.Portuguese, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.Spanish, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.ChineseTraditional, new Dictionary<uint, string> { } },
        { UnityEngine.SystemLanguage.Unknown, new Dictionary<uint, string> { } }, // Spanish - Latin American
    };

    /// <summary>
    /// Dictionaries containing a string to an id existing inside the LanguageDatas.
    /// </summary>
    public static Dictionary<UnityEngine.SystemLanguage, Dictionary<string, uint>> LanguageStringDatas { get; private set; } = new Dictionary<UnityEngine.SystemLanguage, Dictionary<string, uint>>
    {
        { UnityEngine.SystemLanguage.English, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.French, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.German, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.Italian, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.Japanese, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.Korean, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.Portuguese, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.Spanish, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.ChineseTraditional, new Dictionary<string, uint> { } },
        { UnityEngine.SystemLanguage.Unknown, new Dictionary<string, uint> { } }, // Spanish - Latin American
    };

    // Custom datas
    public static List<ParameterDigimonData> CustomDigimonDatas { get; set; } = new List<ParameterDigimonData>();

    public static List<ParameterUsableSkillData> CustomUsableSkillDatas { get; set; } = new List<ParameterUsableSkillData>();

    public static List<DigiviceSoloCameraData> CustomSoloCameraDatas { get; set; } = new List<DigiviceSoloCameraData>();

    public static Dictionary<Vector2, List<ParameterPlacementEnemy>> CustomPlacementEnemyDatas { get; set; } = new Dictionary<Vector2, List<ParameterPlacementEnemy>>();

    public static Dictionary<Vector2, List<ParameterPlacementNpc>> CustomPlacementNPCDatas { get; set; } = new Dictionary<Vector2, List<ParameterPlacementNpc>>();

    public static Dictionary<int, List<ParameterCommonSelectWindow>> CustomCommonSelectWindowDatas { get; set; } = new Dictionary<int, List<ParameterCommonSelectWindow>>();

    public static Dictionary<int, Dictionary<int, int>> CustomCommonSelectWindowSort { get; set; } = new Dictionary<int, Dictionary<int, int>>();

    public static List<ParameterShopItemData> CustomShopItemDatas { get; set; } = new List<ParameterShopItemData>();

    public static Dictionary<string, Action> CustomScriptCommands { get; set; } = new Dictionary<string, Action>();

    // Parameters converted to list/dictionary for faster iteration
    public static List<ParameterDigimonData> DigimonDataList { get; set; } = new List<ParameterDigimonData>();

    public static List<ParameterUsableSkillData> UsableSkillList { get; set; } = new List<ParameterUsableSkillData>();

    public static List<DigiviceSoloCameraData> SoloCameraDataList { get; set; } = new List<DigiviceSoloCameraData>();

    public static List<ParameterPlacementEnemy> PlacementEnemyDataList { get; set; } = new List<ParameterPlacementEnemy>();

    public static List<ParameterPlacementNpc> PlacementNPCDataList { get; set; } = new List<ParameterPlacementNpc>();

    public static List<ParameterShopItemData> ShopItemList { get; set; } = new List<ParameterShopItemData>();

    public static List<ParameterItemData> ItemDataList { get; set; } = new List<ParameterItemData>();

    public static void AddLanguage(uint id, string text, UnityEngine.SystemLanguage language = UnityEngine.SystemLanguage.English)
    {
        if (!LanguageDatas.ContainsKey(language))
            LanguageDatas.Add(language, new Dictionary<uint, string> { });

        LanguageDatas[language].Add(id, text);
    }

    public static void AddTextLanguage(string text, uint id, UnityEngine.SystemLanguage language = UnityEngine.SystemLanguage.English)
    {
        if (!LanguageDatas.ContainsKey(language))
            LanguageStringDatas.Add(language, new Dictionary<string, uint> { });

        LanguageStringDatas[language].Add(text, id);
    }

    /// <summary>
    /// Add a new Digimon entry. Aside from the id, all params are optional, and default to Agumon's params.
    /// </summary>
    public static void AddDigimon(
        uint m_id,
        string m_mdlName = "c001",
        ParameterDigimonData.GrowthIndex m_growth = ParameterDigimonData.GrowthIndex.Growth,
        ParameterDigimonData.PersonalityType m_personality = ParameterDigimonData.PersonalityType.Enthusiastic,
        int m_attr = 1,
        int m_specialty = 4,
        int m_weak = 9,
        int m_favorite = 0,
        int m_life_style = 1,
        int m_toilet = 1,
        int m_meal_size = 2,
        int m_sleep_time = 2,
        AppInfo.NatureIndex m_nature = AppInfo.NatureIndex.Flame,
        ParameterDigimonData.FloatType m_floatType = ParameterDigimonData.FloatType.groundAlways,
        ParameterDigimonData.StepType m_stepType = ParameterDigimonData.StepType.jump,
        float m_headRotLimitX = 20,
        float m_headRotLimitY = 60,
        int m_permitDamage = 450,
        uint m_attack1 = 2311875725,
        uint m_attack2 = 3342761353,
        uint m_attack3 = 3342761353,
        uint m_attack4 = 3342761353,
        uint m_attackSP = 3941219574,
        //uint m_sp_code = 2249084409, // Unused
        uint m_highTension = 3857721755,
        //int m_attackPermit = 0, // Unused
        int m_dlc_no = 0,
        int m_flags = 0,
        int m_hp = 900,
        int m_mp = 650,
        int m_forcefulness = 95,
        int m_robustness = 80,
        int m_cleverness = 40,
        int m_rapidity = 50,
        int m_lifetime = 5,
        int m_weight = 20,
        int m_se_type = 0,
        int m_crySe = 5,
        float m_effectSize = 1.0f,
        float m_bulletMagnification = 1.0f,
        float m_cameraMagnificationX = 1.5f,
        float m_cameraMagnificationZ = 2.0f,
        float m_talkEventScale = 1.0f,
        int m_talkEventPos1 = 245,
        int m_talkEventPos2 = 246,
        int m_talkEventPos3 = 247,
        int m_talkEventPos4 = 248,
        int m_talkEventPos5 = 249,
        int m_talkEventPos6 = 250,
        int m_talkEventPos7 = 251,
        int m_evo_hpmax = 1900,
        int m_evo_mpmax = 999999,
        int m_evo_forcefulness = 890,
        int m_evo_robustness = 999999,
        int m_evo_cleverness = 999999,
        int m_evo_rapidity = 999999,
        int m_evo_weight = 999999,
        int m_evo_mistakes = 999999,
        int m_evo_bounds = 999999,
        int m_evo_education = 999999,
        int m_evo_battlewin = 999999,
        uint m_key1 = 2199896590,
        uint m_key2 = 2199896590,
        uint m_key3 = 2199896590,
        int m_evo_matches = 2,
        uint m_evolution1 = 3059157304,
        uint m_evolution2 = 3025602002,
        uint m_evolution3 = 3025602006,
        uint m_evolution4 = 3109490161,
        uint m_evolution5 = 2199896590,
        uint m_evo1_flagset = 4011245244,
        uint m_dlc_flagset = 4011245244,
        uint m_beforeID1 = 3059157304,
        uint m_beforeID2 = 3025602002,
        uint m_beforeID3 = 3025602006,
        uint m_beforeID4 = 3109490161,
        uint m_beforeID5 = 2199896590,
        uint m_possibleJogless = 2199896590,
        uint m_jogless = 2199896590,
        uint m_possibleMiracle = 2199896590,
        uint m_miracle = 2199896590,
        int m_partnerFlag = 1,
        string m_infoId = "c001",
        int m_footPrintType = 0,
        int m_fcmdEntryType = 1
    )
    {
        CustomDigimonDatas.Add(new ParameterDigimonData()
        {
            m_id = m_id,
            m_mdlName = m_mdlName,
            m_growth = (int)m_growth,
            m_personality = (int)m_personality,
            m_attr = m_attr,
            m_specialty = m_specialty,
            m_weak = m_weak,
            m_favorite = m_favorite,
            m_life_style = m_life_style,
            m_toilet = m_toilet,
            m_meal_size = m_meal_size,
            m_sleep_time = m_sleep_time,
            m_nature = (int)m_nature,
            m_floatType = (int)m_floatType,
            m_stepType = (int)m_stepType,
            m_headRotLimitX = m_headRotLimitX,
            m_headRotLimitY = m_headRotLimitY,
            m_permitDamage = m_permitDamage,
            m_attack1 = m_attack1,
            m_attack2 = m_attack2,
            m_attack3 = m_attack3,
            m_attack4 = m_attack4,
            m_attackSP = m_attackSP,
            //m_sp_code = m_sp_code,
            m_highTension = m_highTension,
            //m_attackPermit = m_attackPermit,
            m_dlc_no = m_dlc_no,
            m_flags = m_flags,
            m_hp = m_hp,
            m_mp = m_mp,
            m_forcefulness = m_forcefulness,
            m_robustness = m_robustness,
            m_cleverness = m_cleverness,
            m_rapidity = m_rapidity,
            m_lifetime = m_lifetime,
            m_weight = m_weight,
            m_se_type = m_se_type,
            m_crySe = m_crySe,
            m_effectSize = m_effectSize,
            m_bulletMagnification = m_bulletMagnification,
            m_cameraMagnificationX = m_cameraMagnificationX,
            m_cameraMagnificationZ = m_cameraMagnificationZ,
            m_talkEventScale = m_talkEventScale,
            m_talkEventPos1 = m_talkEventPos1,
            m_talkEventPos2 = m_talkEventPos2,
            m_talkEventPos3 = m_talkEventPos3,
            m_talkEventPos4 = m_talkEventPos4,
            m_talkEventPos5 = m_talkEventPos5,
            m_talkEventPos6 = m_talkEventPos6,
            m_talkEventPos7 = m_talkEventPos7,
            m_evo_hpmax = m_evo_hpmax,
            m_evo_mpmax = m_evo_mpmax,
            m_evo_forcefulness = m_evo_forcefulness,
            m_evo_robustness = m_evo_robustness,
            m_evo_cleverness = m_evo_cleverness,
            m_evo_rapidity = m_evo_rapidity,
            m_evo_weight = m_evo_weight,
            m_evo_mistakes = m_evo_mistakes,
            m_evo_bounds = m_evo_bounds,
            m_evo_education = m_evo_education,
            m_evo_battlewin = m_evo_battlewin,
            m_key1 = m_key1,
            m_key2 = m_key2,
            m_key3 = m_key3,
            m_evo_matches = m_evo_matches,
            m_evolution1 = m_evolution1,
            m_evolution2 = m_evolution2,
            m_evolution3 = m_evolution3,
            m_evolution4 = m_evolution4,
            m_evolution5 = m_evolution5,
            m_evo1_flagset = m_evo1_flagset,
            m_dlc_flagset = m_dlc_flagset,
            m_beforeID1 = m_beforeID1,
            m_beforeID2 = m_beforeID2,
            m_beforeID3 = m_beforeID3,
            m_beforeID4 = m_beforeID4,
            m_beforeID5 = m_beforeID5,
            m_possibleJogless = m_possibleJogless,
            m_jogless = m_jogless,
            m_possibleMiracle = m_possibleMiracle,
            m_miracle = m_miracle,
            m_partnerFlag = m_partnerFlag,
            m_infoId = m_infoId,
            m_footPrintType = m_footPrintType,
            m_fcmdEntryType = m_fcmdEntryType
        });
    }

    public static void AddCommonSelectWindow(
        ParameterCommonSelectWindowMode.WindowType commonSelectWindowModeIndex,
        uint m_langid1,
        uint m_langid2 = uint.MaxValue,
        int m_value = -1,
        OutModeParam m_out_mode1 = OutModeParam.None,
        ModeFormat m_out_format1 = ModeFormat.None,
        int m_out_value1 = 0,
        uint m_out_item1 = 354103372,
        uint m_out_digimon1 = 2199896590,
        OutModeParam m_out_mode2 = OutModeParam.None,
        ModeFormat m_out_format2 = ModeFormat.None,
        int m_out_value2 = 0,
        uint m_out_item2 = 354103372,
        uint m_out_digimon2 = 2199896590,
        SelectModeParam m_select_mode1 = SelectModeParam.None,
        ModeFormat m_select_format1 = ModeFormat.None,
        int m_select_value1 = 0,
        uint m_select_item1 = 354103372,
        uint m_select_digimon1 = 2199896590,
        SelectModeParam m_select_mode2 = SelectModeParam.None,
        ModeFormat m_select_format2 = ModeFormat.None,
        int m_select_value2 = 0,
        uint m_select_item2 = 354103372,
        uint m_select_digimon2 = 2199896590,
        string m_scriptCommand = "",
        string m_scriptCommandParam1 = "",
        string m_scriptCommandParam2 = "",
        string m_scriptCommandParam3 = "",
        string m_scriptCommandParam4 = "",
        string m_scriptCommandParam5 = "",
        string m_scriptCommandParam6 = "",
        string m_scriptCommandParam7 = "",
        string m_scriptCommandParam8 = ""
    )
    {
        if (!CustomCommonSelectWindowDatas.ContainsKey((int)commonSelectWindowModeIndex))
            CustomCommonSelectWindowDatas.Add((int)commonSelectWindowModeIndex, new List<ParameterCommonSelectWindow>());
        CustomCommonSelectWindowDatas[(int)commonSelectWindowModeIndex].Add(new ParameterCommonSelectWindow
        {
            m_langid1 = m_langid1,
            m_langid2 = m_langid2,
            m_value = m_value,
            m_out_mode1 = (int)m_out_mode1,
            m_out_format1 = (int)m_out_format1,
            m_out_value1 = m_out_value1,
            m_out_item1 = m_out_item1,
            m_out_digimon1 = m_out_digimon1,
            m_out_mode2 = (int)m_out_mode2,
            m_out_format2 = (int)m_out_format2,
            m_out_value2 = m_out_value2,
            m_out_item2 = m_out_item2,
            m_out_digimon2 = m_out_digimon2,
            m_select_mode1 = (int)m_select_mode1,
            m_select_format1 = (int)m_select_format1,
            m_select_value1 = m_select_value1,
            m_select_item1 = m_select_item1,
            m_select_digimon1 = m_select_digimon1,
            m_select_mode2 = (int)m_select_mode2,
            m_select_format2 = (int)m_select_format2,
            m_select_value2 = m_select_value2,
            m_select_item2 = m_select_item2,
            m_select_digimon2 = m_select_digimon2,
            m_scriptCommand = m_scriptCommand,
            m_scriptCommandParam1 = m_scriptCommandParam1,
            m_scriptCommandParam2 = m_scriptCommandParam2,
            m_scriptCommandParam3 = m_scriptCommandParam3,
            m_scriptCommandParam4 = m_scriptCommandParam4,
            m_scriptCommandParam5 = m_scriptCommandParam5,
            m_scriptCommandParam6 = m_scriptCommandParam6,
            m_scriptCommandParam7 = m_scriptCommandParam7,
            m_scriptCommandParam8 = m_scriptCommandParam8
        });
    }

    /// <summary>
    /// Add a new DigiviceSoloCamera entry. Aside from the id, all params are optional, and default to Agumon's params.
    /// </summary>
    public static void AddDigiviceSoloCamera(
        uint m_id,
        float m_height = 0.0f, // All default parameters have that value, advised to not change.
        float m_unit_x = 0.7f,
        float m_unit_y = 0.16f,
        float m_unit_z = 0.0f,
        float m_unit_rot_x = 0.0f, // All default parameters have that value, advised to not change.
        float m_unit_rot_y = 195.0f,
        float m_unit_rot_z = 0.0f, // All default parameters have that value, advised to not change.
        float m_cam_x = 0.0f, // All default parameters have that value, advised to not change.
        float m_cam_y = 0.77f, // All default parameters have that value, advised to not change.
        float m_cam_z = -2.38f, // All default parameters have that value, advised to not change.
        float m_look_at_x = 0.0f, // All default parameters have that value, advised to not change.
        float m_look_at_y = 0.5f, // All default parameters have that value, advised to not change.
        float m_look_at_z = 0.0f // All default parameters have that value, advised to not change.
    )
    {
        CustomSoloCameraDatas.Add(new DigiviceSoloCameraData()
        {
            m_id = m_id,
            m_height = m_height,
            m_unit_x = m_unit_x,
            m_unit_y = m_unit_y,
            m_unit_z = m_unit_z,
            m_unit_rot_x = m_unit_rot_x,
            m_unit_rot_y = m_unit_rot_y,
            m_unit_rot_z = m_unit_rot_z,
            m_cam_x = m_cam_x,
            m_cam_y = m_cam_y,
            m_cam_z = m_cam_z,
            m_look_at_x = m_look_at_x,
            m_look_at_y = m_look_at_y,
            m_look_at_z = m_look_at_z
        });
    }

    /// <summary>
    /// Add an item in a shop. Aside from the ShopItem id and the itemID, all params are optional, and default
    /// to Tantomon's shop in the new city.
    /// </summary>
    public static void AddShopItem(
        uint m_id,
        uint m_itemID,
        uint m_buildID = 0x73d4af90,
        int m_sortID = 0,
        int m_sellerNo = 1,
        int m_grade = 1,
        int m_price = 0,
        int m_random = 100,
        uint m_scenarioEventFlagName = 0xef16bebc,
        int m_soldOutFlag = 0,
        int m_soldOutTime = 1
    )
    {
        CustomShopItemDatas.Add(new ParameterShopItemData()
        {
            m_id = m_id,
            m_itemID = m_itemID,
            m_buildID = m_buildID,
            m_sortID = m_sortID,
            m_sellerNo = m_sellerNo,
            m_grade = m_grade,
            m_price = m_price,
            m_random = m_random,
            m_scenarioEventFlagName = m_scenarioEventFlagName,
            m_soldOutFlag = m_soldOutFlag,
            m_soldOutTime = m_soldOutTime
        });
    }

    /// <summary>
    /// Add a new UsableSkill entry. Aside from the id, all params are optional, and default to Agumon's params.
    /// </summary>
    public static void AddUsableSkill(
        uint m_id,
        int m_SkillFlag1 = 15,
        int m_SkillFlag2 = 3072
    )
    {
        CustomUsableSkillDatas.Add(new ParameterUsableSkillData()
        {
            m_id = m_id,
            m_SkillFlag1 = m_SkillFlag1,
            m_SkillFlag2 = m_SkillFlag2
        });
    }

    /// <summary>
    /// Add a new PlacementEnemy entry. The "map_area" param is used with a Vector2, with the x value being the map,
    /// and the y value being the area. For example, "new Vector2(2, 7)" would be Nigh Plains - Ruins Lake.
    /// Aside from the map_area, all params are optional, and default to an agumon lv1.
    /// </summary>
    public static void AddPlacementEnemy(
        Vector2 map_area,
        uint m_paramId = 0,
        MainGameManager.UNITID m_unitNo = MainGameManager.UNITID.Enemy00,
        uint m_id = 0x34ee6f05,
        uint m_attack1 = 0xdb1a0de8,
        uint m_attack2 = 0xc73e7d89,
        uint m_attack3 = 0xc73e7d89,
        uint m_attack4 = 0xc73e7d89,
        int m_usesp_hp = -1,
        ParameterPlacementEnemy.RetireAndWakeTime m_bedTime = ParameterPlacementEnemy.RetireAndWakeTime.Morning,
        ParameterPlacementEnemy.RetireAndWakeTime m_wakeupTime = ParameterPlacementEnemy.RetireAndWakeTime.Morning,
        int m_level = 0,
        int m_hp = 0,
        int m_mp = 0,
        int m_forcefulness = 0,
        int m_robustness = 0,
        int m_cleverness = 0,
        int m_rapidity = 0,
        int m_permitDamage = -1,
        int m_possessionBit = 100,
        int m_exp = 10,
        int m_itemDropNo = 37,
        float m_startX = 0f,
        float m_startY = 0f,
        float m_startZ = 0f,
        float m_startRy = 0f,
        int m_routeId = -1,
        int m_moveRange = 4,
        float m_vigilance_dist = 20f,
        float m_chase_dist = 20f,
        float m_chase_time = 3f,
        float m_chase_speed_coeff = 0.7f,
        float m_alermDist = 0.5f,
        int m_Incidence = 100,
        MainGameBattle.BattleBgm m_BattleBgm = MainGameBattle.BattleBgm.Normal
    )
    {
        if (!CustomPlacementEnemyDatas.ContainsKey(map_area))
            CustomPlacementEnemyDatas.Add(map_area, new List<ParameterPlacementEnemy>());

        CustomPlacementEnemyDatas[map_area].Add(new ParameterPlacementEnemy()
        {
            m_paramId = m_paramId,
            m_unitNo = (int)m_unitNo,
            m_id = m_id,
            m_attack1 = m_attack1,
            m_attack2 = m_attack2,
            m_attack3 = m_attack3,
            m_attack4 = m_attack4,
            m_usesp_hp = m_usesp_hp,
            m_bedTime = (int)m_bedTime,
            m_wakeupTime = (int)m_wakeupTime,
            m_level = m_level,
            m_hp = m_hp,
            m_mp = m_mp,
            m_forcefulness = m_forcefulness,
            m_robustness = m_robustness,
            m_cleverness = m_cleverness,
            m_rapidity = m_rapidity,
            m_permitDamage = m_permitDamage,
            m_possessionBit = m_possessionBit,
            m_exp = m_exp,
            m_itemDropNo = m_itemDropNo,
            m_startX = m_startX,
            m_startY = m_startY,
            m_startZ = m_startZ,
            m_startRy = m_startRy,
            m_routeId = m_routeId,
            m_moveRange = m_moveRange,
            m_vigilance_dist = m_vigilance_dist,
            m_chase_dist = m_chase_dist,
            m_chase_time = m_chase_time,
            m_chase_speed_coeff = m_chase_speed_coeff,
            m_alermDist = m_alermDist,
            m_Incidence = m_Incidence,
            m_BattleBgm = (int)m_BattleBgm
        });
    }

    /// <summary>
    /// Add a new PlacementNPC entry. The "map_area" param is used with a Vector2, with the x value being the map,
    /// and the y value being the area. For example, "new Vector2(2, 7)" would be Nigh Plains - Ruins Lake.
    /// Aside from the map_area, all params are optional, and default to a silent (no dialog) agumon.
    /// </summary>
    public static void AddPlacementNPC(
        Vector2 map_area,
        uint m_Name = 0,
        MainGameManager.UNITID m_UintId = MainGameManager.UNITID.Npc00,
        string m_MdlName = "c001",
        float m_Px = 0f,
        float m_Py = 0f,
        float m_Pz = 0f,
        float m_Ry = 0f,
        ParameterDigimonData.FloatType m_IsGround = ParameterDigimonData.FloatType.groundAlways,
        int m_RootId = -1,
        int m_range = -1,
        int m_ChkInput = 1,
        uint m_Message = 0xbe0c011,
        int m_Shape = 0,
        float m_Height = 2f,
        float m_Width = 2f,
        string m_CmdBlock = "",
        MainGameManager.Facility m_Facility = MainGameManager.Facility.None,
        uint m_NpcEnemyParamId = 0xffffffff
    )
    {
        if (!CustomPlacementNPCDatas.ContainsKey(map_area))
            CustomPlacementNPCDatas.Add(map_area, new List<ParameterPlacementNpc>());

        CustomPlacementNPCDatas[map_area].Add(new ParameterPlacementNpc()
        {
            m_Name = m_Name,
            m_UintId = (int)m_UintId,
            m_MdlName = m_MdlName,
            m_Px = m_Px,
            m_Py = m_Py,
            m_Pz = m_Pz,
            m_Ry = m_Ry,
            m_IsGround = (int)m_IsGround,
            m_RootId = m_RootId,
            m_range = m_range,
            m_ChkInput = m_ChkInput,
            m_Message = m_Message,
            m_Shape = m_Shape,
            m_Height = m_Height,
            m_Width = m_Width,
            m_CmdBlock = m_CmdBlock,
            m_Facility = (int)m_Facility,
            m_NpcEnemyParamId = m_NpcEnemyParamId
        });
    }
}
