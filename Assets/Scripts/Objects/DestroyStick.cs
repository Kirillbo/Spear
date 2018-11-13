using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyStick : MonoBehaviour
{

	private DataObject _dataObject;
	private CollisionStick _collisionStick;
	private FruitContainer2 _fruitContainer2;
	private Stick _stickMoveSystem;

	void Awake()
	{
		_dataObject = GetComponent<DataObject>();
		_collisionStick = GetComponent<CollisionStick>();
		_fruitContainer2 = GetComponent<FruitContainer2>();
		_stickMoveSystem = GetComponent<Stick>();
	}

	public void Destroy()
	{
		_dataObject.isDestroy = true;
		_fruitContainer2.DestroyAll();
		_collisionStick.ReturnToPoolWithEffects();
		//_collisionStick.ReturnToPool();
	}
}
