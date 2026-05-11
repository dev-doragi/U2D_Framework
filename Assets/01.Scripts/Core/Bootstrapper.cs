using UnityEngine;
using System.Collections.Generic;

public class Bootstrapper : MonoBehaviour
{
    private static Bootstrapper _instance;
    public static Bootstrapper Instance => _instance;

    [SerializeField] private List<MonoBehaviour> managerPrefabs = new List<MonoBehaviour>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        BootstrapAllManagers();
    }

    private void BootstrapAllManagers()
    {
        // Bootstrap managers from prefabs or scene
        foreach (var prefab in managerPrefabs)
        {
            if (prefab != null)
            {
                var managerObj = Instantiate(prefab, transform);
                if (managerObj is IBootstrapable bootstrapable)
                {
                    bootstrapable.BootstrapIfNeeded();
                }
            }
        }

        Debug.Log("[Bootstrapper] All managers have been bootstrapped successfully.");
    }
}

public interface IBootstrapable
{
    void BootstrapIfNeeded();
}
