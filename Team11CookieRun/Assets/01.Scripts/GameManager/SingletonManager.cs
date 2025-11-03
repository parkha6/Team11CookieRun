using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 싱글턴 instance 생성.
    /// </summary>
    private static T instance;

    /// <summary>
    /// 싱글턴 instance에 자기자신을 할당하고 삭제되지 않게 처리함.
    /// </summary>
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
    /// <summary>
    /// 싱글턴의 instance가 비었을때 재생성
    /// </summary>
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
