using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.HideAndDontSave;
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this) {
            _instance = null;
        }
    }
}

public class SingletonInInspector<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
           // Destroy(this);
        }
        //override and base.Awake();
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"instance is null!");
            }
            return _instance;
        }
    }
    /*
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }*/
}

public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this);
        }
        //override and base.Awake();
    }
}