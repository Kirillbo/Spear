using System;
using UnityEngine;

public class InputManager : SingltoonBehavior<InputManager>
{
    //EVENTS
    public event Action<Vector3, Vector3> TouchScreen;

    public Vector3 PositionTouch;
    private float _depthCamera = 0f;

    public Vector3 StartMousePos;
    Vector3 EndMousePos;
    public TouchInfo InfoTouch;

    public class TouchInfo
    {
        public Vector2 Direction,BeganTouch, EndTouch;

        public TouchInfo()
        {
            Direction = Vector2.zero;
            BeganTouch = Vector2.zero;
            EndTouch = Vector2.zero;
        }
    }

    void Start()
    {
        InfoTouch = new TouchInfo();
    }

    void Update ()
	{

	    if (Input.GetMouseButtonDown(0))
        { 
	        var mousePos = Input.mousePosition;
	        var newMousePosition = Camera.main.ScreenToWorldPoint(mousePos);
            InfoTouch.BeganTouch = new Vector3(newMousePosition.x, newMousePosition.y, 0f);
	    }

        else if (Input.GetMouseButtonUp(0))
	    {
	        var mousePos = Input.mousePosition;
	        InfoTouch.EndTouch = Camera.main.ScreenToWorldPoint(mousePos);
	        InfoTouch.Direction = (InfoTouch.EndTouch - InfoTouch.BeganTouch).normalized;
            BeTouch();

        }
    }



    void BeTouch()
    {
        if (TouchScreen != null)
        {
            TouchScreen(InfoTouch.Direction, InfoTouch.BeganTouch);
        }
    }
}
