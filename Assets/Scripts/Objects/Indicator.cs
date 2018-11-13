using System.Collections;
using UnityEngine;

public class Indicator : MonoBehaviour
{


	private SpriteRenderer _spriteRenderer;
	public float SpeedChangeAlpha;
	private bool _isStartCorutine;
	
	public void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void OnEnable()
	{
		StartCoroutine(ProcessObject(_spriteRenderer, 0, SpeedChangeAlpha));
	}

	IEnumerator ProcessObject(SpriteRenderer sprite, float targetValue, float speedChange)
	{
		if (_isStartCorutine) yield break;

		yield return new WaitForSeconds(1.5f);
		_isStartCorutine = true;
		yield return IenumeratorChangeAlpha(sprite, targetValue, speedChange);
		PoolManager.Instance.ReturnObject(PoolType.Indicator, gameObject);
		_spriteRenderer.color = new Color(1,1,1,1);
		_isStartCorutine = false;
	}
	
	IEnumerator IenumeratorChangeAlpha(SpriteRenderer sprite, float targetValue, float speedChange)
	{
		if (targetValue == 1f)
		{
			sprite.color = new Color(1f,1f,1f, 0f);

			for (float i = 0; i < 1f; i += Time.deltaTime * speedChange)
			{
				sprite.color = new Color(1f,1f,1f, i);
				yield return null;
			}

			yield break;
		}

		else if (targetValue == 0.0f)
		{
			sprite.color = new Color(1f, 1f, 1f, 1f);

			for (float i = 1; i > 0; i -= Time.deltaTime * speedChange)
			{
				sprite.color = new Color(1f, 1f, 1f, i);
				yield return null;
			}
			yield break;
		}

	}
}
