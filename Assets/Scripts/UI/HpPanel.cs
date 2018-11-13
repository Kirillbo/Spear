using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpPanel : UIbase
{
    public Image[] ImageHp;
    private int _count;
    public Color DownColor;


    void DownHp()
    {
        if(_count > ImageHp.Length) return;

        ImageHp[_count].color = DownColor;
        _count++;
    }
}
