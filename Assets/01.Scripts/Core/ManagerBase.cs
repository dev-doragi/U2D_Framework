using UnityEngine;

/// <summary>
/// 모든 매니저의 베이스 클래스
/// Bootstrapper에 의해 중앙 통제 초기화
/// </summary>
public abstract class ManagerBase<T> : MonoBehaviour, IBootstrapable where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"[ManagerBase] {typeof(T).Name} is not initialized. Ensure Bootstrapper has bootstrapped it.");
                return null;
            }
            return _instance;
        }
    }

    protected bool _isBootstrapped = false;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        // Awake에서는 기본 설정만, 실제 부트스트랩은 BootstrapIfNeeded()에서
    }

    public void BootstrapIfNeeded()
    {
        if (!_isBootstrapped)
        {
            OnBootstrap();
            _isBootstrapped = true;
            Debug.Log($"[ManagerBase] {typeof(T).Name} bootstrapped.");
        }
    }

    protected abstract void OnBootstrap();

    protected void StrictNullCheck(object obj, string message)
    {
        if (obj == null)
        {
            Debug.LogError(message);
            // 로직 중단을 위해 throw 또는 return 사용 (사용자 코드에서 처리)
        }
    }
}
