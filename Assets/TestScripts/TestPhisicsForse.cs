using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestPhisicsForse : MonoBehaviour
{

    public GameObject test;
    
    void Start()
    {
        Lel();
        Invoke("Lel", 2f);

    }


    void Lel()
    {
        MoveTo(transform, transform.position, transform.position * 4, 2);
        Debug.Log("work corut");
    }
    
    public void MoveTo(Transform obj ,Vector3 startPos, Vector3 targetPos, float speed)
    {
        StartCoroutine(IEnumeratorMoveTo(obj, startPos, targetPos, speed));
    }


    IEnumerator IEnumeratorMoveTo(Transform objectToMove, Vector3 from, Vector3 to, float speed)
    {

        float step = (speed / (from - to).magnitude) * Time.deltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         
        }
        objectToMove.position = to;
        test.SetActive(false);
    }

}
