using UnityEngine;
using UnityEngine.UI;

public class CheckColorButton : MonoBehaviour
{

	
	private Data _globData;
	private Image _image;
	
	void Start()
	{
		_globData = Data.Instance;
		_image = GetComponent<Image>();
	}

	private void Update()
	{
		_image.color = _globData.GODMODE ? Color.green : Color.red;
	}
}
