using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class ProcessingIndicator 
{
    private static ProcessingIndicator _inst;
    private float height;
    private float heightSprite;
    private float _offset;
    private float _minX, _maxX;
    
    public ProcessingIndicator()
    {
        _inst = this;
        EventManager.StartListening("Damage", SpawnIndicator);
        height = CameraInspector.Instance.LeftEdgeCam.y;
        _minX = CameraInspector.Instance.LeftEdgeCam.x;
        _maxX = CameraInspector.Instance.RightEdgeCam.x;
        heightSprite = 0f;
        _offset = -0.8f;
    }

    private void SpawnIndicator(GameObject arg0, string arg1)
    {
        var indicatorObj = GameManager.Instance.Pool.GetObject(PoolType.Indicator);
        heightSprite = heightSprite == 0 ? indicatorObj.GetComponent<SpriteRenderer>().bounds.size.y : heightSprite;

        float posX = Mathf.Clamp(arg0.transform.position.x, _minX, _maxX);
        float posY = height + heightSprite + _offset;
        Vector2 pos = new Vector2(posX,posY);
        indicatorObj.transform.position = pos;
        indicatorObj.SetActive(true);
    }

    public static ProcessingIndicator Instantiate
    {
        get { return _inst ?? (_inst = new ProcessingIndicator()); }
    }
    
    
}

