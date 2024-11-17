using System.Collections.Generic;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace AlternativeBattleCamera;

[BepInPlugin(GUID, PluginName, PluginVersion)]
[BepInProcess("Digimon World Next Order.exe")]
public class Plugin : BasePlugin
{
    internal const string GUID = "poyoyuoh.DWNO.AlternativeBattleCamera";
    internal const string PluginName = "AlternativeBattleCamera";
    internal const string PluginVersion = "1.0.0";

    public static float VerticalSpeed = 8f;
    public static float HorizontalSpeed = 4f;
    public static float ZoomSpeed = 4f;
    public static float ZoomMinDistance = 1f;
    public static float ZoomMaxDistance = 15f;

    public static float Height = 4f;
    public static float Distance = 0f;
    public static float Radius = 6f;

    private static Vector3 original_target_pos;

    public override void Load() => Harmony.CreateAndPatchAll(typeof(Plugin));

    [HarmonyPatch(typeof(CameraScriptBattle), "BattleCameraUsually")]
    [HarmonyPrefix]
    public static bool CameraScriptBattle_BattleCameraUsually_Prefix(CameraScriptBattle __instance, ref bool __result)
    {
        if (PadManager.IsInput(PadManager.BUTTON.slUp))
            Radius -= ZoomSpeed * Time.deltaTime;
        if (PadManager.IsInput(PadManager.BUTTON.slDown))
            Radius += ZoomSpeed * Time.deltaTime;

        Radius = Mathf.Clamp(Radius, 1f, 15f);

        if (PadManager.IsInput(PadManager.BUTTON.srUp))
            Height -= VerticalSpeed * Time.deltaTime;
        if (PadManager.IsInput(PadManager.BUTTON.srDown))
            Height += VerticalSpeed * Time.deltaTime;

        if (PadManager.IsInput(PadManager.BUTTON.srLeft))
            Distance -= Time.deltaTime;
        if (PadManager.IsInput(PadManager.BUTTON.srRight))
            Distance += Time.deltaTime;

        Vector2 pos = new Vector2(Mathf.Sin(Distance * HorizontalSpeed) * Radius, Mathf.Cos(Distance * HorizontalSpeed) * Radius);

        RelativeCameraCtrl relativeCamera = CameraManager.Ref.relativeCamera;
        relativeCamera.m_blendRate = 0.5f;

        if (StorageData.m_playerData.m_battleCameraType == 1)
        {
            List<Vector3> values = new List<Vector3>();

            UnitCtrlBase digimonCtrlR = MainGameManager.GetUnitCtrl(MainGameManager.UNITID.Partner00);
            UnitCtrlBase digimonCtrlL = MainGameManager.GetUnitCtrl(MainGameManager.UNITID.Partner01);
            if (digimonCtrlR != null && digimonCtrlR.actionState != UnitCtrlBase.ActionState.ActionState_Dead)
            {
                values.Add(digimonCtrlR.transform.position);
            }
            // Check for OnePartner support
            if (digimonCtrlR != digimonCtrlL)
            {
                if (digimonCtrlL != null && digimonCtrlL.actionState != UnitCtrlBase.ActionState.ActionState_Dead)
                {
                    values.Add(digimonCtrlL.transform.localPosition);
                }
            }

            for (int i = 0; i < AppInfo.Ref.battleInfo.enemyList.Count; i++)
            {
                EnemyCtrl enemyCtrl = AppInfo.Ref.battleInfo.enemyList[i];
                if (enemyCtrl.actionState != UnitCtrlBase.ActionState.ActionState_Dead)
                {
                    values.Add(enemyCtrl.transform.localPosition);
                }
            }

            Vector2 center = Vector2.zero;
            foreach (Vector3 value in values)
            {
                center.x += value.x;
                center.y += value.z;
            }
            center.x = center.x / values.Count;
            center.y = center.y / values.Count;
            __instance.m_usually_target_transform.localPosition = new Vector3(center.x, __instance.m_usually_target_transform.localPosition.y, center.y);
            
            relativeCamera.SetViewposTarget(RelativeCameraCtrl.TRACE.RotatePosition, __instance.m_usually_target_transform, Vector3.zero, Vector3.one);
            relativeCamera.SetLookatTarget(RelativeCameraCtrl.TRACE.RotatePosition, __instance.m_usually_target_transform, Vector3.zero, Vector3.one);
        }  
        else
        {
            __instance.m_usually_target_transform = __instance.m_usually_target.transform;
            __instance.m_usually_target_transform.position = CameraManager.Ref.objBattleFieldCenterScr.transform.position;
            relativeCamera.SetViewpos(Vector3.zero, new Vector3(0f, 0f, 0f), Vector3.zero, Vector3.zero);
            relativeCamera.SetLookat(Vector3.zero, new Vector3(0f, 0f, 0f), Vector3.zero, Vector3.zero);
        }
        
        relativeCamera.SetPositionViewpos(new Vector3(pos.x, Height, pos.y));

        if (uBattlePanel.Ref.IsBattleStartActive())
        {
            __result = true;
            return false;
        }
        __result = true;
        return false;
    }
}
