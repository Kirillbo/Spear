using System;
using UnityEngine;
using System.Collections;
using Tools;
using Random = UnityEngine.Random;

public class ScrollBackground : MonoBehaviour
{


    public SpritesBase SpriteBackground;
    private float _hieghtSprite;
    private float _widthSprite;
    private float _timer;
    private Vector3 _startPosition;
    public event Action FinishScroll;
    public Transform VisibleBack, InvisibleBack;
    public float SpeedChange;
    private int _numberBackground;
    
    public event Action EndScrolling;
    
    void Start()
    {
        SpriteBackground.Sprites.Shuffle();
        _hieghtSprite = VisibleBack.GetComponent<SpriteRenderer>().bounds.size.y;
        _widthSprite = VisibleBack.GetComponent<SpriteRenderer>().bounds.size.x * 2;
        _timer = _hieghtSprite / 6f;
        
        ChangeSprite(InvisibleBack, SpriteBackground.Sprites[_numberBackground]);
    }

    public void ChangeBackgroundEvent()
    {
        Change(VisibleBack, InvisibleBack, SpeedChange);
    }
    
    public void Change(Transform visibleBackground, Transform invisibleBackgrond, float speed)
    {
        var targetForVisible = VisibleBack.position - new Vector3(0, _hieghtSprite, 0);
        var targetForInvisbile = invisibleBackgrond.position - new Vector3(0, _hieghtSprite, 0);
        
        MoveTo(visibleBackground, visibleBackground.position, targetForVisible, speed);
        MoveTo(invisibleBackgrond, invisibleBackgrond.position, targetForInvisbile, speed, () => DetectVisibleAndInvisible(visibleBackground, invisibleBackgrond));
        
    }

    void DetectVisibleAndInvisible(Transform visib, Transform invisible)
    {
        VisibleBack = invisible;
        InvisibleBack = visib;
        InvisibleBack.position += Vector3.up * _hieghtSprite * 2;

        _numberBackground += 1;

        if (_numberBackground > SpriteBackground.Lengths)
        {
            _numberBackground = 0;
            SpriteBackground.Sprites.Shuffle();
        }
        
        ChangeSprite(InvisibleBack, SpriteBackground.Sprites[_numberBackground]);
    }


    void ChangeSprite(Transform obj, Sprite inputSprite)
    {
        var sp = obj.GetComponent<SpriteRenderer>();
        if(sp == null) return;

        sp.sprite = inputSprite;
    }
    
    public void MoveTo(Transform obj, Vector3 startPos, Vector3 targetPos, float speed, Action method = null)
    {
        StartCoroutine(IEnumeratorMoveTo(obj, startPos, targetPos, speed, method));
    }

    IEnumerator IEnumeratorMoveTo(Transform objectToMove, Vector3 from, Vector3 to, float speed, Action method = null)
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

        if (method != null)
        {
            method.Invoke();
            if (EndScrolling != null) EndScrolling();
        }
    }

}
