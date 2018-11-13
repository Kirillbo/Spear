using System.Collections;
using System.Collections.Generic;
using Core.Tools;
using UnityEngine;

public class SmoothStopMove : MonoBehaviour {



    void Start()
    {
        StartCoroutine(Move(transform, transform.position, transform.position + Vector3.left * 20, 0.5f));
    }


    IEnumerator Move(Transform obj, Vector3 from, Vector3 to, float speed)
    {
        float time = 0f;

        while (time < 1f)
        {
            obj.position = ToolsMove.SmoothStop4(from, to, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
    }
}
