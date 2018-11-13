using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBlaserDisable : MonoBehaviour
{

	private float TimeToAction = 1f;
	private TrailRenderer _trail;
	private bool _corutineIsStart;
	private float _timeRenderer = 2; 		//время которое будет сущестсовать эфект
	
	// Use this for initialization
	void Awake ()
	{
		_trail = GetComponent<TrailRenderer>();
		Debug.Assert(_trail, "Trail is not init");
	}

	private void OnBecameVisible()
	{
		if (!_corutineIsStart)
		{
			StartCoroutine(DisableObject(TimeToAction));
		}
	}

	private void OnDisable()
	{
		_trail.emitting = true;
	}

	private IEnumerator DisableObject(float timeDisable)
	{
		_corutineIsStart = true;
		yield return new WaitForSeconds(timeDisable);

		_trail.emitting = false;
		_corutineIsStart = false;
	}

//	private IEnumerator DisableObject(float timeDisable)
//	{
//		_corutineIsStart = true;
//		_trail.time = 0.3f;
//		yield return new WaitForSeconds(timeDisable);
//
//		while (_trail.time > 0)
//		{
//			_trail.time -= Time.deltaTime;
//			yield return null;
//		}
//		_corutineIsStart = false;
//	}
}
