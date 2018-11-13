using System;
using System.Collections;
using System.Collections.Generic;
using Homebrew;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * режим на время
 * магазин
 */


//TODO глючит игра при появлении UI

public class GameProcess : AState
{
    private PoolManager _poolManager;
    private bool OpenSecretRoom;
    private GameObject[] _arrayFruit;
    private List<GameObject> _listCritFruit = new List<GameObject>();
    public Queue<GameObject> _queueFruits;
    public AngelsSystem[] RespaunFruit;
    public Canvas CanvasGameProcess;
       
    [Foldout("Settings")] public float StepAngel;                         //Шаг между углами
    [Foldout("Settings")] public float TimeBetweenShot;                   //Задержка между выстрелами 
    [Foldout("Settings")] public float TimeBetweenFruit;
    [Foldout("Settings")] public float TimeBetweenWave;
    [Foldout("Settings")] public float TimeForSecretRoom;
    [Foldout("Settings")] public float StickImmersionDepth;                 //глубина погружения палки в стену


    public DataSession DataSessionGame;
    public LayerMask Mask;


    public override void Init()
    {
        _poolManager = Manager.Pool;
    }

    public override void Enter()
    {

        if (Manager.NewGame)
        {
            Manager.NewGame = false;

            Destroy(Manager.Session);
            Manager.Session = null;
            Manager.Session = Instantiate(DataSessionGame);
            new ProcessingCalculateSticks();
            new ProcessingScore();
            new ProcessingManyFruit();
            new ProcessingHP();

            ManagerView.Get<BlackAnimationBack>().Curtain(Anim.Open);
        }

        CanvasGameProcess.enabled = true;
        WaveFruit(Manager.Session.AmountFruit);
        InputManager.Instance.TouchScreen += ShotStick;

    }

    
    void WaveFruit(int countFruit)
    {
        StartCoroutine(IEnumeratorWaveFruit2(countFruit));
    }

    
    IEnumerator IEnumeratorWaveFruit2(int countFruit)
    {
        Debug.Log("start wave fruits");
        yield return new WaitForSeconds(0.8f);

        _listCritFruit.Clear();
        _queueFruits = new Queue<GameObject>();
        _arrayFruit = new GameObject[countFruit];
        
        for (int i = 0; i < countFruit; i++)
        {
            var obj = _poolManager.GetObject(PoolType.Fruit);
            _queueFruits.Enqueue(obj);
            _arrayFruit[i] = obj;
            yield return null;
        }

        if (ToolsRandom.Choice(1f))
        {
            var critFruit = _poolManager.GetObject(PoolType.CritFruit);
            _queueFruits.Enqueue(critFruit);
            _listCritFruit.Add(critFruit);
        }
        
        yield return null;
        
        for (int b = 0; _queueFruits.Count > 0; b = Random.Range(0,4))
        {
            ManagerSound.Instance.PlayEffect(Track.ShotFruit, Channel.Two);
            
            var amountFruits = 0;
            if (b == 1 || b == 2 && ToolsRandom.Choice(0.3f))
            {
                amountFruits = Random.Range(3, Manager.Session.MaxExploisen);
            }
            
            else amountFruits = Random.Range(1, Manager.Session.MaxEasy);
            
            RespaunFruit[b].ShotFruit(_queueFruits, amountFruits, StepAngel, TimeBetweenFruit);
            
            yield return new WaitForSeconds(TimeBetweenShot);
        }

        _queueFruits.Clear();
        
        Debug.Log("Finish wave");
        WaveFruit(countFruit);
    }
    

    public override void Exit()
    {
        StopAllCoroutines();
        InputManager.Instance.TouchScreen -= ShotStick;

 
            for (int i = 0; i < _arrayFruit.Length; i++)
            {
                var fruit = _arrayFruit[i];
                if(fruit == null) return;
                if (!_poolManager.IsContaine(PoolType.Fruit, fruit) && !fruit.activeInHierarchy)
                {
                    _poolManager.ReturnObject(PoolType.Fruit, fruit);
                }
            }


        foreach (var critFruit in _listCritFruit)
        {
            if(!_poolManager.IsContaine(PoolType.CritFruit, critFruit) && !critFruit.activeInHierarchy) 
                _poolManager.ReturnObject(PoolType.CritFruit, critFruit); 
        }

        foreach (var resp in RespaunFruit)
        {
            resp.StopCorutineAngelSystem();
        }

        CanvasGameProcess.enabled = false;

        if (Manager.GameOver)
        {
            ProcessingHP.Instance.Dispose();
            ProcessingCalculateSticks.Instantiate.Dispose();
            ProcessingScore.Instantiate.Dispose();
            ClearScene();
        }
    }

    public void ClearScene()
    {
        foreach (var o in _arrayFruit)
        {
            if(!_poolManager.IsContaine(PoolType.Fruit, o))
                _poolManager.ReturnObject(PoolType.Fruit, o);
        }

        foreach (var b in _listCritFruit)
        {
            if (!_poolManager.IsContaine(PoolType.CritFruit, b))
                _poolManager.ReturnObject(PoolType.CritFruit, b);
        }
    }

    public void SetEnableShotStick(bool var)
    {
        if (var)
        {
            InputManager.Instance.TouchScreen += ShotStick;
        }
        else InputManager.Instance.TouchScreen -= ShotStick;
    }

    public override void Tick()
    {
        

    }
    
//    public void EnterSecretRoom()
//    {
//        _time = 0;
//        OpenSecretRoom = true;
//        var stateScrolling = Manager.FindState("ScrollBackground") as ScrollBackground;
//        stateScrolling.TypeBackground = TypeBacground.Hidden;
//        GameManager.Instantiate.ChangeState("ScrollBackground");
//        NewTimer.Add(2f, () => ChangeMaterialFruits(Manager.Colors.HiddingFruit));
//    }

//    void ExitSecretRoom()
//    {
//        OpenSecretRoom = false;
//        _time = 0;
//        var stateScrolling = Manager.FindState("ScrollBackground") as ScrollBackground;
//        stateScrolling.TypeBackground = TypeBacground.ExitHidden;
//        GameManager.Instantiate.ChangeState("ScrollBackground");
//        NewTimer.Add(2f, () => ChangeMaterialFruits(Manager.Colors.Colors[0]));
//    }



    void ShotStick(Vector3 direction, Vector3 position)
    {
        if(direction == Vector3.zero) return;

        Manager.Session.AmountStck = -1;
        ManagerSound.Instance.PlayEffect(Track.MoveStick, Channel.Two);
        RaycastHit hit;

        if (!Physics.Raycast(position, -direction, out hit, 1000, Mask)) return;

       // var startPos = hit.point - direction * 3;
        var startPos = hit.point;

        var angel = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var stick = _poolManager.GetObject(PoolType.Stick);
        stick.GetComponent<DataObject>().RotationZ = angel;
        stick.SetTransform(startPos, Quaternion.Euler(0, 0, angel - 90));
    }

    //TODO подобрать оставшийся звук
    //TODO прикрутить рекламу

    public override string GetName()
    {
        return "GameState";
    }
    
}
