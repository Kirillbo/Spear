using System.Collections;
using UnityEngine;

public class LevelUP : AState
{

	public ScrollBackground _managerScrolling;
    public Canvas LabelChangeLevel;


    public override void Init()
	{
		_managerScrolling.EndScrolling += () => Manager.ChangeState("GameState");
	}

	public override void Enter()
	{
	    LevelUPLavel();
        _managerScrolling.ChangeBackgroundEvent();
	}

	public override void Exit()
	{
		
	}

    public void LevelUPLavel()
    {
        StartCoroutine(IenumeratorShowText(3f));
    }

    IEnumerator IenumeratorShowText(float timeWait)
    {
        LabelChangeLevel.enabled = true;
        LabelChangeLevel.GetComponentInChildren<Animator>().SetTrigger("PLAY");
        yield return new WaitForSeconds(timeWait);
        LabelChangeLevel.enabled = false;
    }


    public override void Tick()
	{
		
	}

	public override string GetName()
	{
		return "LevelUpState";
	}
}
