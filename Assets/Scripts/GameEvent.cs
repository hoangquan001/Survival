using System;
using System.Collections.Generic;
using UnityEngine;
public enum EventType { EntityDied }

public class Singleton<T> 
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)Activator.CreateInstance(typeof(T));
            }
            return instance;
        }
    }
}
public class EntityDeathEventArgs : EventArgs
{
    public EventType type = EventType.EntityDied;
    public IEntity killer;
    public IEntity entity;
};
public class GameEventManager : Singleton<GameEventManager>
{
    public Dictionary<EventType, Action< EventArgs>> events = new Dictionary<EventType, Action< EventArgs>>();
    public void AddListener(EventType type, Action< EventArgs> listener)
    {
        if(!events.ContainsKey(type)) events.Add(type, null);
        events[type] += listener;
    }
    public void RemoveListener(EventType type, Action< EventArgs> listener)
    {
        events[type] -= listener;
    }

    public void TriggerEvent(EventType type, EventArgs args)
    {
        if(events.ContainsKey(type)) events[type]?.Invoke( args);
    }
}