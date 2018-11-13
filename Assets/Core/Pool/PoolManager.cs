using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingltoonBehavior<PoolManager>
{

    public bool DynamicPool;
    
	private Dictionary<int, Pool>_pools = new Dictionary<int, Pool>();

    public void CreatePool(PoolType id, int amount, GameObject prefabe)
    {
        Pool pool;
        int IntID = (int) id;

        if(!_pools.ContainsKey(IntID))
        {
            pool = new Pool(IntID, amount, prefabe, transform);
            _pools.Add(IntID, pool);
        }

        else Debug.Log(id + " pool already exist");
    }

    public void CreatePool(PoolType id, int amount, GameObject prefabe, Transform parent, bool createCommonTrasform)
    {
        Pool pool;
        int IntID = (int)id;

        if (_pools.ContainsKey(IntID) == false)
        {
            pool = new Pool(IntID, amount, prefabe, parent, createCommonTrasform);
            _pools.Add(IntID, pool);
        }
    }

    public object[] GetObjects(PoolType id, int count)
    {
        int curentCount = 0;
        GameObject[] arr = new GameObject[count];

        while (curentCount < count)
        {
            arr[curentCount] = _pools[(int) id].GetObject();
            curentCount++;
        }

        return arr;
    }

    public bool IsContaine(PoolType id, GameObject obj)
    {
        return _pools[(int) id].Contains(obj);
    }

    public void CheckConsole()
    {
        Debug.Log("Check console");
    }

    public Stack<object> GetStack(PoolType id)
    {
        if (_pools.ContainsKey((int) id))
        {
            return _pools[(int) id].GetStack();
        }

        Debug.Log("Pool not exist");
        return null;
    } 

    public GameObject GetObject(PoolType id)
    {
        var obj = _pools[(int) id].GetObject();
        if (obj == null)
        {
            Debug.LogFormat("Pool {0} is empty.", id);

            if (DynamicPool)
            {
                obj = Instantiate(_pools[(int) id].OriginalPrefabe());
                IPoollable IPoollabl = obj.GetComponent<IPoollable>();
                if(IPoollabl != null) IPoollabl.Init();

                Debug.LogFormat("Add one object to {0} pool.", id );
            }
        }

        IPoollable iPoollable = obj.GetComponent<IPoollable>();
        if(iPoollable != null) iPoollable.ReSpawn();
        
        return obj;
    }




#if UNITY_EDITOR
    
    public string[] ListPool;
    
    void Update()
    {
                
        for (int i = 0; i < _pools.Count; i++)
        {
            var key  = _pools.ElementAt(i).Key;
            var namePool = Enum.GetName(typeof(PoolType), i);
            var countElementInPool = _pools[i].GetStack().Count;
            ListPool[i] = String.Concat(namePool, " ", countElementInPool);
        }
    }

#endif


    public void ReturnObject(PoolType id, GameObject obj, bool commonTransform = true, bool setActive = false)
    {
        Pool pool;

        if(_pools.TryGetValue((int)id, out pool))
        {
            pool.AddObject(obj, commonTransform);

            var ipoolable = obj.GetComponent<IPoollable>();
            if (ipoolable != null) ipoolable.Despawn();

            obj.SetActive(setActive);
        }

        else Debug.LogFormat("{0} pool does not exist", id);
    }

   // public void 

    /// <summary>
    /// добавить объект к уже существующему пулу
    /// </summary>
    /// <param name="id"></param>
    /// <param name=""></param>
    public void AddObject(PoolType id, GameObject prefab, int count)
    {
        Pool pool = null;

        if (_pools.TryGetValue((int) id, out pool))
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                IPoollable ipoolable = obj.GetComponent<IPoollable>();
                if(ipoolable != null) ipoolable.Init();
                
                pool.AddObject(obj, true);
            }
        }

        else Debug.Log("Pool is not find");
    }

    /// <summary>
    /// перемешать объекты в стеке
    /// </summary>
    public void MixObjectInPool(PoolType id)
    {
        Pool pool;
        if (_pools.TryGetValue((int) id, out pool))
        {
            var arr = GetStack(id).ToArray();
            GetStack(id).Clear();
            System.Random rnd = new System.Random();
            foreach (var value in arr.OrderBy(x => rnd.Next()))
                GetStack(id).Push(value); 
        }
        else Debug.Log(id + " pool not exist");
    }
}

public enum PoolType
{
    CritFruit,
    Fruit,
    Stick,
    Particles,
    UIHitNormal,
    UIHitPerfect,
    Indicator,
    UIHItSticks
}
