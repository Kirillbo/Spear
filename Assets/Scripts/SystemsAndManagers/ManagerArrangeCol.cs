using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerArrangeCol : MonoBehaviour
{

    public Transform LeftCol;
    public Transform RightCol;

    private float _leftEdgeSceen;
    private float _rightEdgeScreen;

    public float Offset;

	void Start ()
	{
	    _leftEdgeSceen = CameraInspector.Instance.LeftEdgeCam.x;
	    _rightEdgeScreen = CameraInspector.Instance.RightEdgeCam.x;
	}

    void Update()
    {
        LeftCol.position = new Vector3( _leftEdgeSceen - Offset, LeftCol.position.y, LeftCol.position.z);
        RightCol.position = new Vector3( _rightEdgeScreen + Offset, RightCol.position.y, RightCol.position.z);
    }
}
