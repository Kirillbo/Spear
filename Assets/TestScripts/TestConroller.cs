using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestConroller : MonoBehaviour
{

    public GameObject prefabSick;
    public float SpeedMoveStick;
    private Vector3 _firstPositionMouse;

    public bool RestartScene;


	// Update is called once per frame
	void Update ()
	{
	    if (RestartScene)
	    {
	        SceneManager.LoadScene(0);
	        RestartScene = false;
	    }

	    if (Input.GetMouseButtonDown(0))
	    {
	        _firstPositionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _firstPositionMouse -= new Vector3(0,0, -2);
	    }

	    if (Input.GetMouseButtonUp(0))
	    {
	        var lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lastMousePos -= new Vector3(0,0, -2);
	        var direction = lastMousePos - _firstPositionMouse;
	        direction = direction.normalized;
            ShotStick(direction, _firstPositionMouse);
	        _firstPositionMouse = Vector3.zero;
	    }
	}


    void ShotStick(Vector3 direction, Vector3 position)
    {
        if (direction == Vector3.zero) return;

        var angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var stick = Instantiate(prefabSick);
        stick.transform.eulerAngles = new Vector3(0, 0, angel - 90);
        stick.transform.position = new Vector3(position.x, position.y, -2);
        stick.SetActive(true);
       
    }
}
