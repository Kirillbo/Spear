using System.Collections;
using Core.Tools;
using UnityEngine;

public class SmoothStartMove : MonoBehaviour {



	// Use this for initialization
	void Start ()
	{
	    int count = 0;
        
    }


    IEnumerator Move(Transform obj, Vector3 from, Vector3 to, float speed)
    {
        float time = 0f;

        while (time < 1.0f)
        {
            obj.position = ToolsMove.SmoothStart3(from, to, time);
            time += Time.deltaTime * speed;
            yield return null;
        }

        
    }
}
