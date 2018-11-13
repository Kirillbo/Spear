using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pool
{

    private Object _originalObj;

    private Stack<object> _cashedStack;

    public Transform CommonTransform;

    private int _idPool;

    
    
    public Pool(int id, int amount, Object prefabe, Transform parent = null)
    {
        _cashedStack = new Stack<object>();
        CommonTransform = new GameObject(Enum.GetName(typeof(PoolType), id) + "_POOL").transform;
        _idPool = id;
        _originalObj = prefabe;

        for (int i = 0; i < amount; i++)
        {
            GameObject go = Object.Instantiate(prefabe, Vector3.zero, Quaternion.identity, CommonTransform) as GameObject;
            _cashedStack.Push(go);
            go.SetActive(false);

            IPoollable ipoolable = go.GetComponent<IPoollable>();
            if(ipoolable != null) ipoolable.Init();
        }

        if (parent != null)
        {
            CommonTransform.SetParent(parent);
        }
    }

    public Pool(int id, int amount, GameObject prefabe, Transform parent, bool createCommonTrans)
    {
        _cashedStack = new Stack<object>();
        _idPool = id;
        _originalObj = prefabe;
        
        for (int i = 0; i < amount; i++)
        {
            var go = Object.Instantiate(prefabe, Vector3.zero, Quaternion.identity, parent);
            _cashedStack.Push(go);
            IPoollable ipoolable = go.GetComponent<IPoollable>();
            if(ipoolable != null) ipoolable.Init();
            go.SetActive(false);
        }
    }

    public bool Contains(object obj)
    {
        return _cashedStack.Contains(obj);
    }

    /// <summary>
    /// возвращает прямую ссылку на стек
    /// </summary>
    /// <returns></returns>
    public Stack<object> GetStack()
    {
        return _cashedStack;
    }


    public void AddObject(GameObject obj, bool commonTransform)
    {
        _cashedStack.Push(obj);

        if (commonTransform)
        {
            obj.transform.SetParent(CommonTransform);
        }
    }

    public GameObject GetObject()
    {
        if (_cashedStack.Count < 1)
        {
            Debug.LogFormat("Stack {0} empty.", _idPool);
            return null;
        }
        GameObject b = (GameObject)_cashedStack.Pop();
        return b;
    }

    public GameObject OriginalPrefabe()
    {
        return (GameObject)_originalObj;
    }

}
