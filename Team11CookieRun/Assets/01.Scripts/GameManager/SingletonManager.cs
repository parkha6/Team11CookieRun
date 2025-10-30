using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { Destroy(gameObject); }
    }
}
