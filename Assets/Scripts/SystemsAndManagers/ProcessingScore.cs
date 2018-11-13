using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProcessingScore : IDisposable
{

    public static ProcessingScore Instantiate
    {
        get { return _inst ?? (_inst = new ProcessingScore()); }
    }

    private static ProcessingScore _inst;
    private DataSession _session;
    private TMP_Text _textScore;
    
    public ProcessingScore()
    {
        _textScore = ManagerView.Get<ContainerUIGameProcess>().FieldTextScore;
        _textScore.text = "0";
        _session = GameManager.Instance.Session;
        _session.ChangeScore += UpdateUIpanel;
    }

    private void UpdateUIpanel()
    {
        _textScore.text = _session.Score.ToString();
    }


    public void Dispose()
    {
        _session.ChangeScore -= UpdateUIpanel;
        _inst = null;
        _textScore = null;
        _session = null;
    }
}
