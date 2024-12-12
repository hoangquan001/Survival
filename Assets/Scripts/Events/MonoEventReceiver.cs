using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// public enum MonoEventType
// {
//     None,
//     Start,
//     Stop,
//     Visible,
//     InVisible
// } 
public class MonoEventReceiver : MonoBehaviour
{
    Dictionary<EventDefine, UnityAction<BaseEventArgs>> events = new Dictionary<EventDefine, UnityAction<BaseEventArgs>>();

    public void AddListener(EventDefine type, UnityAction<BaseEventArgs> listener)
    {
        if (!events.ContainsKey(type)) events.Add(type, null);
        events[type] += listener;
    }

    public void RemoveListener(EventDefine type, UnityAction<BaseEventArgs> listener)
    {
        if (!events.ContainsKey(type)) return;
        events[type] -= listener;  
    }

    public void Send(EventDefine type, BaseEventArgs args = null)
    {
        if(!events.ContainsKey(type)) return;
        events[type]?.Invoke(args);
    }
}