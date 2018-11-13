using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiGameOver : UIbase
{

    public TextMeshProUGUI ScoreField;

    public void Score(int score)
    {
        
        ScoreField.text = score.ToString();
    }
	
}
