using System;
using TMPro;
using UnityEngine;

public class ProcessingCalculateSticks : IDisposable
{
    private static ProcessingCalculateSticks _inst;
    private TMP_Text _filedAmountScore;
    private Vector2 _positinOffset = new Vector2(6f, -4f);
    private Vector2 _neagativeOffset = new Vector2(-6f, -4f);
    
    public static ProcessingCalculateSticks Instantiate
    {
        get { return _inst ?? (_inst = new ProcessingCalculateSticks()); }
    }

    public ProcessingCalculateSticks()
    {
        _inst = this;
        _filedAmountScore = ManagerView.Get<ContainerUIGameProcess>().FieldTextAmountStick;
        _filedAmountScore.text = GameManager.Instance.Session.AmountStck.ToString();
        EventManager.StartListening("ChangeCountSticks", UpdateUI);
        EventManager.StartListening("ManyStick", ShowTitleManyStick);
    }

    void ShowTitleManyStick(GameObject place, string param)
    {
        var obj = PoolManager.Instance.GetObject(PoolType.UIHItSticks);
        Vector3 localOffset = CheckSideScreen(place.transform.position) ? _neagativeOffset : _positinOffset;
        
        obj.transform.position = place.transform.position + localOffset;
        obj.SetActive(true);
    }
    
    private void UpdateUI(GameObject arg0, string arg1)
    {
        var count = GameManager.Instance.Session.AmountStck;
        if (count < 1)
        {
            GameManager.Instance.ChangeState("GameOver");
        } 
        _filedAmountScore.text = GameManager.Instance.Session.AmountStck.ToString();
    }

    public void Dispose()
    {
        _inst = null;
        _filedAmountScore = null;
        EventManager.StopListening("ChangeCountSticks", UpdateUI);
        EventManager.StopListening("ManyStick", ShowTitleManyStick);
    }
    
    /// <summary>
    /// Проверка стороны экрана. false - левая сторона, right - правая сторона
    /// </summary>
    bool CheckSideScreen(Vector2 eventPosition)
    {
        return eventPosition.x > 0;   //right side screen
    }
}
