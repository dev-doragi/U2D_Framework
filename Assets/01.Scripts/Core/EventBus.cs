using System;
using System.Collections.Generic;

/// <summary>
/// 범용 EventBus - 의존성 분리를 위한 이벤트 발행/구독 시스템
/// 구독은 OnEnable, 해제는 OnDisable/OnDestroy에서 반드시 수행
/// </summary>
public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _events = new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<T>(Action<T> handler) where T : struct
    {
        var type = typeof(T);
        if (!_events.ContainsKey(type))
        {
            _events[type] = new List<Delegate>();
        }
        _events[type].Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        var type = typeof(T);
        if (_events.ContainsKey(type))
        {
            _events[type].Remove(handler);
        }
    }

    public static void Publish<T>(T eventData) where T : struct
    {
        var type = typeof(T);
        if (_events.ContainsKey(type))
        {
            var handlers = _events[type].ToArray(); // 복사하여 중간 수정 방지
            foreach (var handler in handlers)
            {
                if (handler is Action<T> action)
                {
                    action(eventData);
                }
            }
        }
    }
}
