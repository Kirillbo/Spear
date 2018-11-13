using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSprite : MonoBehaviour
{

    private SpriteRenderer[] _SpriteRenderer;

    void Start()
    {
        _SpriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        Debug.Log(_SpriteRenderer[0].bounds.size.x);
        Debug.Log(CalculateWidth(_SpriteRenderer));
    }

    float CalculateWidth(SpriteRenderer[] arrSprites)
    {
        float sum = 0;

        foreach (var sp in arrSprites)
        {
            sum += sp.bounds.size.x;
        }

        return sum;
    }
}
