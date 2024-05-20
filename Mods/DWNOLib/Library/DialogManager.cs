using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using System.Collections;

namespace DWNOLib.Library;
public class DialogManager
{
    public class Dialog
    {
        public string title = "";
        public string message = "";
        public Action callback = null;
        public bool one_shot_callback = true;
    }

    private static GameObject DialogObject { get; set; } = null;

    private static GameObject TitleWindow { get; set; } = null;

    private static GameObject MessageWindow { get; set; } = null;

    private static Text TitleText { get; set; } = null;

    private static Text MessageText { get; set; } = null;

    private static Canvas Prompt { get; set; } = null;

    private static List<Dialog> DialogBuffer { get; set; } = new List<Dialog>();

    private static Action EndCallback { get; set; } = null;

    private static bool EndRestoreUI { get; set; } = false;

    public static bool CallbackEnded { get; set; } = true;

    private static bool KeyStillDown { get; set; } = false;

    /// <summary>
    /// Start a new dialog using the custom DialogManager. Look at <c>UnitTests</c> for usage example.
    /// </summary>
    public static void StartDialog(List<Dialog> dialogs, Action endCallback = null, bool endRestoreUI = true)
    {
        if (MainGameManager.Ref.eventScene == null)
            return;
        else
            if (DialogObject == null)
                SetupDialog();

        DialogBuffer = dialogs;
        PrepareUI();

        EndCallback = endCallback;
        EndRestoreUI = endRestoreUI;

        KeyStillDown = CheckPressedKey();

        AppMainScript.Ref.StartCoroutineManaged2(DialogUpdate().WrapToIl2Cpp());
    }

    private static IEnumerator DialogUpdate()
    {
        int UpdateStep = 0;
        int DialogIndex = 0;

        while (true)
        {
            switch (UpdateStep)
            {
                case 0:
                    if (DialogBuffer[DialogIndex].callback != null)
                    {
                        if (DialogBuffer[DialogIndex].one_shot_callback == false)
                            CallbackEnded = false;
                        DialogBuffer[DialogIndex].callback.Invoke();
                    }
                    UpdateStep++;
                    break;
                case 1:
                    if (CallbackEnded)
                    {
                        DialogObject.SetActive(true);
                        UpdateStep++;
                    }
                    else
                    {
                        if (CheckPressedKey())
                            KeyStillDown = true;
                    }
                    break;
                case 2:
                    TitleText.text = DialogBuffer[DialogIndex].title;
                    if (TitleText.text == "")
                        TitleWindow.SetActive(false);
                    else
                        TitleWindow.SetActive(true);
                    MessageText.text = DialogBuffer[DialogIndex].message;
                    if (CallbackEnded)
                    {
                        Prompt.gameObject.SetActive(true);
                        
                        if (CheckPressedKey())
                        {
                            KeyStillDown = true;
                            DialogIndex++;
                            if (DialogIndex >= DialogBuffer.Count)
                            {
                                UpdateStep++;
                                break;
                            }
                            if (DialogBuffer[DialogIndex].callback != null)
                            {
                                if (DialogBuffer[DialogIndex].one_shot_callback == false)
                                    CallbackEnded = false;
                                DialogBuffer[DialogIndex].callback.Invoke();
                            }
                        }
                    }
                    else
                    {
                        CheckPressedKey();
                        Prompt.gameObject.SetActive(false);
                    }
                    break;
                case 3:
                    if (EndRestoreUI)
                        RestoreUI();
                    DialogObject.SetActive(false);
                    UpdateStep++;
                    break;
            }
            if (UpdateStep == 4)
                break;
            yield return null;
        }
        KeyStillDown = false;
        EndCallback?.Invoke();
        EndCallback = null;
        EndRestoreUI = true;
        yield break;
    }

    private static bool CheckPressedKey()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            KeyStillDown = false;

        if (Input.GetKeyUp(KeyCode.Return))
            KeyStillDown = false;

        if (Input.GetKeyUp(KeyCode.KeypadEnter))
            KeyStillDown = false;

        if (Input.GetKeyUp(KeyCode.Mouse0))
            KeyStillDown = false;

        if (PadManager.IsRelease(PadManager.BUTTON.bCross))
            KeyStillDown = false;

        if (PadManager.IsRelease(PadManager.BUTTON.bCircle))
            KeyStillDown = false;

        if (KeyStillDown)
            return false;

        if(Input.GetKeyDown(KeyCode.Space))
            return true;

        if (Input.GetKeyDown(KeyCode.Return))
            return true;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            return true;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            return true;

        if (PadManager.IsInput(PadManager.BUTTON.bCross) && !PadManager.IsRepeat(PadManager.BUTTON.bCross))
            return true;

        if (PadManager.IsInput(PadManager.BUTTON.bCircle) && !PadManager.IsRepeat(PadManager.BUTTON.bCircle))
            return true;

        return false;
    }

    private static void SetupDialog()
    {
        // Copy the original dialog gameobject so we can use it without the TalkMain script of the game giving issues.
        DialogObject = UnityEngine.Object.Instantiate(MainGameManager.Ref.eventScene.transform.FindChild("Event_Field_UI").gameObject);
        DialogObject.transform.SetParent(DWNOLib.Canvas.transform);
        DialogObject.transform.Find("Base/Window").gameObject.SetActive(true);
        TitleWindow = DialogObject.transform.Find("Base/Window/Anim/Name_Window").gameObject;
        MessageWindow = DialogObject.transform.Find("Base/Window/Anim/Main_Window").gameObject;
        TitleText = DialogObject.transform.Find("Base/Window/Anim/Text/Name").transform.GetComponent<Text>();
        MessageText = DialogObject.transform.Find("Base/Window/Anim/Text/Normal").transform.GetComponent<Text>();
        MessageText.alignment = TextAnchor.MiddleCenter;
        Prompt = DialogObject.transform.Find("Base/Window/Anim/Next_Button").transform.GetComponent<Canvas>();
        DialogObject.transform.Find("Base/Window/Anim/Cursor").gameObject.SetActive(false);
        DialogObject.SetActive(false);

        // In case the original TalkMain was activated, as once closed they manually hide UI components.
        foreach (Image image in DialogObject.GetComponentsInChildren<Image>(true))
            image.enabled = true;
        foreach (Animator animator in DialogObject.GetComponentsInChildren<Animator>(true))
            animator.enabled = true;
        foreach (Text text in DialogObject.GetComponentsInChildren<Text>(true))
            text.enabled = true;
    }

    private static void PrepareUI()
    {
        AppInfo.Ref.gameProgress = AppInfo.GAMEPROG.Event;
        CameraManager.Ref.SetCameraQuake(0f, 0f, Vector3.zero);
        MainGameManager.Ref.enableFieldUI(false);
        MainGameComponent.Ref.m_StepProc[0]._ReqAction(MainGameManager.GetPlayer(), UnitCtrlBase.ReqAction.Event);
        MainGameComponent.Ref.m_StepProc[0]._ReqAction(MainGameManager.GetPartner(0), UnitCtrlBase.ReqAction.Event);
        MainGameComponent.Ref.m_StepProc[0]._ReqAction(MainGameManager.GetPartner(1), UnitCtrlBase.ReqAction.Event);

        MainGameComponent.Ref.m_StepProc[0]._ChangeMain(MainGame.STEP.Event);

        MainGameManager.GetPlayerCtrl().ResetFootPrint();
        MainGameManager.GetPartnerCtrl(0).ResetFootPrint();
        MainGameManager.GetPartnerCtrl(1).ResetFootPrint();

        for (int i = 0; i < 15; i++)
        {
            EnemyCtrl enemyCtrl = MainGameManager.GetEnemyCtrl(i);
            if (!(enemyCtrl == null) && enemyCtrl.gameData != null && enemyCtrl.isActiveAndEnabled && enemyCtrl.gameData.m_commonData.m_hp > 0)
            {
                enemyCtrl.SetDisp(false);
                enemyCtrl.gameObject.SetActive(false);
            }
        }

        for (int j = 0; j < 16; j++)
        {
            NpcCtrl npc = MainGameManager.GetNpc(j);
            if (!(npc == null) && npc.isActiveAndEnabled)
            {
                MainGameComponent.Ref.m_StepProc[0]._ReqAction(npc, UnitCtrlBase.ReqAction.Event);
            }
        }
    }

    private static void RestoreUI()
    {
        AppInfo.Ref.gameProgress = AppInfo.GAMEPROG.Field;
        MainGameManager.Ref.enableFieldUI(true);
        MainGameComponent.Ref.m_StepProc[0]._ReqAction(MainGameManager.GetPlayer(), UnitCtrlBase.ReqAction.Field);
        MainGameComponent.Ref.m_StepProc[0]._ReqAction(MainGameManager.GetPartner(0), UnitCtrlBase.ReqAction.Field);
        MainGameComponent.Ref.m_StepProc[0]._ReqAction(MainGameManager.GetPartner(1), UnitCtrlBase.ReqAction.Field);

        MainGameComponent.Ref.m_StepProc[0]._ChangeMain(MainGame.STEP.Field);

        for (int i = 0; i < 15; i++)
        {
            EnemyCtrl enemyCtrl = MainGameManager.GetEnemyCtrl(i);
            if (!(enemyCtrl == null) && enemyCtrl.gameData != null && enemyCtrl.gameData.m_commonData.m_hp > 0)
            {
                enemyCtrl.SetDisp(true);
                enemyCtrl.gameObject.SetActive(true);
            }
        }
    }

    private static void TestDialog()
    {
        Action action = async () =>
        {
            for (int i = 0; i < 120; i++)
            {
                MainGameManager.GetPlayer().transform.GetChild(0).Rotate(new Vector3(720f / 60, 0f, 0f));
                await Task.Delay(TimeSpan.FromSeconds(1f / 60));
            }
            CallbackEnded = true;
        };

        List<Dialog> dialogs = new List<Dialog>()
        {
            new Dialog() { title = "Poyo", message = "Hello world!"},
            new Dialog() { title = "Poyo's mind", message = "Ho nice this dialog is working!\nThat's very cool!"},
            new Dialog() { title = "Poyo's mind", message = "Time to SPIN", callback = action, one_shot_callback = false},
            new Dialog() { title = "Poyo", message = "HEEEEEELP"},
        };

        StartDialog(dialogs);
    }
}
