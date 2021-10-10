using UnityEngine;

public abstract class RecyclableObject : MonoBehaviour
{
    private ObjectPool _objectPool;

    internal void Configure(ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    public void Recycle()
    {
        transform.SetParent(transform.root);
        _objectPool.RecycleGameObject(this);
    }
    internal abstract void Init();
    internal abstract void Release();
}