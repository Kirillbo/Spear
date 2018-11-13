using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;

public class TestTimer : MonoBehaviour
{


    IEnumerator Start()
    {
        yield return WaitCorutine();
        Debug.Log("Finish corut");
        StartCoroutine(SecondCorutine());
    }

    private IEnumerator SecondCorutine()
    {
        yield return null;
        Debug.Log("Work second corut");
    }

    IEnumerator WaitCorutine()
    {
        Debug.Log("zapusck cortu");
        yield return new WaitForSeconds(4f);
    }
    
}
