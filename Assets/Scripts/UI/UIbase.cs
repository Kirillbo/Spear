using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIbase : MonoBehaviour {

    public void Skip()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

}
