using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Homebrew;
using TMPro;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class TimeDelay : AState
{

    
    [Foldout("InputValue")] public float AngelRotateCam;
    [Foldout("InputValue")] public float ZoomForCam;
    [Foldout("InputValue")] public float LifeTimerState;
    [Foldout("InputValue")] public float SpeedMoveStick;

    [Header("Open field")]
    public Camera MainCam;
    public Transform TargetObject;
    public TMP_Text TextField;
    public Canvas CanvasTimeDelay;

    private const string _lable = "x {0}";

    private Vector3 _startPositionCam;
    private float _startZoomCam;
    private bool _allowedToShoot;                //разрешение на стрельбу палками
    private PoolManager _poolManager;
    private int _countScore;
    private Vector3 _centerScreen;

    public override void Init()
    {
        _poolManager = Manager.Pool;
        SavePrimaryStateCam();
        _centerScreen = MainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));

    }

    public override void Enter()
    {
        _countScore = 0;
        TargetObject = GameObject.FindGameObjectsWithTag("CriticlFruit").First(n => n.GetComponent<CollisionCriticalFruit>().IsAcivate).transform;
        
        _centerScreen.Set(_centerScreen.x, _centerScreen.y, TargetObject.position.z);
        CanvasTimeDelay.enabled = true;

        Timer.Add(1f, delegate { ChangeZoomAndRotateCam(ZoomForCam, 3, AngelRotateCam);});
        Timer.Add(1f, delegate { MoveObject(TargetObject, _centerScreen, 6); });
        Timer.Add(1f, delegate { RotateObject(TargetObject, -38f, 6); });
        Timer.Add(1.3f, ()=> InputManager.Instance.TouchScreen += ShotStick);
        
        Timer.Add(LifeTimerState, ()=> Manager.ChangeState("GameState", 2f));
    }

    
    public override void Exit()
    {
        InputManager.Instance.TouchScreen -= ShotStick;
        GameManager.Instance.Session.Score = _countScore;
        GameManager.Instance.Session.AmountStck = 5;

        TargetObject.GetComponent<CollisionObject>().ReturnToPoolWithEffects();
        CanvasTimeDelay.enabled = false;
        TextField.SetText(_lable, 0);
        ChangeZoomAndRotateCam(_startZoomCam, 3, 0);

    }


    public override void Tick()
    {
        
    }

    void ShotStick(Vector3 pos, Vector3 dir)
    {
        if(!_allowedToShoot) return;
        
        ManagerSound.Instance.PlayEffect(Track.FruitSplash);
        var randomPosition = RandomPositionStick();
        var stick = _poolManager.GetObject(PoolType.Stick);
        var endMove = new Vector3(TargetObject.position.x, randomPosition.y, randomPosition.z) + Vector3.left * Random.Range(3, 7);
        stick.GetComponent<Stick>().enabled = false;
        stick.SetTransform(randomPosition, Quaternion.Euler(0, 0, 90));
        StartCoroutine(IEshotStick(stick.transform, endMove, SpeedMoveStick));
        UpdateBonusScore(1);
    }

    private IEnumerator IEshotStick(Transform obj, Vector3 target, float speed)
    {
        var component = obj.GetComponent<Stick>();
        var sprite = obj.GetComponentInChildren<SpriteRenderer>();
        
        yield return IEMoveObject(obj, target, speed);
        yield return IenumeratorChangeAlpha(sprite, 0, 0.6f);
       _poolManager.ReturnObject(PoolType.Stick, obj.gameObject);
        sprite.color = new Color(1,1,1,1);
        component.enabled = true;
    }
    
    
    void UpdateBonusScore(int val)
    {
        _countScore += val;
        TextField.SetText(_lable, _countScore);
    }
    
    Vector3 RandomPositionStick()
    {
        var randomPosition = Random.insideUnitCircle ;
        return  new Vector3(0, randomPosition.x, randomPosition.y) + TargetObject.position + Vector3.right * 6 - new Vector3(0,0, 0.6f);
    }


    void SavePrimaryStateCam()
    {
        _startZoomCam = MainCam.orthographicSize;
    }


    void ChangeZoomAndRotateCam(float targetZoom, float speedZoomCam, float angelRotateZ)
    {
        StartCoroutine(IEZoomCam(targetZoom, speedZoomCam, angelRotateZ));

    }

    private IEnumerator IEZoomCam(float targetZoom, float speedZoomCam, float angelRotateZ)
    {   
        float time = 0f;
        var startRotation = MainCam.transform.rotation;                         //вращаем только по Z
        var endRotation = Quaternion.Euler(0, 0, angelRotateZ);
        float startOrthographicSize = MainCam.orthographicSize;
        
        while (time < 1f)
        {
            MainCam.orthographicSize = Mathf.Lerp(startOrthographicSize, targetZoom, time);
            time += Time.deltaTime * speedZoomCam;

            MainCam.transform.rotation = Quaternion.Lerp(startRotation, endRotation, time);            
            yield return null;
        }

        MainCam.orthographicSize = targetZoom;
        MainCam.transform.rotation = Quaternion.Euler(0, 0, angelRotateZ);
        _allowedToShoot = true;
    }

    void RotateObject(Transform transf, float angel, float speed = 1)
    {
        StartCoroutine(IERotateObject(transf, angel, speed));
    }

    
    IEnumerator IERotateObject(Transform objTransform, float angelY, float speed)
    {
        float time = 0;
        var startAngel = objTransform.rotation;
        
        while (time < 1)
        {
            objTransform.rotation = Quaternion.Lerp(startAngel, Quaternion.Euler(0, angelY, 90), time);
            time += Time.deltaTime * speed;
            yield return null;
        }
        objTransform.rotation = Quaternion.Euler(0, angelY, 90);
    }
    
    void MoveObject(Transform obj, Vector3 target, float speed)
    {
        StartCoroutine(IEMoveObject(obj, target, speed));
    }
    
    private IEnumerator IEMoveObject(Transform obj, Vector3 target, float speed)
    {
        float time = 0f;
        Vector3 startPosition = obj.position;
        while (time < 1f)
        {
            obj.position = Vector3.Lerp(startPosition, target, time);
            time += Time.deltaTime * speed;
            yield return null;
        }

        obj.position = target;
    }
    
    
    
    IEnumerator IenumeratorChangeAlpha(SpriteRenderer sprite, float targetValue, float speedChange)
    {
        if (targetValue == 1f)
        {
            sprite.color = new Color(1f,1f,1f, 0f);

            for (float i = 0; i < 1f; i += Time.deltaTime * speedChange)
            {
                sprite.color = new Color(1f,1f,1f, i);
                yield return null;
            }

            yield break;
        }

        else if (targetValue == 0.0f)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);

            for (float i = 1; i > 0; i -= Time.deltaTime * speedChange)
            {
                sprite.color = new Color(1f, 1f, 1f, i);
                yield return null;
            }
            yield break;
        }

    }

    public override string GetName()
    {
        return "TimeDelay";
    }
}

