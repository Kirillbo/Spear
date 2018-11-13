using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{


	public float Scale1, Scale2, ChangeSpeed;
	
	private void OnEnable()
	{
		StartCoroutine(ProcessChangeSca(Scale1, Scale2, ChangeSpeed));

	}


	private void OnDisable()
	{
		StopAllCoroutines();
	}


	private IEnumerator ProcessChangeSca(float firstTarget, float secondTarget, float speed)
	{
		Vector3 frist = new Vector3(firstTarget, firstTarget, firstTarget);
		Vector3 second = new Vector3(secondTarget, secondTarget, secondTarget);
		
		yield return IEChangeScale(transform, frist, speed);
		yield return IEChangeScale(transform, second, speed);
	}

	private IEnumerator IEChangeScale(Transform obj, Vector3 target, float speed)
	{
		float time = 0f;
		Vector3 startScale = obj.localScale;
		while (time < 1f)
		{
			obj.localScale = Vector3.Lerp(startScale, target, time);
			time += Time.deltaTime * speed;
			yield return null;
		}

		obj.localScale = target;
	}

}
