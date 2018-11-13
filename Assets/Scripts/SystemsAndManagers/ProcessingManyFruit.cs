using System;
using UnityEngine;

public class ProcessingManyFruit : Singleton<ProcessingManyFruit>, IDisposable
{

    private PoolManager _poolManager;
    private Vector3 _offsetForHitUi;

    public ProcessingManyFruit()
    {
        _poolManager = GameManager.Instance.Pool;
        _offsetForHitUi = new Vector3(3, -1.5f);
        EventManager.StartListening("ManyFruits", ManyFruit);
    }

    /// <summary>
    /// Проверка стороны экрана. false - левая сторона, right - правая сторона
    /// </summary>
    bool CheckSideScreen(Vector2 eventPosition)
    {
        return eventPosition.x > 0;   //right side screen
    }

    private void ManyFruit(GameObject obj, string param)
    {
        var positionEvenet = CheckSideScreen(obj.transform.position) ?
            obj.transform.position + new Vector3(-_offsetForHitUi.x, _offsetForHitUi.y) : obj.transform.position + _offsetForHitUi;

        GameObject hitUI = null;
        if (param == "nice")
        {
            hitUI = _poolManager.GetObject(PoolType.UIHitNormal);
        }
        else if (param == "perfect")
        {
            hitUI = _poolManager.GetObject(PoolType.UIHitPerfect);
        }
        else if (param == null) return;

        hitUI.transform.position = positionEvenet;
        hitUI.gameObject.SetActive(true);
    }

    public void Dispose()
    {
        _instance = null;
        EventManager.StopListening("ManyFruits", ManyFruit);
        _poolManager = null;
    }
}
