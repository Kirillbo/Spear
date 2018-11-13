using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


public class Stick : MonoBehaviour
{
    private DataObject _dataObj;
    private float _leftEdgeX, _rightEdgeX, _leftEdgeY, _rightEdgeY;
    private float _timer;                                             //таймер после которого палка начинает реагировать на границы экрана
    public float EndTimer;
    private bool _visibleOnCam;
    private DestroyStick _destroyComponent;
    private CameraInspector _cam;
    private SpriteRenderer _spriteRenderer;
    

    void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _dataObj = GetComponent<DataObject>();
        _destroyComponent = GetComponent<DestroyStick>();
    }

    void Start()
    {
        SetConfinesMainCam();
    }
    
    void OnDisable()
    {
        PrimariState();
    }

    void Update()
    {
        Timer(Time.deltaTime);

        var pos = transform.position;
        if (pos.x < _leftEdgeX || pos.y < _leftEdgeY || pos.x > _rightEdgeX || pos.y > _rightEdgeY)
        {
            if (_dataObj.VisibleOnCam)
            {
                //TODO стик при отключении скрипта принемает вертикальное положение
                
                GetComponent<Collider>().enabled = false;
                return;
            }
        }
            
        transform.position += transform.up * _dataObj.SpeedMove * Time.deltaTime;
    }


    /// <summary>
    /// задает ограничения движения до краев камеры
    /// </summary>
    public void SetConfinesMainCam()
    {
        _cam = CameraInspector.Instance;
        _leftEdgeX = _cam.LeftEdgeCam.x;
        _leftEdgeY = _cam.LeftEdgeCam.y;
        _rightEdgeX = _cam.RightEdgeCam.x;
        _rightEdgeY = _cam.RightEdgeCam.y;
        
    }
    
    private void PrimariState()
    {
        
        transform.rotation = Quaternion.identity;
        _timer = 0;
        _spriteRenderer.enabled = true;
    }

    private void Timer(float deltaTime)
    {
        if(_timer > EndTimer) return;

        _timer += deltaTime;
        if (_timer > EndTimer)
        {
            _dataObj.VisibleOnCam = true;
            NewTimer.Add(_dataObj.TimeDestroy, _destroyComponent.Destroy);
        }
    }

    public void MoveTo(Vector3 startPos, Vector3 targetPos, float speed)
    {
        StartCoroutine(IEnumeratorMoveTo(startPos, targetPos, speed));
    }

    public void MoveTo(Transform obj ,Vector3 startPos, Vector3 targetPos, float speed)
    {
        StartCoroutine(IEnumeratorMoveTo(obj, startPos, targetPos, speed));
    }

    IEnumerator IEnumeratorMoveTo(Transform objectToMove, Vector3 from, Vector3 to, float speed)
    {

        float step = (speed / (from - to).magnitude) * Time.deltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            objectToMove.position = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         
        }
        objectToMove.position = to;
    }

    IEnumerator IEnumeratorMoveTo(Vector3 from,  Vector3 to, float speed)
    {

        float step = (speed / (transform.position - to).magnitude) * Time.deltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            var newPos = Vector3.Lerp(from, to, t); // Move objectToMove closer to b
            transform.position = newPos;
            yield return new WaitForFixedUpdate();
        }

        transform.position = to;
    }

    public void SetActiveRender(bool val)
    {
        _spriteRenderer.enabled = val;
    }
}
