using UnityEngine;

public static class MonoBehaviourExtension
{
    // public static void RegisterEvent (this MonoBehaviour component, string eventName)
    // {
    //     if (component == null)
    //         return;
    // }
}

public static class GameObjectExtension
{
    public static void Fire (this GameObject gameObject, string eventName, object value)
    {
        var monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

        foreach (var monoBehaviour in monoBehaviours)
        {
            monoBehaviour.SendMessage(eventName, value);
        }
    }
}