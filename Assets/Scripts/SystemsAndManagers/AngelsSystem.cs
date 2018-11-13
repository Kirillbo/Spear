using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AngelsSystem : MonoBehaviour
{

    public Transform leftVector, RightVector;
    public float Magnitude;
    public float DeltaMagnitude;
    public float minAngle, maxAngle;
    public Vector2 RedVector;
    public float DistanceBetweenLayer;


    void Awake()
    {
         minAngle = Vector2.Angle(Vector2.right, RightVector.localPosition);
         maxAngle = Vector2.Angle(Vector2.right, leftVector.localPosition);
    }

	void OnDrawGizmos()
	{
        //отсчет угла идет от правого края
	    
        Debug.DrawLine(transform.position, RightVector.position, Color.green);
        Debug.DrawLine(transform.position, leftVector.position, Color.green);
        Debug.DrawLine(transform.position, RedVector, Color.red);

	}

    /// <summary>
    /// Красный вектор движения
    /// </summary>
    /// <returns></returns>
    Vector2 CalculateVector()
    {
        var randomAngel = GetRandomAngel();
        var xDir = transform.localPosition.x + Magnitude * Mathf.Cos(randomAngel * Mathf.Deg2Rad);
        var yDir = transform.localPosition.y + Magnitude * Mathf.Sin(randomAngel * Mathf.Deg2Rad);

        return new Vector2(xDir, yDir);
    }

    Vector2[] CalculateVectors(int count, float stepAngel)
    {
        Vector2[] derivedVectors = new Vector2[count];
        var randomAngel = GetRandomAngel();

        for (int i = 0; i < count; i++)
        {
            var xDir = transform.localPosition.x + Magnitude * Mathf.Cos(randomAngel * Mathf.Deg2Rad);
            var yDir = transform.localPosition.y + Magnitude * Mathf.Sin(randomAngel * Mathf.Deg2Rad);
            randomAngel += stepAngel;
           // Debug.Log(randomAngel);

            derivedVectors[i] = new Vector2(xDir, yDir);
        }

        return derivedVectors;
    }

    public void StopCorutineAngelSystem()
    {
        StopAllCoroutines();
    }
    
    IEnumerator IEShotCirculss(Queue<GameObject> stackFruits, int count, float stepAngel, float timeBetweenShot)
    {
        int countCircles = 0;

        countCircles = stackFruits.Count < count ? stackFruits.Count : count;
    
        Vector2[] directionsMove = CalculateVectors(countCircles, stepAngel);
        float stepLayer = 0;
        var magnitude = Magnitude;

        for (int i = 0; i < countCircles; i++)
        {
            GameObject obj = stackFruits.Dequeue();
            RedVector = directionsMove[i];

            obj.transform.position = new Vector3(transform.position.x, transform.position.y, stepLayer);
            //obj.GetComponent<Fruit>().velocity = transform.InverseTransformPoint(directionsMove[i]) * magnitude;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody>().AddForce(transform.InverseTransformPoint(directionsMove[i]) * magnitude, ForceMode.Impulse);
            stepLayer += DistanceBetweenLayer;
            magnitude -= DeltaMagnitude;

            yield return new WaitForSeconds(timeBetweenShot);
        }
    }


    public void ShotFruit( Queue<GameObject> stackObjects, int countFruit, float stepAngel, float timeBetweenShot)
    {
        StartCoroutine(IEShotCirculss(stackObjects, countFruit, stepAngel, timeBetweenShot));
    }

    private float GetRandomAngel()
    {
        return Random.Range(minAngle, maxAngle);
    }
}
