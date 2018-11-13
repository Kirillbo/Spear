using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Material Database")]
public class MaterialBase : ScriptableObject
{

   
    public Material[] Colors;
    public Material CriticalColor;
    public Material HiddingFruit;

    public Material Critical()
    {
        return CriticalColor;
    }

    public int Lenght
    {
        get { return Colors.Length; }
    }
}
