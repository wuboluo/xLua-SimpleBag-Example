using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this as T;
    }

    protected void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}