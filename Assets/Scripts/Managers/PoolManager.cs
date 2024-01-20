using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct VFXPool
{
    public List<Transform> pool;
    public List<int> poolSize;
}

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance { get { return instance; } }

    [SerializeField]
    private VFXPool _vfxPool;

    private Dictionary<Transform, List<Transform>> m_pooledObjects = new Dictionary<Transform, List<Transform>>();

    //private string debugString = "Creating new pool obj of ";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        for (int i = 0; i < _vfxPool.pool.Count; ++i)
        {
            m_pooledObjects[_vfxPool.pool[i]] = new List<Transform>();

            for (int j = 0; j < _vfxPool.poolSize[i]; ++j)
            {
                Transform newObj = Instantiate(_vfxPool.pool[i], transform);
                m_pooledObjects[_vfxPool.pool[i]].Add(newObj);
                newObj.gameObject.SetActive(false);
            }
        }

        _vfxPool.pool.Clear();
        _vfxPool.poolSize.Clear();
    }

    public Transform Spawn(Transform prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        Transform newObj = m_pooledObjects[prefab][0];

        if (newObj.gameObject.activeSelf)
        {
            for (int i = 1; i < m_pooledObjects[prefab].Count; ++i)
            {
                if (!m_pooledObjects[prefab][i].gameObject.activeSelf)
                {
                    newObj = m_pooledObjects[prefab][i];
                    m_pooledObjects[prefab].RemoveAt(i);
                    m_pooledObjects[prefab].Add(newObj);
                    break;
                }
            }
        }
        else
        {
            m_pooledObjects[prefab].RemoveAt(0);
            m_pooledObjects[prefab].Add(newObj);
        }

        //if still is active that means it didnt find one
        //so create a new one
        if (newObj.gameObject.activeSelf)
        {
            //GSRDebug.Log(debugString + prefab.name);
            newObj = Instantiate(prefab);

            m_pooledObjects[prefab].Add(newObj);
        }

        if (parent)
        {
            newObj.SetParent(parent, false);
        }
        newObj.localPosition = position;
        newObj.localRotation = rotation;
        newObj.localScale = prefab.localScale;

        newObj.gameObject.SetActive(true);

        return newObj;
    }

    public void Despawn(Transform obj)
    {
        obj.SetParent(transform);
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.gameObject.SetActive(false);
    }
}
