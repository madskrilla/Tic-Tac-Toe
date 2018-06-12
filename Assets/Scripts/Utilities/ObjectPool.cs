using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject pooledObject;
    [SerializeField]
    private int poolSize = 100;

    private List<GameObject> pooledObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        //Fill the pool
        for (int i = 0; i < poolSize; i++)
        {
            AddObjectToPool();
        }
    }

    private void AddObjectToPool()
    {
        GameObject item = GameObject.Instantiate(pooledObject);
        item.transform.parent = transform;
        item.transform.hideFlags = HideFlags.HideInHierarchy;
        item.SetActive(false);
        pooledObjects.Add(item);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetObject()
    {
        GameObject obj = pooledObjects.Find(x => x.activeSelf == false);
        if (obj == null)
        {
            AddObjectToPool();
            obj = pooledObjects.Find(x => x.activeSelf == false);
        }
        obj.SetActive(true);
        return obj;
    }
}
