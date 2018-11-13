using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class RotationGide : MonoBehaviour
{

	public float Angel;
	public TransformType type;
	
	// Update is called once per frame
	void Update ()
	{
		var direction = Vector3.zero;
		Swithc(out direction);
		transform.rotation = Quaternion.AngleAxis(Angel, transform.up);
	}

	private void OnDrawGizmos()
	{
		
		Debug.DrawLine(transform.position, transform.forward * 3, Color.green);
		Debug.DrawLine(transform.position, transform.right * 3, Color.yellow);
		Debug.DrawLine(transform.position, transform.up * 3, Color.red);
	}

	void Swithc(out Vector3 trans)
	{
		if (type == TransformType.Forward)
		{
			trans = transform.forward;
		}
		
		else if (type == TransformType.Right)
		{
			trans = transform.right;
		}

		else if (type == TransformType.Up)
		{
			trans = transform.up;
		}
		
		trans = Vector3.zero;
	}

	private void OnDisable()
	{
		Debug.Log("disable");
	}
}


public enum TransformType
{
	Forward, Up, Right
}
