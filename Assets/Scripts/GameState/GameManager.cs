﻿using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;

//TODO пофиксить лаги Collision Sticks
//TODO добавить обучение


//TODO реализовать разные размеры фруктов
//TODO фрукты для взрыва
//TODO сетка для массового уничтожения
//TODO добавить окончание волны, при которой перечисляется общее количество палок

public class GameManager : SingltoonBehavior<GameManager>
{
       
    [Header("OpenFields")]
    public AState[] States;
    public Dictionary<string, AState> StateDictionary;
    public MaterialBase Colors;
    public DataSession Session;

    [HideInInspector] public PoolManager Pool;
    private AState _activeState;
    private bool _tick = true;

    public bool GameOver;
    public bool NewGame;
    public SystemProcessings ECSWorld;

    protected override void Awake()
    {
        base.Awake();
        Pool = PoolManager.Instance;
        NewGame = true;
        GameOver = false;
    }

    void Start()
    {
        StateDictionary = new Dictionary<string, AState>();
        StateDictionary.Clear();

        ECSWorld = new SystemProcessings();
        ECSWorld.Add<ProcessingTimer>();
        ECSWorld.Add<ProcessingIndicator>();
        
        foreach (var state in States)
        {
            state.Manager = this;
            state.Init();
            StateDictionary.Add(state.GetName(), state);
        }
        
        InstanceState("StartGame");

    }
    
    
    public AState GetCurrentState()
    {
        return _activeState;
    }

    public string GetNameCurrentState()
    {
       return GetCurrentState().GetName();
    }

    public AState FindState(string stateName)
    {   
        AState s;
        if (!StateDictionary.TryGetValue(stateName, out s))
        {
            Debug.Log("This state not find");
            return null;
        }
        return s;
    }

    public void GoToStartMenu()
    {
        ManagerView.Get<BlackAnimationBack>().Curtain(Anim.Close);

        var gameState = FindState("GameState") as GameProcess;
        gameState.ClearScene();
        Time.timeScale = 1;
        GameOver = false;
        NewGame = true;
        ChangeState("StartGame", 0.7f);
    }

    public void Restart()
    {
        ManagerView.Get<BlackAnimationBack>().Curtain(Anim.Close);
        var gameState = FindState("GameState") as GameProcess;
        gameState.ClearScene();
        Time.timeScale = 1;
        NewGame = true;
        GameOver = false;
        ChangeState(null);
        Timer.Add(0.7f, () => InstanceState("GameState"));
    }

    public void InstanceState(string stateName)
    {
        _activeState = FindState(stateName);
        _activeState.Enter();
        Debug.Log("Init state " + _activeState.GetName());
    }
    
    public void ChangeState(string newState, float delayBetweenState = 0)
    {
        if (newState == null)
        {
            _activeState.Exit();
            _activeState = null;
            return;
        }

        _activeState.Exit();
        _activeState = FindState(newState);

        if (delayBetweenState == 0)
        {
            _activeState.Enter();
        }
        else
        {
            Timer.Add(delayBetweenState, _activeState.Enter);
        }

        Debug.Log("Current Game State - " + _activeState);
    }
    
    void Update () {
        
        ECSWorld.Update();

        if(_activeState == null) return;
        _activeState.Tick();
    }
}
