using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseEventArgs : EventArgs
{
    public virtual EventDefine type => EventDefine.None;
    public GameObject Sender = null;
}

public class EntityDeathEventArgs : BaseEventArgs
{
    public override EventDefine type => EventDefine.EntityDied;
    public EntityController killer;
    public EntityController entity;
};
public class EntityDetectedEventArgs : BaseEventArgs
{
    public override EventDefine type => EventDefine.DetectedTarget;
    public EntityController target;
};

public class ObjectPoll<T> where T : new()
{
    private static Dictionary<string, T> pool = new Dictionary<string, T>();


    public static T Get()
    {
        if (!pool.ContainsKey(typeof(T).Name))
        {
            pool.Add(typeof(T).Name, new T());
        }
        return pool[typeof(T).Name];
    }
}