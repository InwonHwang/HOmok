﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public GameObject prefab
    {
        get
        {
            return _prefab;
        }
        set
        {
            _initialized = true;
            _prefab = value;
        }
    }
    public int count { get; set; }

    public GameObject this[int index]
    {
        get
        {
            if (index < 0 || index >= _actived.Count)
            {
                return null;
            }
           
            return _actived[index];            
        }
    }

    List<GameObject>    _actived = new List<GameObject>();
    List<GameObject>    _deactived = new List<GameObject>();
    GameObject          _prefab;
    bool                _initialized;

    public List<GameObject> CreateObjects(Transform parent, bool active = true)
    {
        if (!_initialized) return null;

        clear();

        int numToCreate = count - _deactived.Count;

        GameObject newObj = null;
        for (int i = 0; i < numToCreate; ++i)
        {
            newObj = GameObject.Instantiate<GameObject>(prefab);
            newObj.name = prefab.name;
            _deactived.Add(newObj);
        }

        for (int i = 0; i < count; ++i)
        {
            _deactived[0].SetActive(active);
            _deactived[0].transform.SetParent(parent);
            _actived.Add(_deactived[0]);
            _deactived.RemoveAt(0);
        }

        return _actived;
    }

    public List<GameObject> AddObject(int c, Transform parent = null)
    {
        if (!_initialized) return null;

        int numToCreate = c - _deactived.Count;

        GameObject newObj;
        for (int i = 0; i < numToCreate; ++i)
        {
            newObj = GameObject.Instantiate<GameObject>(prefab);

            _deactived.Add(newObj);
        }

        for (int i = 0; i < c; ++i)
        {
            _deactived[0].SetActive(true);
            _deactived[0].transform.SetParent(parent);
            _actived.Add(_deactived[0]);
            _deactived.RemoveAt(0);
        }

        count += c;

        return _actived;
    }

    public void SetActive(bool f)
    {
        foreach (var obj in _actived)
        {
            obj.SetActive(false);
        }
    }

    void clear()
    {
        foreach (var obj in _actived)
        {
            obj.SetActive(false);
            _deactived.Add(obj);
        }

        _actived.Clear();
    }
}
