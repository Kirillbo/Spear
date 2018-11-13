using UnityEngine;

public class CollisionFruit2 : CollisionObject
{

    public bool IsOnStick;
    public float PowerInpuls;
    private GameProcess _gameProcess;

    private Transform _target;


    protected override void Start()
    {
        base.Start();
        // _gameProcess = GameManager.Instantiate.FindState("GameState") as GameProcess;
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("LevelEdge"))
        {
            gameObject.SetActive(false);
            ReturnToPool();
            EventManager.TriggerEvent("Damage", null);
        }

        if (col.CompareTag("Stick"))
        {

            if(IsOnStick) return;

            _target = col.transform;
            Invoke("NewParent", 0.2f);
            IsOnStick = true;
            Rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            var pointContact = Collider.ClosestPoint(col.transform.position);
            var direction = col.transform.TransformPoint(col.bounds.size) - col.transform.position;
            direction = new Vector3(direction.x, 0, 0);
            Rb.AddForceAtPosition(direction * PowerInpuls, pointContact, ForceMode.Impulse);
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

    void NewParent()
    {
        transform.SetParent(_target);
    }
    
    void OnEnable()
    {
        IsOnStick = false;
    }


    public override PoolType ReturnType()
    {
        return PoolType.Fruit;
    }

}
