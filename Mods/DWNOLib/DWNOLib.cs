﻿using BepInEx;
using BepInEx.Unity.IL2CPP;
using DWNOLib.Extras;
using DWNOLib.Library;
using DWNOLib.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DWNOLib.Library.DialogManager;

namespace DWNOLib;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class DWNOLib : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.DWNOLib";
    internal const string PluginName = "DWNOLib";
    internal const string PluginVersion = "0.5.3";

    public static GameObject Canvas {  get; private set; }

    public static Harmony harmony { get; private set; }

    public static bool Debug { get; private set; } = true;

    public override void Load()
    {
        PatchAll();

        Logger.Log("\n" +
            "****************************************************" + "***************************************************\n" +
            "*                                                  *" + "                                                  *\n" +
            "*     ______        ___   _  ___  _     _ _        *" + "                                                  *\n" +
            "*    |  _ \\ \\      / / \\ | |/ _ \\| |   (_) |__     *" + "     Digimon World Next Order Modding Library     *\n" +
            "*    | | | \\ \\ /\\ / /|  \\| | | | | |   | | '_ \\    *" + "                 Made by poyoyuoh                 *\n" +
            "*    | |_| |\\ V  V / | |\\  | |_| | |___| | |_) |   *" + "   Join the Digimon Modding Community Discord :   *\n" +
            "*    |____/  \\_/\\_/  |_| \\_|\\___/|_____|_|_.__/    *" + "          https://discord.gg/cb5AuxU6su           *\n" +
            "*                                                  *" + "                                                  *\n" +
            "*                                                  *" + "                                                  *\n" +
            "****************************************************" + "***************************************************",
            ConsoleColor.Cyan,
            "Special");

        Logger.Log("DWNOLib is currently in alpha, DO NOT USE IN PRODUCTION!");

        // Replace the box talk event to disable the "online" screen.
        // TODO: Give the player the free item, maybe make them be in the storage by default if possible ?
        ParameterManagerLib.AddLanguage(20, "The box doesn't seems to work...");
        ParameterManagerLib.CustomScriptCommands["BOX_TALK00"] = () =>
        {
            List<Dialog> dialogs = new List<Dialog>()
            {
                new Dialog() { message = Language.GetString(20) },
            };

            StartDialog(dialogs);
        };

        if (Debug)
            UnitTests.Start();

        //Language.m_systemLangage = (int)SystemLanguage.Japanese;
    }

    private static void PatchAll()
    {
        harmony = Harmony.CreateAndPatchAll(typeof(DWNOLib));

        // Patches
        Harmony.CreateAndPatchAll(typeof(AppMainScriptPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(CScenarioScriptPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(CsvbPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(EnemyManagerPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(HashIdSearchClassPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(LanguagePatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(MainGameManagerPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(NpcManagerPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(ParameterCommonSelectWindowPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(ParameterDigimonDataPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(ParameterManagerPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(PlayerDataPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(ShopItemDataPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(StorageDataPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(SystemDataSaveLoadPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(uDigiviceBGPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(uSavePanelItemSaveItemPatch), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(uSavePanelPatch), harmony.Id);

        // Extras
        Harmony.CreateAndPatchAll(typeof(CriBindDir), harmony.Id);
        Harmony.CreateAndPatchAll(typeof(MainTitleLibVersionText), harmony.Id);
    }

    [HarmonyPatch(typeof(AppMainScript), "Start")]
    [HarmonyPrefix]
    private static void CreateCanvas()
    {
        Canvas = new GameObject("DWNOLibCanvas");
        UnityEngine.Object.DontDestroyOnLoad(Canvas);
        Canvas.hideFlags |= HideFlags.HideAndDontSave;
        Canvas.layer = 5;
        Canvas.transform.position = new Vector3(0f, 0f, 1f);
    }
}
