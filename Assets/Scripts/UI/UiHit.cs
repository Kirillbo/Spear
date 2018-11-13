using UnityEngine;

public class UiHit : MonoBehaviour, IPoollable
{

    public PoolType _Pool;
    private MeshRenderer _view;
    private Animator _anim;

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnObject(_Pool, gameObject, true, true);
    }

    public void Init()
    {
        _view = GetComponent<MeshRenderer>();
        _view.enabled = false;
        _anim = GetComponent<Animator>();
        gameObject.SetActive(true);
    }

    public void ReSpawn()
    {
        _view.enabled = true;
        _anim.SetTrigger("PLAY");
    }

    public void Despawn()
    {
        _view.enabled = false;
    }
}
