using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DataObject : MonoBehaviour
{

    public float SpeedMove;
    public float TimeDestroy;
    public bool VisibleOnCam, ContactWithWalk;
    public float RotationZ;
    public bool isDestroy;

    private void OnDisable()
    {
        ContactWithWalk = VisibleOnCam = false;
        isDestroy = false;
    }

  
}
