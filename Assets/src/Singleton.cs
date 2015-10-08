using UnityEngine;

//class taken from http://wiki.unity3d.com/index.php/Singleton because i'm lazy -richman

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    protected Singleton() { }

    void Start()
    {
        Debug.Log("Initialised [" + typeof(T) + "]");
    }

    public static T instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    int len = FindObjectsOfType(typeof(T)).Length;
                    if (len > 1)
                    {
                        Debug.LogError("[Singleton] (" + typeof(T) + ") Something went really wrong " +
                            " - there should never be more than 1 singleton! (found " + len + ")" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.LogVerbose("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.LogVerbose("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}