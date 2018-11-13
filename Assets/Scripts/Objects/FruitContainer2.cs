using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class FruitContainer2 : MonoBehaviour {


    public float[] DistanaceBetweenFruit = new float[8];
    public Transform StartingPosition;

   // public List<Collider> _listFruitsCol;
    public List<Vector3> _listPositions;                                        //дистанция между фруктами колеблиться
    public List<Transform> _listFruits;
    
    private bool _isNeedMove;
    public float SpeedMoveFruit;
    private bool _activateEventCriticalUI;
    private int _countFruit;
    private DataObject _dataStick;
    
    public float DistanceBetweenFruittt
    {
        get { return DistanaceBetweenFruit[Random.Range(0, DistanaceBetweenFruit.Length)]; }
    }

    void Awake()
    {
        _dataStick = GetComponent<DataObject>();
        _listFruits = new List<Transform>();
        _listPositions = new List<Vector3>();
       DistanaceBetweenFruit = ToolsRandom.GenerateArrayRandom(4, 1f, 1.2f);
        _isNeedMove = true;
    }


    void Update()
    {
        if(_dataStick.isDestroy) return;

        for (int i = 0; i < _countFruit - 1; i++)
        {
            var startlocalPos = _listFruits[i].transform.localPosition;
            var newPosY = Mathf.MoveTowards(startlocalPos.y, _listPositions[i].y, Time.deltaTime * SpeedMoveFruit);
           _listFruits[i].transform.localPosition = new Vector3(startlocalPos.x, newPosY, startlocalPos.z);
        }
    }

    
    public void Add(Collider col)
    {
       
        if (!_listFruits.Contains(col.transform))
        {
            col.GetComponent<Rigidbody>().useGravity = false;
            col.transform.SetParent(transform);
            col.transform.localPosition = new Vector3(col.transform.localPosition.x, col.transform.localPosition.y, 0f);
            _listPositions.Add(StartingPosition.localPosition);
            _listFruits.Add(col.transform);
            _countFruit += 1;
            
            GameManager.Instance.Session.Score = 1;
            GameManager.Instance.Session.AmountStck = 1;
            
            //шанс получить 10 бонусных очков
            if (!_activateEventCriticalUI && ToolsRandom.Choice(0.1f))
            {
                _activateEventCriticalUI = true;
                GameManager.Instance.Session.Score = 10;
                EventManager.TriggerEvent("ManyFruits", "nice", gameObject);
            }
            
            if (_listFruits.Count > 1)
            {
                for (int i = 0; i < _listFruits.Count - 1; i++)
                {
                    _listPositions[i] = new Vector2(_listPositions[i].x, _listPositions[i].y - DistanceBetweenFruittt);
                }
            }


            if (_listFruits.Count > 2)
            {
                GameManager.Instance.Session.Score = 10;
                GameManager.Instance.Session.AmountStck = 2;
                EventManager.TriggerEvent("ManyStick", null, gameObject);
            }

            if (_listFruits.Count > 3)
            {
                GameManager.Instance.Session.Score = 20;
                EventManager.TriggerEvent("ManyFruits", "perfect", gameObject);
            }
        }
    }
    
//    public void Add(Collider col)
//    {
//        StartCoroutine(EIAdd(col));
//    }
//
//    public IEnumerator EIAdd(Collider col)
//    {
//       
//        if (!_listFruits.Contains(col))
//        {
//            col.GetComponent<Rigidbody>().useGravity = false;
//            col.transform.SetParent(transform);
//            col.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, 0f);
//            _listPositions.Add(StartingPosition.localPosition);
//            _listFruits.Add(col);
//
//            GameManager.Instantiate.Session.Score += 1;
//            EventManager.TriggerEvent("ScoreUpdate", GameManager.Instantiate.Session.Score.ToString(), null);
//            
//            
//            if (_listFruits.Count > 1)
//            {
//                for (int i = 0; i < _listFruits.Count - 1; i++)
//                {
//                    _listPositions[i] = new Vector2(_listPositions[i].x, _listPositions[i].y - DistanceBetweenFruittt);
//                   // _isNeedMove = true;
//                   // NewTimer.Add(1f, delegate { _isNeedMove = false;});
//                }
//            }
//
//            yield return null;
//
//            if (_listFruits.Count > 2 && !_activateEventCriticalUI)
//            {
//                GameManager.Instantiate.Session.Score += 10;
//                EventManager.TriggerEvent("ManyFruits", "nice", gameObject);
//                EventManager.TriggerEvent("ScoreUpdate",  GameManager.Instantiate.Session.Score.ToString(), null);
//                _activateEventCriticalUI = true;
//            }
//
//            if (_listFruits.Count > 3 && !_activateEventCriticalUI)
//            {
//                GameManager.Instantiate.Session.Score += 20;
//                EventManager.TriggerEvent("ManyFruits", "perfect", gameObject);
//                EventManager.TriggerEvent("ScoreUpdate",  GameManager.Instantiate.Session.Score.ToString(), null);
//                _activateEventCriticalUI = true;
//            }
//        }
//    }


    private void OnDisable()
    {
        _isNeedMove = false;
        _activateEventCriticalUI = false;
        _countFruit = 0;
    }

    public void DestroyAll()
    {
        if (_listFruits.Count > 0)
        {
            for (int i = 0; i < _listFruits.Count; i++)
            {
                _listFruits[i].GetComponent<CollisionFruit>().ReturnToPool();
            }
        }

        _listFruits.Clear();
        _listPositions.Clear();
    }
   
}


