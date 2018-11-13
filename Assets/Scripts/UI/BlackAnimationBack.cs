using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackAnimationBack : MonoBehaviour
{

    private Image _image;

	void Start ()
	{
	    _image = GetComponentInChildren<Image>();
	}

    public float Speed;

    public void Curtain(Anim action)
    {
        StartCoroutine(ChangeRadial(action));
    }

    public IEnumerator ChangeRadial(Anim action)
    {
        float t;

        switch (action)
        {
            case Anim.Open:
                t = 1;
                while (t > 0)
                {
                    _image.fillAmount = t;
                    t -= Time.deltaTime * Speed;
                    yield return null;
                }
                _image.fillAmount = 0;
                yield break;

            case Anim.Close:
                t = 0;
                while (t < 1)
                {
                    _image.fillAmount = t;
                    t += Time.deltaTime * Speed;
                    yield return null;
                }
                _image.fillAmount = 1;
                yield break;
        }
    }
}

public enum Anim
{
    Open, Close
}
