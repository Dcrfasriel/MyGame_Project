using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{

    public GameObject ContinueButton;

    public GameObject ReturnButton;

    public GameObject ExitButton;

    public BlackMaskController blackMaskController;

    public GameUIManager gameUIManager;
    private void OnEnable()
    {
        gameUIManager.SetControlMute(true);
        Invoke("SetMuteOff", 0.2f);
        EventSystem.current.SetSelectedGameObject(null);
    }
    private void SetMuteOff()
    {
        gameUIManager.SetControlMute(false);
    }
    public void FirstSelect()
    {
        EventSystem.current.SetSelectedGameObject(ContinueButton);
    }

    public void OnContinueButClick()
    {
        gameUIManager.OnButtonClick();
        Invoke("CloseMenu", 0.3f);
    }
    private void CloseMenu()
    {
        gameUIManager.TabMenu();
    }

    public void OnReturnButClick()
    {
        gameUIManager.OnButtonClick();
        blackMaskController.SetAppear(true);
        blackMaskController.SetTemporaryEvent(LoadMainMenu, true);
    }
    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnExitButClick()
    {
        gameUIManager.OnButtonClick();
        Invoke("Exit", 1f);
    }

    public void Exit()
    {
        Application.Quit();
    }


}
