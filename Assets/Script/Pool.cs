using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private GameObject prefab;
    private List<GameObject> pool;
    
    public Pool(GameObject prefab)
    {
        this.prefab = prefab;
        pool = new List<GameObject>();
    }

    public void LoadPool(int poolsize)
    {
        for (int i = 0; i < poolsize; i++)
        {
            pool.Add(GenerateObject());
        }
    }  

    public GameObject GetInactiveObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeSelf) return pool[i];
        }
        //if we've gotten here, they are all being used. EXPAND!!
        GameObject go = GenerateObject();
        pool.Add(go);
        return go;
    }
            
    public void DeactivatePoolObjects()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].gameObject.activeSelf) pool[i].gameObject.SetActive(false);
        }
    }

    private GameObject GenerateObject()
    {
        GameObject go = GameObject.Instantiate(prefab);
        go.SetActive(false);
        return go;
    }

}
