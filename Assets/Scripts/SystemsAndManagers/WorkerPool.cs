using System.Collections;
using System.Collections.Generic;
using Homebrew;
using UnityEngine;

public class WorkerPool : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject PrefabStick;
    public GameObject[] ArrPreabFruits;

    [Space(10)]
    public GameObject PrefabUIHiTPerfect;
    public GameObject PrefabUiHit;
    public GameObject PrefabUIAmountStick;
    public GameObject PrefabCrit;
    public GameObject PrefabeIndicator;

    [Foldout("Amount Prefabs")]
    public int CountUIHit, CountFruit, CountStick, CountParticle, CountCrit;

    public GameObject PrefabeFX;
    private PoolManager Pool;

    void Awake() { 

        Pool = PoolManager.Instance;
        Pool.CreatePool(PoolType.Particles, CountParticle, PrefabeFX);
        Pool.CreatePool(PoolType.Stick, CountStick, PrefabStick);
        Pool.CreatePool(PoolType.UIHitNormal, CountUIHit, PrefabUiHit);
        Pool.CreatePool(PoolType.UIHitPerfect, CountUIHit, PrefabUIHiTPerfect);
        Pool.CreatePool(PoolType.CritFruit, CountCrit, PrefabCrit);
        Pool.GetObject(PoolType.CritFruit);
        Pool.CreatePool(PoolType.Fruit, 0, null);
        Pool.CreatePool(PoolType.Indicator, 3, PrefabeIndicator);
        Pool.CreatePool(PoolType.UIHItSticks, 2, PrefabUIAmountStick);
        
        for (int i = 0; i<ArrPreabFruits.Length; i++)
        {
            Pool.AddObject(PoolType.Fruit, ArrPreabFruits[i], CountFruit);
        }

        Pool.MixObjectInPool(PoolType.Fruit);

    }

}
