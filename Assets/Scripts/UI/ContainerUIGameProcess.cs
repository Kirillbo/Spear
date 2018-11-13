using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerUIGameProcess : MonoBehaviour
{

    public TMP_Text FieldTextAmountStick;
    public TMP_Text FieldTextScore;
    public Image[] ImageHP;

    public GameObject BlackBackground;
    public Canvas CanvasPause;

    public void Start()
    {
        EventManager.StartListening(BttnType.GameProcess.ToString(), ButtonMenu);
    }

    void ButtonMenu(GameObject obj, string parametr)
    {
        var state = GameManager.Instance.FindState("GameState") as GameProcess;

        switch (parametr)
        {
            case "Pause":
                if (state != null) state.SetEnableShotStick(false);

                Time.timeScale = 0;
                BlackBackground.SetActive(true);
                CanvasPause.enabled = true;
                break;

            case "Resume":
                if (state != null) state.SetEnableShotStick(true);

                BlackBackground.SetActive(false);
                CanvasPause.enabled = false;
                Time.timeScale = 1;
                break;

            case "Exit":
                CanvasPause.enabled = false;
                BlackBackground.SetActive(false);
                GameManager.Instance.GoToStartMenu();
                break;

            case "Restart":
                BlackBackground.SetActive(false);
                CanvasPause.enabled = false;
                GameManager.Instance.GameOver = true;
                GameManager.Instance.Restart();
                break;

            case "GODMOD":
                Data.Instance.GODMODE = !Data.Instance.GODMODE;
                break;
        }
    }

}
