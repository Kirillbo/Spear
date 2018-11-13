using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class Fruit : MonoBehaviour, IPoollable
{
    private Rigidbody _rb;
    private Quaternion _originalQuaternion;
    private RigidbodyConstraints _firstRigidbodyConstraints;

    private float _maxQuanterionY = 22f;
    private float _minQuanterionY = 10f;


    void OnDisable()
    {
        PrimaryState();
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _firstRigidbodyConstraints = _rb.constraints;
    }

    public Vector2 velocity, angelVelocyti;


    /// <summary>
    /// первичное состояние объекта
    /// </summary>
    public void PrimaryState()
    {
        _rb.constraints = _firstRigidbodyConstraints;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        transform.rotation = _originalQuaternion;
    }

    public void PrimaryStateBeforeStick()
    {
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
    }

    float RandomQanterionY(float minValue, float MaxValue)
    {
        var quanter = Random.Range(minValue, MaxValue);
        return quanter * ToolsRandom.RandomSign();
    }


    public void DestroyFruit()
    {
        
    }

    public void Init()
    {
        var randomY = RandomQanterionY(_minQuanterionY, _maxQuanterionY);
        _originalQuaternion = Quaternion.Euler(new Vector3(0,randomY,90));
        transform.rotation = _originalQuaternion;
    }

    public void ReSpawn()
    {
        
    }

    public void Despawn()
    {
        
    }
}


