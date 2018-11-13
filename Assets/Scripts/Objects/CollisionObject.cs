using System.Collections;
using UnityEngine;

public abstract class CollisionObject : MonoBehaviour
{

    protected Rigidbody Rb;
    protected Collider Collider;
    protected PoolManager PoolManager;
    private bool test;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        PoolManager = PoolManager.Instance;
       // Debug.Assert(PoolManager , "PoolManager is not found");
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("LevelEdge"))
        {
            gameObject.SetActive(false);
            ReturnToPool();

            if (GameManager.Instance.GetCurrentState() is GameProcess)
            {
                EventManager.TriggerEvent("Damage", null, gameObject);
            }
        }
    }

    public void ReturnToPool()
    {
        PoolManager.ReturnObject(ReturnType(), gameObject);
    }

    public abstract PoolType ReturnType();

    public virtual void ReturnToPoolWithEffects()
    {
        PoolManager.ReturnObject(ReturnType(), gameObject);
    }


}
