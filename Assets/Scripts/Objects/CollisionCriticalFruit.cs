using System;
using System.Collections;
using Tools;
using UnityEngine;
using UnityEngine.Playables;


public class CollisionCriticalFruit : CollisionObject
{

    public bool TimeDelayState;
    private float _powerImpuls = 60f;
    public TypeFruit TypFruit;
    private MaterialBase ColorParticle;
    public ParticleSystem ParticleDestroy;
    private MeshRenderer _view;
    public bool IsAcivate;

    protected override void Awake()
    {
        base.Awake();
        _view = GetComponentInChildren<MeshRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        ColorParticle = GameManager.Instance.Colors;
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        if (col.CompareTag("Stick"))
        {

            ManagerSound.Instance.PlayEffect(Track.FruitSplash);

            var pointContact = Collider.ClosestPoint(col.transform.position);
            var angelContact = 0;

            var particle = PoolManager.GetObject(PoolType.Particles).GetComponent<ParticleSystem>();
            particle.gameObject.SetTransform(pointContact, Quaternion.Euler(0, 0, angelContact), col.transform);
            particle.GetComponent<ParticleSystemRenderer>().material = ColorParticle.Colors[(int) TypFruit];
            particle.Play();

            if (!TimeDelayState)
            {
                IsAcivate = true;
                Rb.isKinematic = true;
                GameManager.Instance.ChangeState("TimeDelay");
                TimeDelayState = true;
            }
        }
    }

    public override void ReturnToPoolWithEffects()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        ParticleDestroy.Play();
        _view.enabled = false;
        yield return new WaitForSeconds(2);
        ReturnToPool();
    }

    private void OnDisable()
    {
        TimeDelayState = false;
        Collider.enabled = true;
        _view.enabled = true;   
        IsAcivate = false;
    }


    public override PoolType ReturnType()
    {
        return PoolType.CritFruit;
    }

}

