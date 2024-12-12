using EditorAttributes;
using UnityEngine;

public class MonoEventSender : MonoBehaviour
{
    public MonoEventReceiver receiver;
    public EventDefine eventType;
    [Button("Send Event")]
    public void SendEvent()
    {
        if(eventType!= EventDefine.None)
        receiver?.Send(eventType);
    }
    
}