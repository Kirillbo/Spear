using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ParticleObject : MonoBehaviour
{

	private void OnEnable()
	{
		Invoke("ReturnObject", 1.3f);
	}

	void ReturnObject()
	{
		PoolManager.Instance.ReturnObject(PoolType.Particles, gameObject);
	}
}
