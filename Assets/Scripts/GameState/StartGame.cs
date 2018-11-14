using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : AState
{

    public Canvas CanvasGameStart;
    public GameObject SettingsPanel;
    public CanvasGroup MainMenu;
    public GameObject BlackPanel;

    public Image OFFMusic;
    public Image OFFSound;

    void Start()
    {
        EventManager.StartListening(BttnType.MainMenu.ToString(), MenuButtons);
    }

    public override void Init()
    {
        OFFSound.enabled = !Data.Instance.Sound;
        OFFMusic.enabled = !Data.Instance.Music;
    }

    private void MenuButtons(GameObject arg0, string param)
    {
        switch (param)
        {
            case "StartGame":
               GameManager.Instance.ChangeState("GameState", 1f);
                break;

            case "Settings":
                MainMenu.blocksRaycasts = false;
                BlackPanel.SetActive(true);
                SettingsPanel.SetActive(true);
                Debug.Log("showSettings");
                break;

            case "RemoveAds":
                Debug.Log("Remove Ads");
                break;

            case "Sound":
                Data.Instance.Sound = !Data.Instance.Sound;
                OFFSound.enabled = !Data.Instance.Sound;
                break;

            case "Music":
                Data.Instance.Music = !Data.Instance.Music;
                OFFMusic.enabled = !Data.Instance.Music;
                break;

            case "ReturnMenu":
                MainMenu.blocksRaycasts = true;
                BlackPanel.SetActive(false);
                SettingsPanel.SetActive(false);
                break;
        }        
    }

    public override void Enter()
    {

        CanvasGameStart.enabled = true;
        Manager.NewGame = true;
        Manager.GameOver = false;
        ManagerView.Get<BlackAnimationBack>().Curtain(Anim.Open);

    }

    public override void Exit()
    {
        Timer.Add(0.5f, () => CanvasGameStart.enabled = false);
        ManagerView.Get<BlackAnimationBack>().Curtain(Anim.Close);
    }


    public override void Tick()
    {

    }

    public override string GetName()
    {
        return "StartGame";
    }

}
