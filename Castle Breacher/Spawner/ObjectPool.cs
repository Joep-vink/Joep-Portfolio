using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Object Pool")]
    //What it is going to spawn
    public GameObject objectToSpawn;

    [Header("Values")]
    public Transform ParentPool;

    [Header("Pool Values")]
    //MaxZise and the currentSize of the pool
    private int currentSize;

    internal object SpawnObject(List<GameObject> summons)
    {
        throw new NotImplementedException();
    }

    [HideInInspector] public bool autoExpand, useActiveObjWhenFull;
    [HideInInspector] public int maxSize;
    private int poolSize = 0;

    //The ObjectPool
    public Queue<GameObject> objectPool;

    private void Awake()
    {
        objectPool = new Queue<GameObject>();

        //Set values if not done correctly
        if (autoExpand) maxSize = -1;
        if (!autoExpand) poolSize = maxSize;
        if (poolSize >= maxSize && !autoExpand) poolSize = maxSize;
    }

    private void Start()
    {
        if (ParentPool == null)
        {
            GameObject go = new GameObject("Pool");
            go.transform.SetParent(GameManager.instance.parentObject);
            ParentPool = go.transform;
        }
    }

    /// <summary>
    /// Manages the object pool : Spawns the object with the right parameters and reuse's it 
    /// </summary>
    public virtual GameObject SpawnObject(GameObject currentObject = null, Transform spawnPos = null)
    {
        //Check if the object is null and set the object
        if (currentObject == null)
            currentObject = objectToSpawn;
        else
        {
            objectPool.Enqueue(currentObject);
            currentObject.SetActive(true);
            return currentObject;
        }

        if (spawnPos == null)
            spawnPos = transform;

        GameObject spawnedObject = GetPooledObject();

        if (poolSize == maxSize && currentSize == poolSize)
        {
            if (useActiveObjWhenFull)
            {
                spawnedObject.SetActive(false);
                spawnedObject.transform.position = spawnPos.position;
                spawnedObject.transform.rotation = Quaternion.identity;
            }
            else if (spawnedObject == null) return null;
            else
            {
                spawnedObject.transform.position = spawnPos.position;
                spawnedObject.transform.rotation = Quaternion.identity;
            }
        }
        else if (spawnedObject == null || autoExpand && spawnedObject == null)
        {
            spawnedObject = Instantiate(currentObject, spawnPos.position, Quaternion.identity);
            spawnedObject.transform.parent = ParentPool;
            spawnedObject.name = currentObject.name + "_" + currentSize;
            currentSize++;
        }
        else
        {
            spawnedObject.transform.position = spawnPos.position;
            spawnedObject.transform.rotation = Quaternion.identity;
        }

        objectPool.Enqueue(spawnedObject);
        spawnedObject.SetActive(true);
        return spawnedObject;
    }

    private GameObject GetPooledObject()
    {
        for (int i = 0; i < ParentPool.childCount; i++)
        {
            if (!ParentPool.GetChild(i).gameObject.activeInHierarchy)
            {
                return ParentPool.GetChild(i).gameObject;
            }
        }

        return null;
    }

    public void DisableAllChildren()
    {
        for (int i = 0; i < ParentPool.childCount; i++)
        {
            ParentPool.GetChild(i).gameObject.SetActive(false);
        }
    }

    #region Editor
#if UNITY_EDITOR

    [CustomEditor(typeof(ObjectPool))]
    public class TestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ObjectPool objPool = (ObjectPool)target;

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);

            EditorGUILayout.Space();

            objPool.autoExpand = GUILayout.Toggle(objPool.autoExpand, "AutoExpand");

            if (!objPool.autoExpand)
            {
                objPool.maxSize = EditorGUILayout.IntField("MaxSize", objPool.maxSize);
                objPool.useActiveObjWhenFull = EditorGUILayout.Toggle("UseActiveObj", objPool.useActiveObjWhenFull);
            }
        }
    }

#endif
    #endregion 
}
