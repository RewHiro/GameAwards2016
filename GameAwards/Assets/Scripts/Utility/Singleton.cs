using UnityEngine;
using System;

public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour
{
    public static Type instancce
    {
        get
        {
            return _instance;
        }
    }

    //------------------------------------------------------------

    protected static Type _instance;

    protected virtual void Awake()
    {
        _instance = FindObjectOfType<Type>();
        if (_instance == null) throw new NullReferenceException(typeof(Type) + "is nothing");
        DontDestroyOnLoad(gameObject);
    }
}