using DWNOLib.Library;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DWNOLib.Library.DialogManager;

namespace DWNOLib;
internal class UnitTests
{
    private static bool test_many_digimons { get; set; } = false;

    public static void Start()
    {
        if (test_many_digimons)
            TestAddManyDigimons();
        else
        {
            TestAddDigimon();
            TestAddItem();
            TestAddItemInShop();
            TestAddJogless();
            TestAddCommonSelectWindow();
            TestAddPlacementEnemy();
            TestAddPlacementNPC();
            TestAddTamerExpTable();
        }
    }

    private static void TestAddDigimon()
    {
        // A digimon use it's id to find it's name, so we add a new entry to our LangageData with that id.
        ParameterManagerLib.AddLanguage(1, "This is a custom digimon name");
        ParameterManagerLib.AddLanguage(2, "This is a custom digimon description");

        // Example of adding an entry for a specific language.
        ParameterManagerLib.AddLanguage(1, "これはカスタムネームです", SystemLanguage.Japanese);
        ParameterManagerLib.AddLanguage(2, "これはカスタム記述である", SystemLanguage.Japanese);

        // That's how you add a new digimon. You can individually specify what params you want to set with "ParamName: type".
        ParameterManagerLib.AddDigimon(1, m_mdlName: "m999", m_infoId: "m999");

        // Give our new digimon techs. THIS IS MANDATORY! The game will crash when getting the digimon otherwise.
        ParameterManagerLib.AddUsableSkill(1);

        // Give our new digimon a "camera" for inside the menu. Determine it's model position/rotation
        // in the partner and library menu. THIS IS MANDATORY! The game will crash when opening menu otherwise.
        ParameterManagerLib.AddDigiviceSoloCamera(1);

        // The description of a digimon is searched using it's "m_infoId" + "_d", so we add a text in our
        // LanguageStringData that point to an id existing inside our LanguageData.
        ParameterManagerLib.AddTextLanguage("m999_d", 2);
        ParameterManagerLib.AddTextLanguage("m999_d", 2, SystemLanguage.Japanese);
    }

    private static void TestAddManyDigimons()
    {
        // This is to test the game limit. 72 was the max before breaking the limit with the new save system.
        // At some point, around 300, game also crashed for other reason, didn't take note of why, but it was fixed.
        // The game have no issue handling a large amount, but the library interface started to struggle when i tried 10k.
        // But it's just due to how the game made that menu. Maybe fix it one day ? Doesn't seems really necessary as
        // realistically we will never even reach 1k entry anyways.
        int max = 1000;

        for (uint i = 1; i <= max;  i++)
        {
            ParameterManagerLib.AddLanguage(i, "This is a custom digimon name.");
            ParameterManagerLib.AddDigimon(i, m_mdlName: "c001", m_infoId: "c001");
            ParameterManagerLib.AddUsableSkill(i);
            ParameterManagerLib.AddDigiviceSoloCamera(i);
        }
        ParameterManagerLib.AddLanguage((uint)(max + 1), "This is a custom digimon description.");
        ParameterManagerLib.AddTextLanguage("c001_d", (uint)(max + 1));
    }

    private static void TestAddItem()
    {
        ParameterManagerLib.AddLanguage(25, "Custom item");
        ParameterManagerLib.AddLanguage(26, "Custom item description");

        // The id of the item and the item kind must be the same.
        ParameterManagerLib.AddItem(25);
        ParameterManagerLib.AddItemRecovery(25, 26, 99999);
    }

    private static void TestAddItemInShop()
    {
        // Add a "Phantom Nectar" in Tentomon's new town shop.
        ParameterManagerLib.AddShopItem(0, 0x81d4dfd9, m_sortID: 5);
    }

    private static void TestAddJogless()
    {
        // Make a jogress with Examon and MetalGarurumon (Black) that become Omegamon Zwart.
        // It is important that both digimon have a jogress into the wanted digimon.
        ParameterManagerLib.AddJogless(0x5c3a290b, 0xf9a2ef1a, 44, m_flag_set: 0x37d005e3);
        ParameterManagerLib.AddJoglessGroup(44, 0x5b3a279c);

        ParameterManagerLib.AddJogless(0x5b3a279c, 0xf9a2ef1a, 45, m_flag_set: 0x37d005e3);
        ParameterManagerLib.AddJoglessGroup(45, 0x5c3a290b);
    }

    private static void TestAddCommonSelectWindow()
    {
        // Add reverse exchange in Tyrannomon's material exchange, the wood exchange day one.
        ParameterManagerLib.AddLanguage(3, "DigiFirewood x 1 -> DigiBamboo x 5");
        ParameterManagerLib.AddCommonSelectWindow(
            commonSelectWindowModeIndex: ParameterCommonSelectWindowMode.WindowType.MaterialChange03,
            m_langid1: 3,
            m_select_mode1: ParameterCommonSelectWindow.SelectModeParam.TownMaterialNumCheck2,
            m_select_format1: ParameterCommonSelectWindow.ModeFormat.UP,
            m_select_value1: 1,
            m_select_item1: 0xdd47538b,
            m_scriptCommand: "TestAddCommonSelectWindow1");
        ParameterManagerLib.AddLanguage(4, "DigiFelwood x 1 -> DigiFirewood x 5");
        ParameterManagerLib.AddCommonSelectWindow(
            commonSelectWindowModeIndex: ParameterCommonSelectWindowMode.WindowType.MaterialChange03,
            m_langid1: 4,
            m_select_mode1: ParameterCommonSelectWindow.SelectModeParam.TownMaterialNumCheck2,
            m_select_format1: ParameterCommonSelectWindow.ModeFormat.UP,
            m_select_value1: 1,
            m_select_item1: 0xdd475384,
            m_scriptCommand: "TestAddCommonSelectWindow2");
        ParameterManagerLib.AddLanguage(5, "DigiStarwood x 1 -> DigiFelwood x 5");
        ParameterManagerLib.AddCommonSelectWindow(
            commonSelectWindowModeIndex: ParameterCommonSelectWindowMode.WindowType.MaterialChange03,
            m_langid1: 5,
            m_select_mode1: ParameterCommonSelectWindow.SelectModeParam.TownMaterialNumCheck2,
            m_select_format1: ParameterCommonSelectWindow.ModeFormat.UP,
            m_select_value1: 1,
            m_select_item1: 0xdd475385,
            m_scriptCommand: "TestAddCommonSelectWindow3");

        // Callbacks for the "m_scriptCommand" given to the new CommonSelectWindow.
        ParameterManagerLib.CustomScriptCommands["TestAddCommonSelectWindow1"] = () =>
        {
            // The game already have an easy way to give the player item (and remove as well by giving negative value).
            // Maybe add a function in the lib, for the purpose of better documenting it ?
            // (and also managing in case someone try to call those function before a save is loaded.)
            StorageData.m_ItemStorageData.AddItemPlayer(0xdd47538b, -1);
            StorageData.m_ItemStorageData.AddItemPlayer(0xdd47538a, 5);

            // Using the scenario script to request a talk using the original talk system.
            // See "TestAddPlacementNPC" for an example using the custom DialogManager.
            MainGameManager.Ref.scenarioScript._RequestStartTalkEvent(1, "EV_TOWN_D021", "TALK_003", "D021_TALK01");
            MainGameManager.Ref.scenarioScript.m_EventAfterCallScript = "SubScenario";
        };

        ParameterManagerLib.CustomScriptCommands["TestAddCommonSelectWindow2"] = () =>
        {
            StorageData.m_ItemStorageData.AddItemPlayer(0xdd475384, -1);
            StorageData.m_ItemStorageData.AddItemPlayer(0xdd47538b, 5);
            MainGameManager.Ref.scenarioScript._RequestStartTalkEvent(1, "EV_TOWN_D021", "TALK_003", "D021_TALK01");
            MainGameManager.Ref.scenarioScript.m_EventAfterCallScript = "SubScenario";
        };

        ParameterManagerLib.CustomScriptCommands["TestAddCommonSelectWindow3"] = () =>
        {
            StorageData.m_ItemStorageData.AddItemPlayer(0xdd475385, -1);
            StorageData.m_ItemStorageData.AddItemPlayer(0xdd475384, 5);
            MainGameManager.Ref.scenarioScript._RequestStartTalkEvent(1, "EV_TOWN_D021", "TALK_003", "D021_TALK01");
            MainGameManager.Ref.scenarioScript.m_EventAfterCallScript = "SubScenario";
        };

        // Change the position of the custom entry in the window list.
        // TODO: Is kinda scuffed to use, need a better implementation to manage sorting.
        Dictionary<int, int> sort = new Dictionary<int, int>
        {
            { 8, 5 },
            { 9, 7 },
            { 10, 9 },
        };

        ParameterManagerLib.CustomCommonSelectWindowSort.Add((int)ParameterCommonSelectWindowMode.WindowType.MaterialChange04, sort);
    }

    private static void TestAddPlacementEnemy()
    {
        // Add a drop item which drop 3 mandrakes with 100% of happening
        ParameterManagerLib.AddDropItem(144, 0x86d4e78c, 3, 100);

        // This test on a map with no csvb file existing for it. (Nigh Plains - Ruins Lake)
        ParameterManagerLib.AddPlacementEnemy(new System.Numerics.Vector2(2, 7), m_level: 98, m_itemDropNo: 144, m_startX: 103, m_startY: 22, m_startZ: 88);

        // This test on a map with a csvb file existing for it. (Nigh Plains - Vast Plateau)
        ParameterManagerLib.AddPlacementEnemy(new System.Numerics.Vector2(2, 1), m_unitNo: MainGameManager.UNITID.Enemy10, m_startX: 86, m_startY: 21, m_startZ: 225);
    }

    private static void TestAddPlacementNPC()
    {
        ParameterManagerLib.AddLanguage(10, "Yo DMC peps.");
        ParameterManagerLib.AddLanguage(11, "How are you guys doing?");
        ParameterManagerLib.AddLanguage(10, "DMCの皆さん、こんにちは。", SystemLanguage.Japanese);
        ParameterManagerLib.AddLanguage(11, "今日の調子はどうだい？", SystemLanguage.Japanese);

        // Callback given to the "m_CmdBlock" param, that will be executed when talking to the npc.
        ParameterManagerLib.CustomScriptCommands["LIB_DEBUG_TALKNPC"] = () =>
        {
            // Example of a callback using commands that need waiting. The DialogManager will wait for
            // all it's commands to be finished before proceeding.
            // Adding ".Wait()" make sure the action is finished before continuing.
            Action action = () =>
            {
                Commands.RotatePlayerAndNPC(MainGameManager.UNITID.Npc04).Wait();
            };

            // title/message doesn't require to use the game Language, you can give it text directly.
            // However it is adviced to use them to allow different language support.
            // Language.GetString("c001") - is getting a traduction using a string, which is then converted to hash.
            // the string "c001" make the hash to get the text for agumon.
            // Language.GetString(11) - is getting a traducting using an id, in this case, we get the text we just created
            // in the function.
            List<Dialog> dialogs = new List<Dialog>()
            {
                new Dialog() { title = Language.GetString("c001"), message = Language.GetString(10), callback = action },
                new Dialog() { title = Language.GetString("c001"), message = Language.GetString(11) },
            };

            // Don't forget this function to actually start the dialog.
            StartDialog(dialogs);
        };

        ParameterManagerLib.CustomScriptCommands["LIB_DEBUG_TALKNPC2"] = () =>
        {
            MainGameManager.Ref.RequestMapChange(99, 1);
        };

        ParameterManagerLib.AddLanguage(12, "hi");
        ParameterManagerLib.AddLanguage(13, "Puppetmon took a vacation :(");

        ParameterManagerLib.CustomScriptCommands["LIB_DEBUG_TALKNPCENEMY"] = () =>
        {
            Action battle_end = null;
            Action end_action = null;

            Action action = () =>
            {
                Commands.FadeOut(Commands.DefaultFadeOutColor).Wait();
                MainGameManager.GetNpc(0).SetDisp(true);
                Commands.MoveGameCamera(MainGameManager.UNITID.Npc00, new Vector3(0f, 1f, 10f), new Vector3(0f, 180f, 0f), 0).Wait();
                Commands.FadeIn().Wait();
            };

            battle_end = () =>
            {
                List<Dialog> dialogs = new List<Dialog>()
                {
                    new Dialog() { title = Language.GetString("f029"), message = ">:(", callback = action },
                    new Dialog() { title = Language.GetString("f029"), message = "T-T" },
                };

                StartDialog(dialogs, () => { MainGameManager.GetNpc(0).Destroy(); }, true);
            };

            end_action = () =>
            {
                uint[] array = [5, 6, 7];
                Commands.StartNPCBattle(array, battle_end);
            };

            List<Dialog> dialogs = new List<Dialog>()
            {
                new Dialog() { title = Language.GetString("f029"), message = Language.GetString(12)},
                new Dialog() { title = Language.GetString("f029"), message = Language.GetString(13)},
            };

            StartDialog(dialogs, end_action, false);
        };

        // Add an npc in Nigh Plains - Ruins Lake. (Random talk)
        ParameterManagerLib.AddPlacementNPC(new System.Numerics.Vector2(2, 7), m_UintId: MainGameManager.UNITID.Npc04, m_CmdBlock: "LIB_DEBUG_TALKNPC", m_Px: 112, m_Py: 22.3f, m_Pz: 70, m_Ry: 318);

        // npc in Nigh Plains - Ruins Lake that teleport the player to the tutorial area.
        ParameterManagerLib.AddPlacementNPC(new System.Numerics.Vector2(2, 7), m_MdlName: "e021", m_Name: 1, m_UintId: MainGameManager.UNITID.Npc05, m_CmdBlock: "LIB_DEBUG_TALKNPC2", m_Px: 133f, m_Py: 22.3f, m_Pz: 92, m_Ry: 285);

        // Enemy npc example
        // 1st is for the actual npc.
        // 2nd is for the enemy npc (is invisible). It must have an m_UintId of Enemy15 to Enemy19. Must have a m_NpcEnemyParamId pointing to the date of that id.
        // 3rd is the data of that npc.
        ParameterManagerLib.AddPlacementNPC(new System.Numerics.Vector2(99, 1), m_MdlName: "f029", m_Name: 1, m_UintId: MainGameManager.UNITID.Npc00, m_CmdBlock: "LIB_DEBUG_TALKNPCENEMY", m_Px: -0.6714f, m_Py: -0.2324f, m_Pz: 34.6682f, m_Ry: 180);
        ParameterManagerLib.AddPlacementNPC(new System.Numerics.Vector2(99, 1), m_MdlName: "f029", m_Name: 5, m_UintId: MainGameManager.UNITID.Enemy15, m_ChkInput: 0, m_CmdBlock: "", m_Px: -0.6714f, m_Py: -0.2324f, m_Pz: 34.6682f, m_Ry: 180, m_NpcEnemyParamId: 10);
        ParameterManagerLib.AddNPCEnemy(new System.Numerics.Vector2(99, 1), m_ParamId: 10, m_Level: 1, m_BattleBgm: 4);

        // multiple npc battle
        ParameterManagerLib.AddPlacementNPC(new System.Numerics.Vector2(99, 1), m_MdlName: "f025", m_Name: 6, m_UintId: MainGameManager.UNITID.Enemy16, m_ChkInput: 0, m_CmdBlock: "", m_Px: -0.6714f, m_Py: -0.2324f, m_Pz: 34.6682f, m_Ry: 180, m_NpcEnemyParamId: 11);
        ParameterManagerLib.AddNPCEnemy(new System.Numerics.Vector2(99, 1), m_ParamId: 11, m_Level: 1, m_BattleBgm: 4);

        ParameterManagerLib.AddPlacementNPC(new System.Numerics.Vector2(99, 1), m_MdlName: "f039", m_Name: 7, m_UintId: MainGameManager.UNITID.Enemy17, m_ChkInput: 0, m_CmdBlock: "", m_Px: -0.6714f, m_Py: -0.2324f, m_Pz: 34.6682f, m_Ry: 180, m_NpcEnemyParamId: 12);
        ParameterManagerLib.AddNPCEnemy(new System.Numerics.Vector2(99, 1), m_ParamId: 12, m_Level: 1, m_BattleBgm: 4);
    }

    private static void TestAddTamerExpTable()
    {
        // Meant to be tested with a save already at level 50.
        // instantly level up to 54 on loading save file. Missing levels are automatically filled in with empty requirement.
        ParameterManagerLib.AddTamerExpTable(55, 100, 100);
    }
}
