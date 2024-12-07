using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum MonoEventType
{
    None,
    Start,
    Stop,
    Visible,
    InVisible
} 
public class MonoEventReceiver : MonoBehaviour
{
    Dictionary<MonoEventType, UnityAction> events = new Dictionary<MonoEventType, UnityAction>();

    public void AddListener(MonoEventType type, UnityAction listener)
    {
        if (!events.ContainsKey(type)) events.Add(type, null);
        events[type] += listener;
    }

    public void RemoveListener(MonoEventType type, UnityAction listener)
    {
        if (!events.ContainsKey(type)) return;
        events[type] -= listener;  
    }

    public void Send(MonoEventType type)
    {
        events[type]?.Invoke();
    }
}