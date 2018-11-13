using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInspector : SingltoonBehavior<CameraInspector>
{

    public Camera CamMain;
    public Vector2 LeftEdgeCam, RightEdgeCam;
    

     protected override void Awake()
    {
        base.Awake();
        CamMain = Camera.main;
        LeftEdgeCam = Camera.main.ViewportToWorldPoint(new Vector2(0.01f, 0.01f));
        RightEdgeCam = Camera.main.ViewportToWorldPoint(new Vector2(0.99f, 0.99f));
    }
}
