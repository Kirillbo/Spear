using System;
using System.Collections;
using System.Collections.Generic;
using Homebrew;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParabolistickMove : AState
{

    [Foldout("InputValue")] public float SpeedMove;
    [Foldout("InputValue")] public float SpeedRotateCam;
    [Foldout("InputValue")] public float AngelRotateCam;
    [Foldout("InputValue")] public float SpeedZoomCam;
    [Foldout("InputValue")] public float ZoomForCam;
    [Foldout("InputValue")] public float LifeTimerState;

    [Header("Open field")]
    public Camera MainCam;
    public Transform TargetObject;
    public SpriteRenderer BackgroundSpriteFX;
    public GameObject PrefabStick;
    public GameObject UiPanel;


    private Vector3 Target
    {
        get
        {
            return TargetObject.position + new Vector3(-2, 0, -2);
        }
    }
    private Vector3 _startPositionCam;
    private float _startZoomCam;
    public bool IsReverse;
    public bool MoveToTarget;
    private bool _allowedToShoot;
    private PoolManager _poolManager;
    private Stack<GameObject> _stackSticks;


    //EVENTS
    public event Action Shot;


    public override void Init()
    {
        
    }

    public override void Enter()
    {
        TargetObject = GameObject.FindWithTag("CriticlFruit").transform;
        if(TargetObject == null) Debug.Log("State don't get target");

        _stackSticks = new Stack<GameObject>();
        _poolManager = Manager.Pool;
        SavePrimaryStateCam();
        InputManager.Instance.TouchScreen += ShotStick;
        StartCoroutine(IEnumeratorMoveCam(MainCam.transform, MainCam.transform.position, Target, TargetObject.position, SpeedMove, ZoomForCam));
        Invoke("Reverse", 5f);

    }

    public override void Exit()
    {
       InputManager.Instance.TouchScreen -= ShotStick;
    }


    public override void Tick()
    {
     
        if (MoveToTarget)
        {
            StartCoroutine(IEnumeratorMoveCam(MainCam.transform, MainCam.transform.position, Target, TargetObject.position, SpeedMove, ZoomForCam));
            MoveToTarget = false;
        }

        if (IsReverse)
        {
            Reverse();
            IsReverse = false;
        }
    }

    void ShotStick(Vector3 pos, Vector3 dir)
    {
        if(!_allowedToShoot) return;

        var stick = _poolManager.GetObject(PoolType.Stick);
        _stackSticks.Push(stick);
        stick.transform.position = RandomPositionStick();
        stick.transform.eulerAngles = new Vector3(0,0,-90);
        stick.SetActive(true);
        Vector3 target = stick.transform.position + (Vector3.left * 4);

        OnShot();
    }

    Vector3 RandomPositionStick()
    {

        var randomPosition = Random.insideUnitCircle ;
        return  new Vector3(0, randomPosition.x, randomPosition.y) + TargetObject.position + Vector3.right * 2;
    }

    void Reverse()
    {
        _allowedToShoot = false;
          UiPanel.SetActive(false);
          DestroyObjectsOnScene();
          StopAllCoroutines();
          StartCoroutine(IenumeratorChangeAlpha(BackgroundSpriteFX, 0f, 4f));
          StartCoroutine(IEnumeratorReturnCam(MainCam.transform, Target, _startPositionCam, Vector3.zero, -SpeedMove, _startZoomCam));
    }

    void DestroyObjectsOnScene()
    {
        foreach (var stick in _stackSticks)
        {
            _poolManager.ReturnObject(PoolType.Stick, stick);
        }

        _stackSticks.Clear();
        Destroy(TargetObject.gameObject);
    }

    void SavePrimaryStateCam()
    {
        _startZoomCam = MainCam.orthographicSize;
        _startPositionCam = MainCam.transform.position;
    }


    private void Rotate(Vector3 target)
    {
        var direction = target - MainCam.transform.position;
        Quaternion rotate = Quaternion.LookRotation(direction);
        Vector3 ealer = rotate.eulerAngles;

        if (ealer.y < 180)
        {
            ealer.y = Mathf.Min(ealer.y, AngelRotateCam);
        }

        rotate = Quaternion.Euler(ealer);
        MainCam.transform.rotation = Quaternion.Lerp(MainCam.transform.rotation, rotate, Time.deltaTime * SpeedRotateCam);
    }

    private void BackRotate(Vector3 target)
    {
        MainCam.transform.rotation = Quaternion.Lerp(MainCam.transform.rotation, Quaternion.identity, Time.deltaTime * SpeedRotateCam * 4);
    }

    private void ZoomCam(float valueZoom)
    {
        var zoom = Mathf.MoveTowardsAngle(MainCam.orthographicSize, valueZoom, Time.deltaTime * SpeedZoomCam);
        MainCam.orthographicSize = zoom;
    }

    IEnumerator IenumeratorChangeAlpha(SpriteRenderer sprite, float value, float speedChange)
    {
        if (value == 1f)
        {
            sprite.color = new Color(1f,1f,1f, 0f);

            for (float i = 0; i < 1f; i += Time.deltaTime * speedChange)
            {
                sprite.color = new Color(1f,1f,1f, i);
                yield return null;
            }

            yield break;
        }

        if (value == 0.0f)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);

            for (float i = 1; i > 0; i -= Time.deltaTime * speedChange)
            {
                sprite.color = new Color(1f, 1f, 1f, i);
                yield return null;
            }

            BackgroundSpriteFX.gameObject.SetActive(false);
            yield break;
        }
        
    }

    public override string GetName()
    {
        return "TimeDelay";
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"> поворачиваемый объект</param>
    /// <param name="target"> цель для движения</param>
    /// <param name="targetForRotate">цель для поворота</param>
    /// <param name="speedMove"> скорость движения</param>
    /// <param name="zoomCam">приближение камеры </param>
    /// <returns></returns>
    IEnumerator IEnumeratorMoveCam(Transform obj, Vector3 startPlace, Vector3 target, Vector3 targetForRotate, float speedMove, float zoomCam)
    {

        yield return new WaitForSeconds(1f);

      //  BackgroundSpriteFX.gameObject.SetActive(true);
      //  StartCoroutine(IenumeratorChangeAlpha(BackgroundSpriteFX, 1, 2f));

        var centerCircle = (startPlace - target) / 2;
        var newPosCenterCircle = centerCircle + target;
        var R = centerCircle.magnitude;

        var angel = -GetAngelOnCircle(newPosCenterCircle, obj.position);
        var startAngel = angel - 180;

        while (angel > startAngel)
        {

            var radian = angel * Mathf.Deg2Rad;

            float x = (float) Math.Cos(radian) * R;
            float z = (float) Math.Sin(radian) * R;
            float y = Mathf.MoveTowards(obj.position.y, target.y, Time.deltaTime * 3);

            obj.position = new Vector3(x, y, z) + new Vector3(newPosCenterCircle.x, 0, newPosCenterCircle.z);

            angel -= Time.deltaTime * speedMove;

            Rotate(targetForRotate);
            ZoomCam(zoomCam);

            yield return null;
           }

        //        while (true)
        //        {
        //            var radian = angel * Mathf.Deg2Rad;
        //            
        //            float x = (float)Math.Cos(radian) * R;
        //            float z = (float)Math.Sin(radian) * R;
        //             float y = Mathf.MoveTowards(obj.position.y, target.y, Time.deltaTime);
        //
        //            obj.position = new Vector3(x, obj.position.y, z) + new Vector3(newPosCenterCircle.x, 0, newPosCenterCircle.z);
        //
        //            angel -= 0.01f;
        //
        //            yield return null;
        //        }

        UiPanel.SetActive(true);
        _allowedToShoot = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"> поворачиваемый объект</param>
    /// <param name="to"> цель для движения</param>
    /// <param name="targetForRotate">цель для поворота</param>
    /// <param name="speedMove"> скорость движения</param>
    /// <param name="zoomCam">приближение камеры </param>
    /// <returns></returns>
    IEnumerator IEnumeratorReturnCam(Transform obj, Vector3 from, Vector3 to, Vector3 targetForRotate, float speedMove, float zoomCam)
    {
        var centerCircle = (from - to) / 2;
        var newPosCenterCircle = centerCircle + to;
        var R = centerCircle.magnitude;

        var angel = -GetAngelOnCircle(newPosCenterCircle, obj.position);
        var startAngel = angel + 180;

        while (angel < startAngel)
        {

            var radian = angel * Mathf.Deg2Rad;

            float x = (float) Math.Cos(radian) * R;
            float z = (float) Math.Sin(radian) * R;
            float y = Mathf.MoveTowards(obj.position.y, to.y, Time.deltaTime * 3);

            obj.position = new Vector3(x, y, z) + new Vector3(newPosCenterCircle.x, 0, newPosCenterCircle.z);

            angel -= Time.deltaTime * speedMove;

            BackRotate(targetForRotate);
            ZoomCam(zoomCam);
            yield return null;
        }

        var camComponent = obj.GetComponent<Camera>();
        if (camComponent != null)
        {
            obj.position = _startPositionCam;
        }
        
        Manager.ChangeState("GameState");
    }


    /// <summary>
    /// Получение угла точки, лежащей на окружности
    /// </summary>
    /// <param name="center"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    float GetAngelOnCircle(Vector3 center, Vector3 point)
    {
        float angel = (float)(Math.Atan2(center.z - point.z, point.x - center.x) * (180 / Mathf.PI));
        return angel;
    }

    //    IEnumerator SimulateProjectile()
    //    {
    //        // Short delay added before Projectile is thrown
    //        yield return new WaitForSeconds(1.5f);
    //
    //        // Move projectile to the position of throwing object + add some offset if needed.
    //        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);
    //
    //        // Calculate distance to target
    //        float target_Distance = Vector3.Distance(Projectile.position, Target.position + Offset);
    //
    //
    //        // Рассчитаем скорость, необходимую для того, чтобы бросить объект в цель под заданным углом.
    //        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * StartAngel * Mathf.Deg2Rad) / gravity);
    //
    //        // Extract the X  Y componenent of the velocity
    //        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(StartAngel * Mathf.Deg2Rad);
    //        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(StartAngel * Mathf.Deg2Rad);
    //
    //        float rotateSpeed = 0f;
    //        float zoomSpeed = 0f;
    //
    //        // Расчет времени полета.
    //        float flightDuration = target_Distance / Vx;
    //
    //        Mathf.SmoothDamp(0, AngelRotateCam, ref rotateSpeed, flightDuration);
    //        Mathf.SmoothDamp(_startZoomCam, ZoomForCam, ref zoomSpeed, flightDuration);
    //
    //        float elapse_time = 0;
    //
    //        while (elapse_time < flightDuration)
    //        {
    //            Projectile.transform.position += new Vector3(-(Vy - (gravity * elapse_time)) * Time.deltaTime, 0, Vx * Time.deltaTime);
    //            Projectile.eulerAngles += new Vector3(0,rotateSpeed,0);
    //            //MainCam.orthographicSize += zoomSpeed;
    //            elapse_time += Time.deltaTime;
    //
    //            yield return null;
    //        }
    //    }
    //    



    protected virtual void OnShot()
    {
        var handler = Shot;
        if (handler != null) handler();

    }
}

