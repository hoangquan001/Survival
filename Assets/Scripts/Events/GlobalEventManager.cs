using System;
using System.Collections.Generic;
using UnityEngine;
public enum EventDefine { EntityDied, EngineStarted }

public class BaseEventArgs : EventArgs { 
    public GameObject Sender = null;
}
public class EntityDeathEventArgs : BaseEventArgs
{
    public EventDefine type = EventDefine.EntityDied;
    public IEntity killer;
    public IEntity entity;
};
public class GlobalEventManager : XSingleton<GlobalEventManager>
{
    // public Dictionary<EventDefine, Action< EventArgs>> events = new Dictionary<EventDefine, Action< EventArgs>>();
    public Dictionary<int, Dictionary<EventDefine, Action<BaseEventArgs>>> events = new Dictionary<int, Dictionary<EventDefine, Action<BaseEventArgs>>>();
    public void AddListener(EventDefine type, Action<BaseEventArgs> listener, GameObject firer = null)
    {
        int id = firer == null? 0 : firer.GetInstanceID();
        if (!events.ContainsKey(id)) events.Add(id, new Dictionary<EventDefine, Action<BaseEventArgs>>());

        if (!events[id].ContainsKey(type)) events[id].Add(type, null);
        events[id][type] += listener;
    }
    // public void RemoveListener(EventDefine type, Action< BaseEventArgs> listener)
    // {
    //     events[type] -= listener;
    // }

    public void Fire(EventDefine type, BaseEventArgs args)
    {
        events[0][type]?.Invoke(args);
        if(args.Sender != null)
        {
            int id = args.Sender.GetInstanceID();
            events[id][type]?.Invoke(args);
        }
    }
}