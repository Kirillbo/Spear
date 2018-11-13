using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class FruitContainer : MonoBehaviour
{


    private Vector3 _localPositionChild;
    public float[] DistanaceBetweenFruit = new float[8];
    public Transform StartingPosition;
    private Vector3 _previousFruit;

    private List<Collider> _listFruits;
    private List<Vector3> _listPositions;                                        //дистанция между фруктами колеблиться

    private bool _isNeedMove = true;
    public float SpeedMoveFruit;
    private int _countFruitOnSticks;

    public float DistanceBetweenFruittt
    {
        get { return DistanaceBetweenFruit[Random.Range(0, DistanaceBetweenFruit.Length)]; }
    }

    void Awake()
    {
        //  _gameState = GameManager.Instantiate.FindState("GameState") as GameProcess;
        _listFruits = new List<Collider>();
        _listPositions = new List<Vector3>();
        DistanaceBetweenFruit = ToolsRandom.GenerateArrayRandom(8, 0.9f, 1.15f);
    }

    private float aceleration;

    void FixedUpdate()
    {
        if (_isNeedMove)
        {
            for (int i = 0; i < _listFruits.Count; i++)
            {
                var startlocalPos = _listFruits[i].transform.localPosition;
                var newPos = Vector3.MoveTowards(startlocalPos, _listPositions[i], Time.deltaTime * SpeedMoveFruit);
                _listFruits[i].transform.localPosition = new Vector3(startlocalPos.x, newPos.y, startlocalPos.z);
            }

        }
    }


    public void Add(Collider col)
    {
        
        if (!_listFruits.Contains(col))
        {
            col.GetComponent<Rigidbody>().useGravity = false;
           // col.transform.SetParent(transform);

            _listPositions.Add(StartingPosition.localPosition);
            _listFruits.Add(col);

            if (_listFruits.Count > 1)
            {
                for (int i = 0; i < _listFruits.Count - 1; i++)
                {
                    _listPositions[i] = new Vector2(_listPositions[i].x, _listPositions[i].y - DistanceBetweenFruittt);
                }
            }

            if (_listFruits.Count > 2)
            {
                // _gameState.ALotFruits(transform.position);
            }
        }

    }



    public void DestroyAll()
    {
        for (int i = 0; i < _listFruits.Count; i++)
        {
            _listFruits[i].GetComponent<CollisionObject>().ReturnToPool();
        }

        _listFruits.Clear();
        _listPositions.Clear();
        GameManager.Instance.Pool.ReturnObject(PoolType.Stick, gameObject);
    }

}


