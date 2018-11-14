using System;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu( menuName = "ScriptableObjects/DataSession")]
public class DataSession : ScriptableObject
{

   
    public float TimeBetweenShot;
    public event Action ChangeScore = delegate { };
    
    public int Score
    {
        get { return _score; }
        set
        {
            _score += value;
            ChangeScore();

            if (_score >= ScoreForLevelUP[_currentLevel])
            {
                _currentLevel = Mathf.Min(_currentLevel += 1, ScoreForLevelUP.Length);
                int index = Mathf.Min(_currentLevel, Diffic.Length);
                
                AmountFruit = Diffic[index]._amountFruit;
                MaxExploisen = Diffic[index]._maxExploisen;
                MaxEasy = Diffic[index]._maxEasy;

                ChangeState();
            }
        }
    }
    
    [SerializeField]
    private int _score;
    public int _currentLevel;
    
    public int AmountFruit, MaxExploisen, MaxEasy;
    
    
    [System.Serializable]
    public class Difficult
    {
        public int _amountFruit, _maxExploisen, _maxEasy;
    }

    [SerializeField]
    private Difficult[] Diffic;
    
    [ReadOnly(true)]
    public int[] ScoreForLevelUP;

    public int AmountStck
    {
        get { return _amountStck; }
        set
        {
            _amountStck += value;
            EventManager.TriggerEvent("ChangeCountSticks", null);
        }
    }

    [SerializeField]
    private int _amountStck = 10;

    private void ChangeState()
    {
        if (GameManager.Instance.GetCurrentState() is GameProcess)
        {
            GameManager.Instance.ChangeState("LevelUpState", 2f);
        }
        else Timer.Add(5f, ChangeState);
    }
}
