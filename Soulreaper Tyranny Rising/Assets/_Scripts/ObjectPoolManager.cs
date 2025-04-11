using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    Dictionary<string, List<GameObject>> pools;
    public GameObject[] prefabs;
    public int prefabStartAmount = 200;
    public Transform blankTransform;
    bool overflowCreationActive = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        pools = new Dictionary<string, List<GameObject>>();
        
        foreach(var p in prefabs)
        {
            pools.Add(p.name, new List<GameObject>());
            Transform listTransform = Instantiate(blankTransform, transform);
            listTransform.name = p.name + " Pool";
            for(int count = 0;count < prefabStartAmount;++count)
            {
                GameObject o = Instantiate(p);
                o.name = p.name;
                ReturnToPool(o);
            }
        }
    }

    public GameObject SpawnPrefab(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab.name)) // Pool doesn't exist, create one
        {
            var list = new List<GameObject>();
            Transform listTransform = Instantiate(blankTransform, transform);
            listTransform.name = prefab.name + " Pool";
            GameObject o = Instantiate(prefab);
            pools.Add(prefab.name, list);
            // Don't add to list because it's still active
            return o;
        }

        else // Pool exists, get first inactive object
        {
            List<GameObject> list;
            pools.TryGetValue(prefab.name, out list);
            if (list.Count > 0) // There are inactive objects
            {
                GameObject o = list[0]; // Grab first object
                o.SetActive(true);
                list.RemoveAt(0);
                o.transform.SetParent(null); // So we know its not in the pool inside the editor
                return o;
            }
            
            if(!overflowCreationActive)
                StartCoroutine("CreateObjects", prefab);
            return GetNewObject(prefab);
        }
    }

    GameObject GetNewObject(GameObject prefab)
    {
        GameObject o = Instantiate(prefab);
        o.name = prefab.name;
        return o;
    }

    IEnumerator CreateObjects(GameObject prefab)
    {
        overflowCreationActive = true;
        for (int count = 0; count < 30; ++count)
        {
            GameObject o = Instantiate(prefab);
            o.name = prefab.name;
            ReturnToPool(o);
            yield return new WaitForEndOfFrame();
        }

        overflowCreationActive = false;
    }

    public void ReturnToPool(GameObject prefab)
    {
        List<GameObject> list;
        pools.TryGetValue(prefab.name, out list);
        if (list != null)
        {
            list.Add(prefab);
        }
        prefab.transform.SetParent(transform.Find(prefab.name + " Pool"), false);
        prefab.SetActive(false);
    }
}
