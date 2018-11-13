using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugTimeDeltatime : MonoBehaviour
{

    public Text textfield;

    void Update()
    {
        double value = Math.Round(Time.deltaTime, 4);

        textfield.text = value.ToString();
    }
}
