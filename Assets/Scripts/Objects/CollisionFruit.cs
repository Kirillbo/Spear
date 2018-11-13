using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


public class CollisionFruit : CollisionObject
{

    public bool IsOnStick;
    private float _powerImpuls = 60f;
    public TypeFruit TypFruit;
    private MaterialBase ColorParticle;
    private MeshRenderer _view;

    protected override void Start()
    {
        base.Start();
        ColorParticle = GameManager.Instance.Colors;
        _view = GetComponentInChildren<MeshRenderer>();
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        
        if (col.CompareTag("Stick"))
        {
            if (IsOnStick) return;

            ManagerSound.Instance.PlayEffect(Track.FruitSplash);
            var pointContact = Collider.ClosestPoint(col.transform.position);
            var angelContact = col.GetComponent<DataObject>().RotationZ + 180f;

            var particle = PoolManager.GetObject(PoolType.Particles).GetComponent<ParticleSystem>();
            particle.gameObject.SetTransform(pointContact, Quaternion.Euler(0, 0, angelContact), col.transform);
            particle.GetComponent<ParticleSystemRenderer>().material = ColorParticle.Colors[(int) TypFruit];
            particle.Play();

            IsOnStick = true;
            Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
                             RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY |
                             RigidbodyConstraints.FreezeRotationX;
            var direction = col.transform.TransformPoint(col.bounds.size) - col.transform.position;
            direction = new Vector3(direction.x, 0, 0);
            Rb.AddForceAtPosition(direction * _powerImpuls, pointContact, ForceMode.Impulse);
            Collider.enabled = false;
        }
    }


    void Update()
    {
        var rot = transform.eulerAngles;
        if (rot.x < 75)
        {
            Rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnDisable()
    {
        IsOnStick = false;
        Collider.enabled = true;
    }



    public override PoolType ReturnType()
    {
        return PoolType.Fruit;
    }

}

public enum TypeFruit
{
    Orange = 1, Apple = 4, Watermelon = 2, Kivi = 0, Mango = 3
}
