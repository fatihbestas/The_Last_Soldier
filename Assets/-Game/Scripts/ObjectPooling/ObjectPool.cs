using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    public int poolSize;
    public GameObject objectToPool;

    [Tooltip("Check this option if the creation of the pool will not trigger from somewhere else.")]
    public bool createPoolOnAwake;

    private List<T> pool;
    private int poolIndex = 0;
    private GameObject tempObject;

    private void Awake()
    {
        if (createPoolOnAwake)
            CreatePool();
    }

#if UNITY_EDITOR

    private void Start()
    {
        if (poolSize <= 0)
        {
            Debug.LogError("Pool size error. Object Name: " + gameObject.name);
        }

        if (objectToPool == null)
        {
            Debug.LogWarning("objectToPool error. Object Name:" + gameObject.name);
        }
    }

#endif

    public void CreatePool()
    {
        if (objectToPool.GetComponent<T>() == null)
        {
            throw new System.Exception($"{objectToPool.name} does not have a component type of {typeof(T)}");
        }

        pool = new List<T>();

        for (int i = 0; i < poolSize; i++)
        {
            AddNewObjectToPool();
        }
    }

    protected T AddNewObjectToPool()
    {
        tempObject = Instantiate(objectToPool, transform);
        T component = tempObject.GetComponent<T>();
        pool.Add(component);
        AddObjectToDictionary(tempObject);
        tempObject.SetActive(false);
        return component;
    }

    public T GetPooledComponent()
    {
        // Check from most likely to least likely to be available. If none of them are available, create a new one.

        for (int i = poolIndex; i < poolSize; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                poolIndex = (poolIndex + 1) % poolSize;
                return pool[i];
            }
        }

        for (int i = 0; i < poolIndex; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                poolIndex = (poolIndex + 1) % poolSize;
                return pool[i];
            }
        }

        poolIndex = 0;
        poolSize++;
        return AddNewObjectToPool();
    }

    public GameObject GetPooledGameObject()
    {
        return GetPooledComponent().gameObject;
    }

    public void SendAllObjectsToPool()
    {
        foreach (T component in pool)
        {
            component.gameObject.SetActive(false);
        }
    }

    // only for TargetableAgent objects
    protected virtual void AddObjectToDictionary(GameObject gameObject)
    {

    }

}
