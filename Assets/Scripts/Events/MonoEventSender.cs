using EditorAttributes;
using UnityEngine;

public class MonoEventSender : MonoBehaviour
{
    public MonoEventReceiver receiver;
    public MonoEventType eventType;
    [Button("Send Event")]
    public void SendEvent()
    {
        if(eventType!= MonoEventType.None)
        receiver?.Send(eventType);
    }
    
}