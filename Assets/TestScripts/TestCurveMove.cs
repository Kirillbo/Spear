using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Tools;

public class TestCurveMove : MonoBehaviour
{

    public Transform target;
    public Transform startPos;

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(Move(transform, startPos.position, target.position, 0.3f));
	}

    private float timer;

	// Update is called once per frame
	void Update ()
	{
	    timer += Time.deltaTime / 2;

	}


    IEnumerator Move(Transform obj, Vector3 from, Vector3 to, float speed)
    {
        float time = 0f;

        while (time < 1)
        {
            obj.position = ToolsMove.SmoothStop4(from, to, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
	    
	    target.gameObject.SetActive(false);
    }


}
