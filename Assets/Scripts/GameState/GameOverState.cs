using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverState : AState
{

    public Canvas MainCanvas;
    public TMP_Text FieldScore;
    public TMP_Text FieldBestScore;

    void Start()
    {
        EventManager.StartListening(BttnType.GameOver.ToString(), Buttons);
    }

    private void Buttons(GameObject arg0, string param)
    {
        switch (param)
        {
            case "returnToMainMenu":
                Manager.GoToStartMenu();
                break;

            case "Restart":
                Manager.Restart();
                break;
        }
    }

    public override void Init()
    {
        
    }

    public override void Enter()
    {
        
        ManagerSound.Instance.PlayEffect(Track.GameOver, Channel.Two);
        
        Data.Instance.BestScore = Manager.Session.Score;

        MainCanvas.enabled = true;
        FieldScore.text = Manager.Session.Score.ToString();
        FieldBestScore.text = Data.Instance.BestScore.ToString();

        Data.Instance.Save();
    }

    public override void Exit()
    {
        MainCanvas.enabled = false;

    }

    //IEnumerator IeExit()
    //{
      //  yield return ManagerView.Get<BlackAnimationBack>().ChangeRadial(Anim.Close);
    //}

    public override void Tick()
    {
        
    }

    public override string GetName()
    {
        return "GameOver";
    }
}
