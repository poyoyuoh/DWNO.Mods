using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace ModelSwap;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
[BepInDependency("poyoyuoh.DWNO.InstantFishing")]
[BepInDependency("FastPickup", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("InstantCareMenu", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.ModelSwap";
    internal const string PluginName = "ModelSwap";
    internal const string PluginVersion = "1.1.0";

    private static ConfigEntry<string> ModelID;
    private static ConfigEntry<string> AnimationReplacement;
    private static ConfigEntry<bool> RemoveMireiHologramEffect;

    private static int LoadPlayerStep = 0;
    private static bool is_voice = false;
    private static bool is_miracle = false;

    public override void Load()
    {
        bool FastPickup_found = false;
        bool InstantCareMenu_found = false;
        foreach (var plugin in IL2CPPChainloader.Instance.Plugins)
        {
            var metadata = plugin.Value.Metadata;
            if (metadata.GUID.Equals("FastPickup"))
                FastPickup_found = true;
            else if (metadata.GUID.Equals("InstantCareMenu"))
                InstantCareMenu_found = true;
        }
        if (!FastPickup_found)
            Log.LogWarning("Plugin \"FastPickup\" not found. It is not necessary for the mod to work, but it is highly recommended.");
        if (!InstantCareMenu_found)
            Log.LogWarning("Plugin \"InstantCareMenu\" not found. It is not necessary for the mod to work, but it is highly recommended.");

        ModelID = Config.Bind("#General", "ModelID", "z008", "The model to change to. Use https://docs.google.com/spreadsheets/d/11zy6lzOl_4WR679IB9OflsXjRHZh8JvRFx8M6-nrWAI/edit#gid=2105054197\nfor a list of all the model ID.");
        AnimationReplacement = Config.Bind("#General", "AnimationReplacement", "null", "Some models, such as Mirei, doesn't have proper animation.\nChange this value to a model ID to use that model animation instead.\nMay not work, the model may still T-pose.");
        RemoveMireiHologramEffect = Config.Bind("#General", "RemoveMireiHologramEffect", true, "Self explanatory. Remove the hologram effect/shader from Mirei's model.");
        Harmony.CreateAndPatchAll(typeof(Plugin));
    }

    [HarmonyPatch(typeof(uTitlePanel), "Awake")]
    [HarmonyPrefix]
    public static void uTitlePanel_Awake_Prefix()
    {
        string bundle_name = string.Format("ui/icons/ch_{0}", StorageData.m_playerData.GetPlayerModelName());
        if (AssetBundleManager.GetAssetData(bundle_name) == null)
            AssetBundleManager.StartDownLoad(bundle_name, null);
    }

    [HarmonyPatch(typeof(uSavePanelItemSaveItem), "Load")]
    [HarmonyPostfix]
    public static void uSavePanelItemSaveItem_Load_Postfix(uSavePanelItemSaveItem __instance)
    {
        string iconName = string.Format("ui/icons/ch_{0}", StorageData.m_playerData.GetPlayerModelName());
        Texture2D asset = AssetBundleManager.GetAsset<Texture2D>(iconName);

        if ((bool)asset)
        {
            Sprite sprite = Sprite.Create(asset, new Rect(0f, 0f, asset.width, asset.height), new Vector2(0.5f, 0.5f));
            __instance.m_playerIcon.sprite = sprite;
            __instance.m_playerIcon.rectTransform.sizeDelta = new Vector2(8, 8);
        }
    }

    [HarmonyPatch(typeof(uDigiviceTopPanelKizuna), "Initialize")]
    [HarmonyPostfix]
    public static void uDigiviceTopPanelKizuna_Initialize_Postfix()
    {
        string bundle_name = string.Format("ui/icons/ch_{0}", StorageData.m_playerData.GetPlayerModelName());
        if (AssetBundleManager.GetAssetData(bundle_name) == null)
            AssetBundleManager.StartDownLoad(bundle_name, null);
    }

    [HarmonyPatch(typeof(uDigiviceTopPanelKizuna), "enablePanel")]
    [HarmonyPrefix]
    public static void uDigiviceTopPanelKizuna_enablePanel_Postfix(bool enable, uDigiviceTopPanelKizuna __instance)
    {
        if (enable)
        {
            string iconName = string.Format("ui/icons/ch_{0}", StorageData.m_playerData.GetPlayerModelName());
            Texture2D asset = AssetBundleManager.GetAsset<Texture2D>(iconName);

            if ((bool)asset)
            {
                Sprite sprite = Sprite.Create(asset, new Rect(0f, 0f, asset.width, asset.height), new Vector2(0.5f, 0.5f));
                __instance.m_TamerSprite[0] = sprite;
                __instance.m_TamerSprite[1] = sprite;
                __instance.m_Icons[0].rectTransform.sizeDelta = new Vector2(116, 116);
            }
        }
    }

    [HarmonyPatch(typeof(MainGameManager), "_LoadPlayer")]
    [HarmonyPrefix]
    public static void MainGameManager__LoadPlayer_Prefix() => LoadPlayerStep = 1;

    [HarmonyPatch(typeof(MainGameManager._LoadTask_d__3), "MoveNext")]
    [HarmonyPostfix]
    public static void MainGameManager__LoadTask_d__3_MoveNext_Postfix(MainGameManager._LoadTask_d__3 __instance)
    {
        switch (__instance.__1__state)
        {
            case -1:
                // Force radius to be the same as the normal player model, otherwise big model will not be able to talk with NPCs.
                MainGameManager.GetPlayerCtrl().m_move.currentAgentRadius = 0.5f;

                if (ModelID.Value == "z010" && RemoveMireiHologramEffect.Value)
                {
                    var mesh = MainGameManager.GetPlayer().transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
                    var materials = mesh.GetComponent<UnityEngine.SkinnedMeshRenderer>().materials;
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i].SetColor("_ScanLine_Color", new Color(0, 0, 0, 0));
                        materials[i].SetColor("_Noise_Color", new Color(0, 0, 0, 0));
                    }
                }
                break;
        }
    }

    [HarmonyPatch(typeof(PlayerData), "GetPlayerModelName")]
    [HarmonyPrefix]
    public static bool PlayerData_GetPlayerModelName_Prefix(ref string __result)
    {
        if (is_voice)
            return true;

        switch (LoadPlayerStep)
        {
            case 1:
                // First GetPlayerModelName call inside the _LoadPlayer function.
                // Call to get the model.
                LoadPlayerStep += 1;
                break;
            case 2:
                // Second GetPlayerModelName call inside the _LoadPlayer function.
                // Call to get the animation.
                if (AnimationReplacement.Value != "null")
                    __result = AnimationReplacement.Value;
                else
                    __result = ModelID.Value;
                LoadPlayerStep = 0;
                return false;
        }

        __result = ModelID.Value;
        return false;
    }

    [HarmonyPatch(typeof(uFieldItemPanel), "UseItems", [typeof(MainGameManager.ORDER_UNIT), typeof(GameObject), typeof(bool)], [ArgumentType.Ref, ArgumentType.Normal, ArgumentType.Normal])]
    [HarmonyPrefix]
    public static void uFieldItemPanel_UseItems_Prefix(uFieldItemPanel __instance)
    {
        if (__instance.m_playerCtrl.itemJoint == null)
            __instance.m_playerCtrl.m_itemJoint = __instance.m_playerCtrl.transform;
    }

    [HarmonyPatch(typeof(uBattlePanelItemBox), "UseItem")]
    [HarmonyPrefix]
    public static void uBattlePanelItemBox_UseItem_Prefix(uBattlePanelItemBox __instance)
    {
        if (__instance.m_player.itemJoint == null)
            __instance.m_player.m_itemJoint = __instance.m_player.transform;
    }

    [HarmonyPatch(typeof(Item), "HaveToHand")]
    [HarmonyPostfix]
    public static void Item_HaveToHand_Postfix(Item __instance) => __instance.ThrowItem();

    [HarmonyPatch(typeof(MainGameBattle), "_Lose")]
    [HarmonyPrefix]
    public static bool MainGameBattle__Lose_Prefix(MainGameBattle __instance)
    {
        // Game will try to load voice sound by getting the player model.
        if (__instance.m_Step.m_SubStep == 0)
            is_voice = true;
        else
            is_voice = false;

        return true;
    }

    [HarmonyPatch(typeof(MainGameBattle), "_Miracle")]
    [HarmonyPostfix]
    public static void MainGameBattle__Miracle_Postfix(MainGameBattle __instance)
    {
        string PlayerModelName = StorageData.m_playerData.GetPlayerModelName();

        if (PlayerModelName == "z003" || PlayerModelName == "z004")
            return;
        else
        {
            if (__instance.m_Step.m_SubStep == 0)
                __instance.m_Step.m_SubStep = 1;

            if (__instance.m_Step.m_SubStep == 8)
                __instance.m_Step.m_SubStep = 100;
        }
    }

    [HarmonyPatch(typeof(MainGameBattle), "_Lose")]
    [HarmonyPostfix]
    public static void MainGameBattle__Lose_Postfix(MainGameBattle __instance)
    {
        if (__instance.m_Step.m_SubStep == 4)
        {
            string PlayerModelName = StorageData.m_playerData.GetPlayerModelName();

            if (PlayerModelName == "z003" || PlayerModelName == "z004")
                return;
            else
            {
                if (__instance.m_Step.m_SubFrame >= 40)
                {
                    if (!is_miracle)
                    {
                        if (__instance.m_lastPartner != null && __instance.m_lastPartner.GetComponent<PartnerCtrl>().IsPossibleMiracle())
                        {
                            is_miracle = true;
                            __instance.m_Step.step = MainGameBattle.STEP.Miracle;
                            return;
                        }
                    }

                    __instance._UiAreaOff();
                    if (is_miracle)
                        MainGameManager.GetPartnerCtrl(2).ReleaseMiracle();

                    __instance.m_Step.m_SubStep = 301;
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerCtrl), "SetChangeBattleLoseMode")]
    [HarmonyPrefix]
    public static bool PlayerCtrl_SetChangeBattleLoseMode_Prefix(GameObject target, PlayerCtrl __instance)
    {
        string PlayerModelName = StorageData.m_playerData.GetPlayerModelName();

        if (PlayerModelName == "z003" || PlayerModelName == "z004")
            return true;
        
        __instance.m_move.ResetPlayerMove();
        __instance.m_move.SetAccelerationRate(1f);
        __instance.m_move.SetDestinationAI(target.transform.position - (target.transform.position - __instance.m_Transform.position).normalized * (target.GetComponent<UnitCtrlBase>().unitRadius + __instance.unitRadius * 2f));
        __instance.m_isBattle = false;
        return false;
    }

    [HarmonyPatch(typeof(MapTriggerScript), "Start")]
    [HarmonyPostfix]
    public static void MapTriggerScript_Start_Postfix(MapTriggerScript __instance)
    {
        __instance.areaSize = new Vector3(__instance.areaSize.x, 10, __instance.areaSize.z);
    }
}
