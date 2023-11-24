using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    public int poolSize;
    public GameObject objectToPool;

    [Tooltip("Check this option if the creation of the pool will not trigger from somewhere else.")]
    public bool createPoolOnAwake;

    private List<GameObject> pool;
    private int poolIndex = 0;
    private GameObject tempObject;

    private void Awake()
    {
        if(createPoolOnAwake)
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
        pool = new List<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            AddNewObjectToPool();
        }
    }

    protected GameObject AddNewObjectToPool()
    {
        tempObject = Instantiate(objectToPool, transform);
        pool.Add(tempObject);
        AddObjectToDictionaries(tempObject);
        tempObject.SetActive(false);
        return tempObject;
    }

    public GameObject GetPooledObject()
    {
        // Check from most likely to least likely to be available. If none of them are available, create a new one.

        for (int i = poolIndex; i < poolSize; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                poolIndex = (poolIndex + 1) % poolSize;
                return pool[i];
            }
        }

        for (int i = 0; i < poolIndex; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                poolIndex = (poolIndex + 1) % poolSize;
                return pool[i];
            }
        }

        poolIndex = 0;
        poolSize++;
        return AddNewObjectToPool();
    }

    // only for TargetableAgent objects
    protected virtual void AddObjectToDictionaries(GameObject _gameObject)
    {

    }

}
