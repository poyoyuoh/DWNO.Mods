﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Randomizer;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.Randomizer";
    internal const string PluginName = "Randomizer";
    internal const string PluginVersion = "1.0.0";

    private static System.Random rng { get; set; }

    private static ConfigEntry<int> Seed { get; set; }

    private static ConfigEntry<bool> SaveSeed { get; set; }

    private static ConfigEntry<bool> TrueEvolutionRandomize { get; set; }

    private static ConfigEntry<bool> RandomizeAllEvolutionSlot { get; set; }

    private static ConfigEntry<bool> PlacementEnemyChangeToSameGrowth { get; set; }

    private static ConfigEntry<bool> HideEvolutionInfo { get; set; }

    private static Dictionary<int, List<uint>> DigimonDatas { get; set; } = new Dictionary<int, List<uint>>();

    private static Dictionary<uint, uint> PlacementEnemyDatas { get; set; } = new Dictionary<uint, uint>();

    private static ConfigFile config { get; set; }

    public override void Load()
    {
        Seed = Config.Bind("#General", "Seed", -1, "The seed of the randomizer. Setting it to -1 make it generate a new seed on game start.");
        SaveSeed = Config.Bind("#General", "SaveSeed", true, "If true, Save the new seed value for the next restart.");
        TrueEvolutionRandomize = Config.Bind("#General", "TrueEvolutionRandomize", false, "If false, the randomizer will make sure every digimon can be obtained, if true, every evolution will be random and mean some digimon might not be available.");
        RandomizeAllEvolutionSlot = Config.Bind("#General", "RandomizeAllEvolutionSlot", false, "If true, every digimon will have 5 evolutions, if false, digimons will keep their normal amount of evolution.");
        PlacementEnemyChangeToSameGrowth = Config.Bind("#General", "PlacementEnemyChangeToSameGrowth", true, "If true, enemy will always be replaced by a digimon of the same growth (child into child, adult into adult, etc...), if false, they can be any digimon.");
        HideEvolutionInfo = Config.Bind("#General", "HideEvolutionInfo", true, "If true, hide the Key Digimon, Nature, Property and Digimon you will evolve into.");
        config = Config;
        Harmony.CreateAndPatchAll(typeof(Plugin));
    }

    [HarmonyPatch(typeof(uGenelogyInformationUI), "Initialize")]
    [HarmonyPostfix]
    private static void uGenelogyInformationUI_Initialize_Postfix(uGenelogyInformationUI __instance)
    {
        if (HideEvolutionInfo.Value)
        {
            // Yoink from Yashajin's HideEvoMon mod
            var HeadTransform = __instance.m_After.transform.Find("Head").gameObject.transform;
            HeadTransform.Find("Key_Digimon").gameObject.active = false;
            HeadTransform.Find("Nature").gameObject.active = false;
            HeadTransform.Find("Property").gameObject.active = false;
            HeadTransform.Find("Digimon").gameObject.active = false;
        }
    }

    [HarmonyPatch(typeof(uDigiviceLibraryDetailAfter), "SetCurrentDigiID")]
    [HarmonyPostfix]
    private static void uGenelogyInformationUI_SetCurrentDigiID_Postfix(uDigiviceLibraryDetailAfter __instance)
    {
        if (HideEvolutionInfo.Value)
        {
            Transform after = __instance.transform.GetChild(0).gameObject.active ? __instance.transform.GetChild(0) :
                __instance.transform.GetChild(1).gameObject.active ? __instance.transform.GetChild(1) :
                __instance.transform.GetChild(2).gameObject.active ? __instance.transform.GetChild(2) :
                __instance.transform.GetChild(3).gameObject.active ? __instance.transform.GetChild(3) :
                __instance.transform.GetChild(4);

            after.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(AppMainScript), "_FinishedParameterLoad")]
    [HarmonyPrefix]
    private static void AppMainScript__FinishedParameterLoad_Prefix(AppMainScript __instance)
    {
        StartRandomizer();
    }

    [HarmonyPatch(typeof(EnemyManager), "CreateNormalEnemy")]
    [HarmonyPostfix]
    private static void EnemyManager_CreateNormalEnemy_Postfix(EnemyManager __instance)
    {
        RandomizePlacementEnemyDatas(__instance);
    }

    [HarmonyPatch(typeof(uTitlePanel), "enablePanel")]
    [HarmonyPostfix]
    private static void uTitlePanel_enablePanel_Postfix(uTitlePanel __instance)
    {
        __instance.m_VersionText.alignment = TextAnchor.LowerRight;
        __instance.m_VersionText.text = $"Version 1.0.0\nRandomizer seed: {Seed.Value}\nPress F5 to generate a new seed!";
    }

    [HarmonyPatch(typeof(uTitlePanel), "Update")]
    [HarmonyPostfix]
    private static void uTitlePanel_Update_Postfix(uTitlePanel __instance)
    {
        if (Input.GetKeyDown(KeyCode.F5))
            StartRandomizer(true, __instance);
    }

    private static void StartRandomizer(bool force_new_seed = false, uTitlePanel __instance = null)
    {
        if (Seed.Value == -1 || force_new_seed)
            Seed.Value = new System.Random().Next();
        rng = new System.Random(Seed.Value);

        if (SaveSeed.Value)
            config.Save();

        if (force_new_seed && __instance != null)
            __instance.m_VersionText.text = $"Version 1.0.0\nRandomizer seed: {Seed.Value}\nPress F5 to generate a new seed!";

        DigimonDatas.Clear();
        PlacementEnemyDatas.Clear();

        SetupDigimonDatas(AppMainScript.Ref);
        RandomizeEvolutions(AppMainScript.Ref);
        SetupPlacementEnemyDatas(AppMainScript.Ref);
    }

    private static void SetupDigimonDatas(AppMainScript __instance)
    {
        foreach (ParameterDigimonData @params in __instance.m_parameters.m_csvbDigimonData.m_params)
        {
            // Unlock all evos
            @params.m_dlc_flagset = 4011245244;
            @params.m_evo1_flagset = 4011245244;
            @params.m_dlc_no = 0;

            if (@params.m_partnerFlag == 1)
            {
                if (!DigimonDatas.ContainsKey(@params.m_growth))
                {
                    DigimonDatas.Add(@params.m_growth, new List<uint>());
                }
                
                DigimonDatas[@params.m_growth].Add(@params.m_id);
            }
        }

        foreach (List<uint> data in DigimonDatas.Values)
            data.OrderBy(_ => rng.Next()).ToList();
    }

    private static void RandomizeEvolutions(AppMainScript __instance)
    {
        Dictionary<int, List<uint>> copy = new Dictionary<int, List<uint>>(DigimonDatas);
        Dictionary<int, List<int>> previous_index = new Dictionary<int, List<int>>();
        foreach (ParameterDigimonData @params in __instance.m_parameters.m_csvbDigimonData.m_params)
        {
            if (!previous_index.ContainsKey(@params.m_growth + 1))
                previous_index.Add(@params.m_growth + 1, new List<int>());

            if (DigimonDatas.ContainsKey(@params.m_growth + 1) && @params.m_partnerFlag == 1)
            {
                if (TrueEvolutionRandomize.Value)
                    previous_index[@params.m_growth + 1].Clear();

                if (previous_index[@params.m_growth + 1].Count >= DigimonDatas[@params.m_growth + 1].Count)
                    previous_index[@params.m_growth + 1].Clear();

                if (RandomizeAllEvolutionSlot.Value || @params.m_evolution1 != 0x831fc20e)
                {
                    int index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    while (previous_index[@params.m_growth + 1].Contains(index))
                        index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    previous_index[@params.m_growth + 1].Add(index);
                    uint id = DigimonDatas[@params.m_growth + 1][index];
                    @params.m_evolution1 = id;
                }

                if (previous_index[@params.m_growth + 1].Count >= DigimonDatas[@params.m_growth + 1].Count)
                    previous_index[@params.m_growth + 1].Clear();

                if (RandomizeAllEvolutionSlot.Value || @params.m_evolution2 != 0x831fc20e)
                {
                    int index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    while (previous_index[@params.m_growth + 1].Contains(index))
                        index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    previous_index[@params.m_growth + 1].Add(index);
                    uint id = DigimonDatas[@params.m_growth + 1][index];
                    @params.m_evolution2 = id;
                }

                if (previous_index[@params.m_growth + 1].Count >= DigimonDatas[@params.m_growth + 1].Count)
                    previous_index[@params.m_growth + 1].Clear();

                if (RandomizeAllEvolutionSlot.Value || @params.m_evolution3 != 0x831fc20e)
                {
                    int index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    while (previous_index[@params.m_growth + 1].Contains(index))
                        index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    previous_index[@params.m_growth + 1].Add(index);
                    uint id = DigimonDatas[@params.m_growth + 1][index];
                    @params.m_evolution3 = id;
                }

                if (previous_index[@params.m_growth + 1].Count >= DigimonDatas[@params.m_growth + 1].Count)
                    previous_index[@params.m_growth + 1].Clear();

                if (RandomizeAllEvolutionSlot.Value || @params.m_evolution4 != 0x831fc20e)
                {
                    int index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    while (previous_index[@params.m_growth + 1].Contains(index))
                        index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    previous_index[@params.m_growth + 1].Add(index);
                    uint id = DigimonDatas[@params.m_growth + 1][index];
                    @params.m_evolution4 = id;
                }

                if (previous_index[@params.m_growth + 1].Count >= DigimonDatas[@params.m_growth + 1].Count)
                    previous_index[@params.m_growth + 1].Clear();

                if (RandomizeAllEvolutionSlot.Value || @params.m_evolution5 != 0x831fc20e)
                {
                    int index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    while (previous_index[@params.m_growth + 1].Contains(index))
                        index = rng.Next(DigimonDatas[@params.m_growth + 1].Count);
                    previous_index[@params.m_growth + 1].Add(index);
                    uint id = DigimonDatas[@params.m_growth + 1][index];
                    @params.m_evolution5 = id;
                }
            }
        }
    }

    private static void SetupPlacementEnemyDatas(AppMainScript __instance)
    {
        List<uint> used_id = new List<uint>();
        foreach (ParameterDigimonData @params in __instance.m_parameters.m_csvbDigimonData.m_params)
        {
            if (@params.m_partnerFlag == 1)
            {
                uint replacement_id;

                if (PlacementEnemyChangeToSameGrowth.Value)
                {
                    int index = rng.Next(DigimonDatas[@params.m_growth].Count);
                    replacement_id = DigimonDatas[@params.m_growth][index];
                    while (used_id.Contains(replacement_id))
                    {
                        index = rng.Next(DigimonDatas[@params.m_growth].Count);
                        replacement_id = DigimonDatas[@params.m_growth][index];
                    }
                    used_id.Add(replacement_id);
                }
                else
                {
                    int growth = rng.Next(DigimonDatas.Count);
                    int index = rng.Next(DigimonDatas[growth].Count);
                    replacement_id = DigimonDatas[growth][index];
                    while (used_id.Contains(replacement_id))
                    {
                        growth = rng.Next(DigimonDatas.Count);
                        index = rng.Next(DigimonDatas[growth].Count);
                        replacement_id = DigimonDatas[growth][index];
                    }
                    used_id.Add(replacement_id);
                }

                PlacementEnemyDatas.Add(@params.m_id, replacement_id);
            }
        }
    }

    private static void RandomizePlacementEnemyDatas(EnemyManager __instance)
    {
        foreach (ParameterPlacementEnemy @params in __instance.m_placementEnemyList)
        {
            if (PlacementEnemyDatas.ContainsKey(@params.m_id))
                @params.m_id = PlacementEnemyDatas[@params.m_id];
        }
    }
}
