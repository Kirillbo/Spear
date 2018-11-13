using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntorpolateMove : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        StartCoroutine(IEnumeratorMoveTo(transform.position, transform.position + Vector3.left * 20, 2f));
    }



    IEnumerator IEnumeratorMoveTo(Vector3 from, Vector3 to, float speed)
    {

        float step = (speed / (transform.position - to).magnitude) * Time.deltaTime;
        float t = 0;
        while (t <= 2f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            var newPos = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
            transform.position = newPos;
            yield return new WaitForFixedUpdate();
        }

        transform.position = to;
    }
}
