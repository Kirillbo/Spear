using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingHP : Singleton<ProcessingHP>, IDisposable
{

    private int _damage;
    private Image[] _hpImages;

    public ProcessingHP()
    {
        _instance = this;
        _damage = -1;
        _hpImages = ManagerView.Get<ContainerUIGameProcess>().ImageHP;
        foreach (var elemnt in _hpImages)
        {
            elemnt.gameObject.SetActive(true);
        }

        EventManager.StartListening("Damage", Damage);
    }

    private void Damage(GameObject obj, string param)
    {
        _damage = Mathf.Min(_damage += 1, 2);
        _hpImages[_damage].gameObject.SetActive(false);

        if (_damage == 2 && !GameManager.Instance.GameOver && !Data.Instance.GODMODE)
        {
            GameManager.Instance.GameOver = true;
            GameManager.Instance.ChangeState("GameOver");
        }
    }


    public void Dispose()
    {
        EventManager.StopListening("Damage", Damage);
        _hpImages = null;
        _instance = null;
    }
}
