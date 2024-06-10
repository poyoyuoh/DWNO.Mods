using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
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

    public static List<ParameterAttackData> CustomAttackDatas { get; set; } = new List<ParameterAttackData>();
    
    public static List<ParameterUsableSkillData> CustomUsableSkillDatas { get; set; } = new List<ParameterUsableSkillData>();

    public static List<DigiviceSoloCameraData> CustomSoloCameraDatas { get; set; } = new List<DigiviceSoloCameraData>();

    public static Dictionary<Vector2, List<ParameterPlacementEnemy>> CustomPlacementEnemyDatas { get; set; } = new Dictionary<Vector2, List<ParameterPlacementEnemy>>();

    public static Dictionary<Vector2, List<ParameterPlacementNpc>> CustomPlacementNPCDatas { get; set; } = new Dictionary<Vector2, List<ParameterPlacementNpc>>();
    
    public static Dictionary<Vector2, List<ParameterNpcEnemyData>> CustomNPCEnemyDatas { get; set; } = new Dictionary<Vector2, List<ParameterNpcEnemyData>>();

    public static Dictionary<int, List<ParameterCommonSelectWindow>> CustomCommonSelectWindowDatas { get; set; } = new Dictionary<int, List<ParameterCommonSelectWindow>>();

    public static Dictionary<int, Dictionary<int, int>> CustomCommonSelectWindowSort { get; set; } = new Dictionary<int, Dictionary<int, int>>();

    public static List<ParameterItemData> CustomItemDatas { get; set; } = new List<ParameterItemData>();

    public static List<ParameterItemDataBattle> CustomItemBattleDatas { get; set; } = new List<ParameterItemDataBattle>();
    
    public static List<ParameterItemDataEvolution> CustomItemEvolutionDatas { get; set; } = new List<ParameterItemDataEvolution>();
    
    public static List<ParameterItemDataOther> CustomItemOtherDatas { get; set; } = new List<ParameterItemDataOther>();
    
    public static List<ParameterItemDataRecovery> CustomItemRecoveryDatas { get; set; } = new List<ParameterItemDataRecovery>();
    
    public static List<ParameterItemDataFood> CustomItemFoodDatas { get; set; } = new List<ParameterItemDataFood>();
    
    public static List<ParameterItemDataMaterial> CustomItemMaterialDatas { get; set; } = new List<ParameterItemDataMaterial>();
    
    public static List<ParameterItemDataKeyItem> CustomItemKeyItemDatas { get; set; } = new List<ParameterItemDataKeyItem>();

    public static List<ParameterDropItem> CustomDropItemDatas { get; set; } = new List<ParameterDropItem>();

    public static List<ParameterShopItemData> CustomShopItemDatas { get; set; } = new List<ParameterShopItemData>();

    public static List<ParameterJoglessData> CustomJoglessDatas { get; set; } = new List<ParameterJoglessData>();

    public static List<ParameterJoglessGroupData> CustomJoglessGroupDatas { get; set; } = new List<ParameterJoglessGroupData>();

    public static Dictionary<string, Action> CustomScriptCommands { get; set; } = new Dictionary<string, Action>();

    // Parameters converted to list/dictionary for faster iteration
    public static List<ParameterDigimonData> DigimonDataList { get; set; } = new List<ParameterDigimonData>();

    public static List<ParameterAttackData> AttackList { get; set; } = new List<ParameterAttackData>();

    public static List<ParameterUsableSkillData> UsableSkillList { get; set; } = new List<ParameterUsableSkillData>();

    public static List<DigiviceSoloCameraData> SoloCameraDataList { get; set; } = new List<DigiviceSoloCameraData>();

    public static List<ParameterPlacementEnemy> PlacementEnemyDataList { get; set; } = new List<ParameterPlacementEnemy>();

    public static List<ParameterPlacementNpc> PlacementNPCDataList { get; set; } = new List<ParameterPlacementNpc>();

    public static List<ParameterNpcEnemyData> NPCEnemyDataList { get; set; } = new List<ParameterNpcEnemyData>();

    public static List<ParameterItemData> ItemDataList { get; set; } = new List<ParameterItemData>();

    public static List<ParameterItemDataBattle> ItemBattleDataList { get; set; } = new List<ParameterItemDataBattle>();
    
    public static List<ParameterItemDataEvolution> ItemEvolutionDataList { get; set; } = new List<ParameterItemDataEvolution>();
    
    public static List<ParameterItemDataOther> ItemOtherDataList { get; set; } = new List<ParameterItemDataOther>();
    
    public static List<ParameterItemDataRecovery> ItemRecoveryDataList { get; set; } = new List<ParameterItemDataRecovery>();
    
    public static List<ParameterItemDataFood> ItemFoodDataList { get; set; } = new List<ParameterItemDataFood>();
    
    public static List<ParameterItemDataMaterial> ItemMaterialDataList { get; set; } = new List<ParameterItemDataMaterial>();
    
    public static List<ParameterItemDataKeyItem> ItemKeyItemDataList { get; set; } = new List<ParameterItemDataKeyItem>();

    public static List<ParameterShopItemData> ShopItemList { get; set; } = new List<ParameterShopItemData>();

    public static List<ParameterDropItem> DropItemList { get; set; } = new List<ParameterDropItem>();

    public static List<ParameterJoglessData> JoglessList { get; set; } = new List<ParameterJoglessData>();

    public static List<ParameterJoglessGroupData> JoglessGroupList { get; set; } = new List<ParameterJoglessGroupData>();

    public static void AddLanguage(uint id, string text, UnityEngine.SystemLanguage language = UnityEngine.SystemLanguage.English)
    {
        if (!LanguageDatas.ContainsKey(language))
            LanguageDatas.Add(language, new Dictionary<uint, string> { });

        if (LanguageDatas[language].ContainsKey(id))
        {
            Logger.Log($"LanguageDatas for language {language} already contain the id {id} and will get overriden.", Logger.LogType.Warning);
            Logger.Log($"{LanguageDatas[language][id]} -> {text}", Logger.LogType.Warning);
            LanguageDatas[language][id] = text;
        }
        else
            LanguageDatas[language].Add(id, text);
    }

    public static void AddTextLanguage(string text, uint id, UnityEngine.SystemLanguage language = UnityEngine.SystemLanguage.English)
    {
        if (!LanguageDatas.ContainsKey(language))
            LanguageStringDatas.Add(language, new Dictionary<string, uint> { });

        if (LanguageStringDatas[language].ContainsKey(text))
        {
            Logger.Log($"LanguageStringDatas for language {language} already contain the text {text} and will get overriden.", Logger.LogType.Warning);
            Logger.Log($"{LanguageStringDatas[language][text]} -> {id}", Logger.LogType.Warning);
            LanguageStringDatas[language][text] = id;
        }
        else
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
    /// Add a new ItemData entry. Aside from the id, all params are optional, and default to the Recovery Disk params.
    /// </summary>
    public static void AddItem(
        uint m_id,
        ParameterItemData.KindIndex m_kind = ParameterItemData.KindIndex.KindRecovery,
        ParameterItemData.ClassIndex m_class = ParameterItemData.ClassIndex.ClassGeneralPurpose,
        int m_sortID = 1000,
        int m_price = 100,
        int m_initialPossession = 0,
        int m_itemSetPriority = -1,
        string m_triggeredSound = "O_014",
        string m_modelName = "disk",
        int m_mdlVariation = 7,
        string m_iconName = "IT_item284",
        string m_effectName = "o_05_s",
        float m_offset_px = 0.15f,
        float m_offset_py = -0.13f,
        float m_offset_pz = 0.01f,
        float m_offset_rx = 8.5f,
        float m_offset_ry = 0f,
        float m_offset_rz = 120f
    )
    {
        CustomItemDatas.Add(new ParameterItemData()
        {
            m_id = m_id,
            m_kind = (int)m_kind,
            m_class = (int)m_class,
            m_sortID = m_sortID,
            m_price = m_price,
            m_initialPossession = m_initialPossession,
            m_itemSetPriority = m_itemSetPriority,
            m_triggeredSound = m_triggeredSound,
            m_modelName = m_modelName,
            m_mdlVariation = m_mdlVariation,
            m_iconName = m_iconName,
            m_effectName = m_effectName,
            m_offset_px = m_offset_px,
            m_offset_py = m_offset_py,
            m_offset_pz = m_offset_pz,
            m_offset_rx = m_offset_rx,
            m_offset_ry = m_offset_ry,
            m_offset_rz = m_offset_rz
        });
    }

    /// <summary>
    /// Add a new ItemDataBattle entry. Aside from the id and description code, all params are optional, and default to the Attack Plugin params.
    /// </summary>
    public static void AddItemBattle(
        uint m_id,
        uint m_description_code,
        ParameterItemDataBattle.TargetIndex m_target = ParameterItemDataBattle.TargetIndex.TargetSimple,
        ParameterItemDataBattle.RangeIndex m_range = ParameterItemDataBattle.RangeIndex.RangeSmall,
        ParameterItemDataBattle.KindIndex m_effect_kind = ParameterItemDataBattle.KindIndex.KindAttackUp,
        float m_effect_value = 1.2f,
        int m_probability = 100
    )
    {
        CustomItemBattleDatas.Add(new ParameterItemDataBattle()
        {
            m_id = m_id,
            m_description_code = m_description_code,
            m_target = (int)m_target,
            m_range = (int)m_range,
            m_effect_kind = (int)m_effect_kind,
            m_effect_value = m_effect_value,
            m_probability = m_probability
        });
    }

    /// <summary>
    /// Add a new ItemDataEvolution entry.
    /// </summary>
    public static void AddItemEvolution(
        uint m_id,
        uint m_description_code,
        uint m_digiimonID
    )
    {
        CustomItemEvolutionDatas.Add(new ParameterItemDataEvolution()
        {
            m_id = m_id,
            m_description_code = m_description_code,
            m_digiimonID = m_digiimonID
        });
    }

    /// <summary>
    /// Add a new ItemDataOther entry. Aside from the id and description_code, all params are optional.
    ///  TODO: Implement a way to check for special action on those item (like potty,medicine,etc)
    /// </summary>
    public static void AddItemOther(
        uint m_id,
        uint m_description_code,
        int m_hpMax = 0,
        int m_mpMax = 0,
        int m_forcefulness = 0,
        int m_robustness = 0,
        int m_cleverness = 0,
        int m_rapidity = 0,
        int m_lifeDecrease = 0,
        ParameterItemDataOther.Licensing m_howToUse = ParameterItemDataOther.Licensing.Normal
    )
    {
        CustomItemOtherDatas.Add(new ParameterItemDataOther()
        {
            m_id = m_id,
            m_description_code = m_description_code,
            m_hpMax = m_hpMax,
            m_mpMax = m_mpMax,
            m_forcefulness = m_forcefulness,
            m_robustness = m_robustness,
            m_cleverness = m_cleverness,
            m_rapidity = m_rapidity,
            m_lifeDecrease = m_lifeDecrease,
            m_howToUse = (int)m_howToUse
        });
    }

    /// <summary>
    /// Add a new ItemDataRecovery entry. Aside from the id and description_code, all params are optional, and default to the Recovery Disk params.
    /// </summary>
    public static void AddItemRecovery(
        uint m_id,
        uint m_description_code,
        int m_hpRecoveryAmount = 500,
        int m_mpRecoveryAmount = 0,
        int m_range = 2
    )
    {
        CustomItemRecoveryDatas.Add(new ParameterItemDataRecovery()
        {
            m_id = m_id,
            m_description_code = m_description_code,
            m_hpRecoveryAmount = m_hpRecoveryAmount,
            m_mpRecoveryAmount = m_mpRecoveryAmount,
            m_range = m_range
        });
    }

    /// <summary>
    /// Add a new ItemDataFood entry. Aside from the id and description_code, all params are optional, and default to the Meat params.
    /// </summary>
    public static void AddItemFood(
        uint m_id,
        uint m_description_code,
        ParameterItemDataFood.ItemFoodLineageIndex m_lineage = ParameterItemDataFood.ItemFoodLineageIndex.ItemFoodLineageMeat,
        int m_bonds = 0,
        int m_trust = 0,
        int m_activityTime = 0,
        int m_hpMax = 0,
        int m_hp = 0,
        int m_mpMax = 0,
        int m_mp = 0,
        int m_forcefulness = 0,
        int m_robustness = 0,
        int m_cleverness = 0,
        int m_rapidity = 0,
        int m_bodyWeight = 1,
        int m_lifeTime = 0,
        int m_education = 0,
        int m_mood = 2,
        int m_curse = 0,
        int m_satiety = 40,
        int m_genkiDegree = 0,
        int m_trainingFailure = 0,
        int m_trainingCorrectionHp = 0,
        int m_trainingCorrectionMp = 0,
        int m_trainingCorrectionForcefulness = 0,
        int m_trainingCorrectionRobustness = 0,
        int m_trainingCorrectionVleverness = 0,
        int m_trainingCorrectionRapidity = 0,
        int m_effectTime = 0,
        int m_conditionClear = 0,
        int m_sickRand = 0
    )
    {
        CustomItemFoodDatas.Add(new ParameterItemDataFood()
        {
            m_id = m_id,
            m_description_code = m_description_code,
            m_lineage = (int)m_lineage,
            m_bonds = m_bonds,
            m_trust = m_trust,
            m_activityTime = m_activityTime,
            m_hpMax = m_hpMax,
            m_hp = m_hp,
            m_mpMax = m_mpMax,
            m_mp = m_mp,
            m_forcefulness = m_forcefulness,
            m_robustness = m_robustness,
            m_cleverness = m_cleverness,
            m_rapidity = m_rapidity,
            m_bodyWeight = m_bodyWeight,
            m_lifeTime = m_lifeTime,
            m_education = m_education,
            m_mood = m_mood,
            m_curse = m_curse,
            m_satiety = m_satiety,
            m_genkiDegree = m_genkiDegree,
            m_trainingFailure = m_trainingFailure,
            m_trainingCorrectionHp = m_trainingCorrectionHp,
            m_trainingCorrectionMp = m_trainingCorrectionMp,
            m_trainingCorrectionForcefulness = m_trainingCorrectionForcefulness,
            m_trainingCorrectionRobustness = m_trainingCorrectionRobustness,
            m_trainingCorrectionVleverness = m_trainingCorrectionVleverness,
            m_trainingCorrectionRapidity = m_trainingCorrectionRapidity,
            m_effectTime = m_effectTime,
            m_conditionClear = m_conditionClear,
            m_sickRand = m_sickRand
        });
    }

    /// <summary>
    /// Add a new ItemDataMaterial entry. Make sure to create 2 of them, 1 for the individual material, and the other for the bag.
    /// </summary>
    public static void AddItemMaterial(
        uint m_id,
        uint m_description_code,
        uint m_description_code_for_construction,
        ParameterItemDataMaterial.MaterialKindIndex m_kind,
        uint m_expansion_id,
        uint m_convergent_id
    )
    {
        CustomItemMaterialDatas.Add(new ParameterItemDataMaterial()
        {
            m_id = m_id,
            m_description_code = m_description_code,
            m_description_code_for_construction = m_description_code_for_construction,
            m_kind = (int)m_kind,
            m_expansion_id = m_expansion_id,
            m_convergent_id = m_convergent_id
        });
    }

    /// <summary>
    /// Add a new ItemDataKeyItem entry.
    /// </summary>
    public static void AddItemKeyItem(
        uint m_id,
        uint m_description_code
    )
    {
        CustomItemKeyItemDatas.Add(new ParameterItemDataKeyItem()
        {
            m_id = m_id,
            m_description_code = m_description_code
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

    public static void AddNPCEnemy(
        Vector2 map_area,
        uint m_ParamId,
        //uint m_DigiParamId = 0x34ee6f05, // Never used
        uint m_AtkParamId01 = 0xdb1a0de8,
        uint m_AtkParamId02 = 0xc73e7d89,
        uint m_AtkParamId03 = 0xc73e7d89,
        uint m_AtkParamId04 = 0xc73e7d89,
        int m_SpHp = -1,
        int m_Level = 0,
        int m_Hp = 0,
        int m_Mp = 0,
        int m_Str = 0,
        int m_Def = 0,
        int m_Int = 0,
        int m_Spd = 0,
        int m_permitDamage = -1,
        int m_Money = 100,
        int m_Exp = 10,
        int m_DropItemTblIdx = 37,
        int m_BattleBgm = 1
    )
    {
        if (!CustomNPCEnemyDatas.ContainsKey(map_area))
            CustomNPCEnemyDatas.Add(map_area, new List<ParameterNpcEnemyData>());

        CustomNPCEnemyDatas[map_area].Add(new ParameterNpcEnemyData()
        {
            m_ParamId = m_ParamId,
            //m_DigiParamId = m_DigiParamId,
            m_AtkParamId01 = m_AtkParamId01,
            m_AtkParamId02 = m_AtkParamId02,
            m_AtkParamId03 = m_AtkParamId03,
            m_AtkParamId04 = m_AtkParamId04,
            m_SpHp = m_SpHp,
            m_Level = m_Level,
            m_Hp = m_Hp,
            m_Mp = m_Mp,
            m_Str = m_Str,
            m_Def = m_Def,
            m_Int = m_Int,
            m_Spd = m_Spd,
            m_permitDamage = m_permitDamage,
            m_Money = m_Money,
            m_Exp = m_Exp,
            m_DropItemTblIdx = m_DropItemTblIdx,
            m_BattleBgm = m_BattleBgm
        });
    }

    /// <summary>
    /// Add a new DropItem entry. Aside from the id and the first item, everything is set to a NG item and 0 probability.
    /// The last default index is 143, so use a bigger number to add new DropItem. Using a small number will replace existing one.
    /// The game doesn't support showing how much item dropped, so if you want to drop the same item multiple time, it's recommended
    /// to put the same item multiple times and setting the num to 1.
    /// </summary>
    public static void AddDropItem(
        int m_index,
        uint m_item1Id,
        int m_item1Num,
        int m_item1Probability,
        uint m_item2Id = 0x151b304c,
        int m_item2Num = 0,
        int m_item2Probability = 0,
        uint m_item3Id = 0x151b304c,
        int m_item3Num = 0,
        int m_item3Probability = 0,
        uint m_item4Id = 0x151b304c,
        int m_item4Num = 0,
        int m_item4Probability = 0,
        uint m_item5Id = 0x151b304c,
        int m_item5Num = 0,
        int m_item5Probability = 0
    )
    {
        CustomDropItemDatas.Add(new ParameterDropItem()
        {
            m_index = m_index,
            m_item1Id = m_item1Id,
            m_item1Num = m_item1Num,
            m_item1Probability = m_item1Probability,
            m_item2Id = m_item2Id,
            m_item2Num = m_item2Num,
            m_item2Probability = m_item2Probability,
            m_item3Id = m_item3Id,
            m_item3Num = m_item3Num,
            m_item3Probability = m_item3Probability,
            m_item4Id = m_item4Id,
            m_item4Num = m_item4Num,
            m_item4Probability = m_item4Probability,
            m_item5Id = m_item5Id,
            m_item5Num = m_item5Num,
            m_item5Probability = m_item5Probability
        });
    }

    public static void AddJogless(
        uint m_id,
        uint m_jogless1,
        int m_group1,
        uint m_jogless2 = 0x831fc20e,
        int m_group2 = 0,
        uint m_flag_set = 0xef16bebc
    )
    {
        CustomJoglessDatas.Add(new ParameterJoglessData()
        {
            m_id = m_id,
            m_jogless1 = m_jogless1,
            m_group1 = m_group1,
            m_jogless2 = m_jogless2,
            m_group2 = m_group2,
            m_flag_set = m_flag_set
        });
    }

    public static void AddJoglessGroup(
        int m_index,
        uint m_groupDigi1,
        uint m_groupDigi2 = 0x831fc20e,
        uint m_groupDigi3 = 0x831fc20e,
        uint m_groupDigi4 = 0x831fc20e,
        uint m_groupDigi5 = 0x831fc20e,
        uint m_groupDigi6 = 0x831fc20e,
        uint m_groupDigi7 = 0x831fc20e,
        uint m_groupDigi8 = 0x831fc20e,
        uint m_groupDigi9 = 0x831fc20e,
        uint m_groupDigi10 = 0x831fc20e,
        uint m_groupDigi11 = 0x831fc20e,
        uint m_groupDigi12 = 0x831fc20e,
        uint m_groupDigi13 = 0x831fc20e,
        uint m_groupDigi14 = 0x831fc20e,
        uint m_groupDigi15 = 0x831fc20e,
        uint m_groupDigi16 = 0x831fc20e
    )
    {
        CustomJoglessGroupDatas.Add(new ParameterJoglessGroupData()
        {
            m_index = m_index,
            m_groupDigi1 = m_groupDigi1,
            m_groupDigi2 = m_groupDigi2,
            m_groupDigi3 = m_groupDigi3,
            m_groupDigi4 = m_groupDigi4,
            m_groupDigi5 = m_groupDigi5,
            m_groupDigi6 = m_groupDigi6,
            m_groupDigi7 = m_groupDigi7,
            m_groupDigi8 = m_groupDigi8,
            m_groupDigi9 = m_groupDigi9,
            m_groupDigi10 = m_groupDigi10,
            m_groupDigi11 = m_groupDigi11,
            m_groupDigi12 = m_groupDigi12,
            m_groupDigi13 = m_groupDigi13,
            m_groupDigi14 = m_groupDigi14,
            m_groupDigi15 = m_groupDigi15,
            m_groupDigi16 = m_groupDigi16
        });
    }

    /// <summary>
    /// Add a new Attack entry. Aside from the id/typecode/description, all params are optional
    /// and default to a special attack.
    /// </summary>
    public static void AddAttack(
        uint m_id,
        uint m_typeCode,
        uint m_description_code,
        string m_iconName = "kara",
        int m_flagIndex = -1,
        int m_nature = 1,
        int m_attackPower = 1000,
        int m_break = 100,
        int m_consumptionMP = 0,
        int m_consumptionOP = 150,
        float m_forcefulnessUp = 0f,
        int m_forcefulnessUpTime = 0,
        float m_robustnessUp = 0f,
        int m_robustnessUpTime = 0,
        float m_clevernessUp = 0f,
        int m_clevernessUpTime = 0,
        float m_rapidityUp = 0f,
        int m_rapidityUpTime = 0,
        ParameterAttackData.AbnormalIndex m_abnormal = ParameterAttackData.AbnormalIndex.AbnormalIndex_None,
        float m_abnormalProb = 0f,
        float m_abnormalTime = 0f,
        int m_type = 0, // Seems to be about buff, set to 6 if it's a buff, otherwise keep 0
        ParameterAttackData.RangeIndex m_range = ParameterAttackData.RangeIndex.Range_S,
        ParameterAttackData.AttackTypeIndex m_kind = ParameterAttackData.AttackTypeIndex.AttackType_Target,
        int m_offsetNo0 = 0,
        float m_size = 0f,
        float m_distance = 0f,
        float m_speed = 0f,
        float m_rushSpeed = 0f,
        float m_homing = 0f,
        int m_motionNo = 6,
        float m_motionSpeed = 0f,
        float m_collisionStart = 0f,
        float m_collisionTime = 0f,
        string m_effect1 = "",
        int m_offsetNo1 = 0,
        float m_effectSize1 = 1f,
        float m_effectStartTime1 = 0f,
        string m_effect2 = "",
        int m_offsetNo2 = 0,
        float m_effectSize2 = 1f,
        float m_effectStartTime2 = 0f,
        string m_effect3 = "",
        int m_offsetNo3 = 0,
        float m_effectSize3 = 1f,
        float m_effectStartTime3 = 0f,
        string m_effect4 = "b_h_02",
        int m_offsetNo4 = 4,
        float m_effectSize4 = 1f,
        int m_damageMotionNo = 4,
        float m_knockBackPower = 2.5f,
        float m_launchPower = 8.0f,
        float m_hitSlow = 0.1f,
        float m_hitSlowTime = 2.0f,
        float m_quakePower = 0.25f,
        float m_quakeTime = 1.5f,
        int m_bulletNum = 0,
        float m_bulletInterval = 0f,
        float m_bulletBarake = 0f
    )
    {
        CustomAttackDatas.Add(new ParameterAttackData()
        {
            m_id = m_id,
            m_typeCode = m_typeCode,
            m_description_code = m_description_code,
            m_iconName = m_iconName,
            m_flagIndex = m_flagIndex,
            m_nature = m_nature,
            m_attackPower = m_attackPower,
            m_break = m_break,
            m_consumptionMP = m_consumptionMP,
            m_consumptionOP = m_consumptionOP,
            m_forcefulnessUp = m_forcefulnessUp,
            m_forcefulnessUpTime = m_forcefulnessUpTime,
            m_robustnessUp = m_robustnessUp,
            m_robustnessUpTime = m_robustnessUpTime,
            m_clevernessUp = m_clevernessUp,
            m_clevernessUpTime = m_clevernessUpTime,
            m_rapidityUp = m_rapidityUp,
            m_rapidityUpTime = m_rapidityUpTime,
            m_abnormal = (int)m_abnormal,
            m_abnormalProb = m_abnormalProb,
            m_abnormalTime = m_abnormalTime,
            m_type = m_type,
            m_range = (int)m_range,
            m_kind = (int)m_kind,
            m_offsetNo0 = m_offsetNo0,
            m_size = m_size,
            m_distance = m_distance,
            m_speed = m_speed,
            m_rushSpeed = m_rushSpeed,
            m_homing = m_homing,
            m_motionNo = m_motionNo,
            m_motionSpeed = m_motionSpeed,
            m_collisionStart = m_collisionStart,
            m_collisionTime = m_collisionTime,
            m_effect1 = m_effect1,
            m_offsetNo1 = m_offsetNo1,
            m_effectSize1 = m_effectSize1,
            m_effectStartTime1 = m_effectStartTime1,
            m_effect2 = m_effect2,
            m_offsetNo2 = m_offsetNo2,
            m_effectSize2 = m_effectSize2,
            m_effectStartTime2 = m_effectStartTime2,
            m_effect3 = m_effect3,
            m_offsetNo3 = m_offsetNo3,
            m_effectSize3 = m_effectSize3,
            m_effectStartTime3 = m_effectStartTime3,
            m_effect4 = m_effect4,
            m_offsetNo4 = m_offsetNo4,
            m_effectSize4 = m_effectSize4,
            m_damageMotionNo = m_damageMotionNo,
            m_knockBackPower = m_knockBackPower,
            m_launchPower = m_launchPower,
            m_hitSlow = m_hitSlow,
            m_hitSlowTime = m_hitSlowTime,
            m_quakePower = m_quakePower,
            m_quakeTime = m_quakeTime,
            m_bulletNum = m_bulletNum,
            m_bulletInterval = m_bulletInterval,
            m_bulletBarake = m_bulletBarake
        });
    }
}
