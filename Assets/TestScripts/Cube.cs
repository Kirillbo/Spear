using System;
using System.Collections;
using System.Collections.Generic;
using Homebrew;
using UnityEditor;
using UnityEngine;

public class Cube : MonoBehaviour
{

    [Foldout("InputValue")] public Transform Target;

    [Foldout("InputValue")] public float SpeedMove;


    private float R;
    private Vector3 CenterCircleNewPos;
    private float Radian;
    public float _startAngel;
    public float Angel;



        void Update()
        {
            Vector3 targetDir = Target.position - transform.position;

            // The step size is equal to speed times frame time.
            float step = SpeedMove * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    

    void Awake()
    {
        var centerCircle = (transform.position - Target.position) / 2;         //неверный центр окружности поскольку находится в начале отсчета
        CenterCircleNewPos = centerCircle + Target.position;

        R = centerCircle.magnitude;

        Angel = GetAngelOnCircle(CenterCircleNewPos, transform.position);
        _startAngel = Angel;
    }

    private float _curentTime;

//	void Update ()
//	{
//
//	    _curentTime += Time.deltaTime;
//        if(_curentTime < 2) return;
//
////	    if (Angel >= 180 + _startAngel) return;
//
//	    Radian = Angel * Mathf.Deg2Rad;
//
//	    float x = (float) Math.Cos(Radian) * R;
//	    float y = (float) Math.Sin(Radian) * R;
//
//	    transform.position = new Vector3(x,transform.position.y, y) + new Vector3(CenterCircleNewPos.x, 0, CenterCircleNewPos.z);
//       
//
//	    Angel += Time.deltaTime * SpeedMove;
//	}

    /// <summary>
    /// Получение угла точки, лежащей на окружности
    /// </summary>
    /// <param name="center"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    float GetAngelOnCircle(Vector3 center, Vector3 point)
    {
        float angel = (float)(Math.Atan2(center.z - point.z, point.x - center.x) * (180 / Mathf.PI));

        return Mathf.Abs(angel);
    }


}



//        _timer += Time.deltaTime;
//
//        if (_timer < 2) return;
//
//#region moveOnCircle
//            if (Angel <= _startAngel - 180) return;
//
//            Radian = Angel * Mathf.Deg2Rad;
//
//            float x = (float)Math.Cos(Radian) * R;
//            float z = (float)Math.Sin(Radian) * R;
//            float y = Mathf.MoveTowards(MainCam.transform.position.y, _target.y, Time.deltaTime * 2);
//
//            MainCam.transform.position = new Vector3(x, y, z) + new Vector3(CenterCircleNewPos.x, 0 , CenterCircleNewPos.z);
//
//            Angel -= Time.deltaTime * SpeedMove;
//#endregion

//
//        Rotate();
//        ZoomCam();


//public class Cube : MonoBehaviour
//{
//
//    [Foldout("InputValue")] public Transform Target;
//
//    [Foldout("InputValue")] public float SpeedMove;
//
//
//    private float R;
//    private Vector3 CenterCircleNewPos;
//    private float Radian;
//    private float _startAngel;
//    private float Angel;
//
//    void Awake()
//    {
//        var centerCircle = (transform.position - Target.position) / 2;         //неверный центр окружности поскольку находится в начале отсчета
//        CenterCircleNewPos = centerCircle + Target.position;
//
//        R = centerCircle.magnitude;
//
//        Angel = GetAngelOnCircle(CenterCircleNewPos, transform.position);
//        _startAngel = Angel;
//    }
//
//    private float _curentTime;
//
//    void Update()
//    {
//
//        _curentTime += Time.deltaTime;
//        if (_curentTime < 2) return;
//
//        if (Angel >= 180 + _startAngel) return;
//
//        Radian = Angel * Mathf.Deg2Rad;
//
//        float x = (float)Math.Cos(Radian) * R;
//        float y = (float)Math.Sin(Radian) * R;
//
//        transform.position = new Vector3(x, y, transform.position.z) + CenterCircleNewPos;
//
//        Angel += Time.deltaTime * SpeedMove;
//    }
//
//    /// <summary>
//    /// Получение угла точки, лежащей на окружности
//    /// </summary>
//    /// <param name="center"></param>
//    /// <param name="point"></param>
//    /// <returns></returns>
//    float GetAngelOnCircle(Vector3 center, Vector3 point)
//    {
//        float angel = (float)(Math.Atan2(center.y - point.y, point.x - center.x) * (180 / Mathf.PI));
//
//        return Mathf.Abs(angel);
//    }
//
//
//}

