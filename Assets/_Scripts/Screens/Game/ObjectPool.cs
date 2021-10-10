using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPool
{//Power ups Learning...
    private readonly RecyclableObject _prefab;
    private readonly HashSet<RecyclableObject> _instantiateObjects;
    private Queue<RecyclableObject> _recycledObjects;

    public ObjectPool(RecyclableObject prefab)
    {
        _prefab = prefab;
        _instantiateObjects = new HashSet<RecyclableObject>();
    }

    public void Init(int numberOfInitialObjects, Transform transform)
    {
        _recycledObjects = new Queue<RecyclableObject>(numberOfInitialObjects);

        for (var i = 0; i < numberOfInitialObjects; i++)
        {
            var instance = InstantiateNewInstance(transform);
            instance.gameObject.SetActive(false);
            _recycledObjects.Enqueue(instance);
        }
    }

    private RecyclableObject InstantiateNewInstance(Transform transform)
    {
        var instance = Object.Instantiate(_prefab, transform.position, Quaternion.identity);
        instance.Configure(this);
        return instance;
    }

    public T Spawn<T>(Transform transform)
    {
        var recyclableObject = GetInstance(transform);
        _instantiateObjects.Add(recyclableObject);
        recyclableObject.gameObject.SetActive(true);
        recyclableObject.Init();
        return recyclableObject.GetComponent<T>();
    }

    private RecyclableObject GetInstance(Transform transform)
    {
        if (_recycledObjects.Count > 0)
        {
            var recycleObject =_recycledObjects.Dequeue();
            recycleObject.transform.position = transform.position;
            return recycleObject;
        }

        Debug.LogWarning($"Not enough recycled objets for {_prefab.name} consider increase the initial number of objets");
        var instance = InstantiateNewInstance(transform);
        return instance;
    }

    public void RecycleGameObject(RecyclableObject gameObjectToRecycle)
    {
        var wasInstantiated = _instantiateObjects.Remove(gameObjectToRecycle);
        Assert.IsTrue(wasInstantiated, $"{gameObjectToRecycle.name} was not instantiate on {_prefab.name} pool");

        gameObjectToRecycle.gameObject.SetActive(false);
        gameObjectToRecycle.Release();
        _recycledObjects.Enqueue(gameObjectToRecycle);
    }
}
