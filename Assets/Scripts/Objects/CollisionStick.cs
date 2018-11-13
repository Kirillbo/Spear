using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;


public class CollisionStick : CollisionObject
{
    private FruitContainer2 _container2;
    private DataObject _dataObj;
    private SpriteRenderer _spriteRenderer;
    private float _speedInvisible = 3;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    protected override void Start()
    {
        base.Start();
        _container2 = GetComponent<FruitContainer2>();
        _dataObj = GetComponent<DataObject>();
    }

    protected override void OnTriggerEnter(Collider col)
    {

         if(col.CompareTag("Fruit"))
        {        
             _container2.Add(col);
        }
    }

    private void OnDisable()
    {
        _spriteRenderer.color = new Color(1,1,1,1);
        Collider.enabled = true;
        StopCoroutine("IEReturntoPool");
    }

    public override PoolType ReturnType()
    {
        return PoolType.Stick;
    }

    public override void ReturnToPoolWithEffects()
    {
        StartCoroutine("IEReturntoPool");
    }
    
     IEnumerator IEReturntoPool()
    {

        float delta = Time.deltaTime * _speedInvisible;
        
        for (float i = 1; i > 0; i -= delta)
        {
            if (!gameObject.activeInHierarchy)
            {
                break;
            }
            _spriteRenderer.color = new Color(1, 1, 1, i);
            yield return null;;
        }
        
        ReturnToPool();
    }
}
