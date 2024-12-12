
using UnityEngine;

public enum LoopType { LoopRepeat, LoopPingPong, LoopOnce }
[System.Serializable]
public class BootData
{
    public LoopType type = LoopType.LoopOnce;

}
public class Engine : MonoBehaviour
{
    public LoopType Type = LoopType.LoopRepeat;
    public float Duration = 10;
    // public AnimationCurve accelCurve;
    protected int Direction => Invert? 1:-1;
    public bool IsRunning = false;
    public bool Invert = true;

    protected MonoEventReceiver receiver;

    void Awake() {
        receiver = GetComponent<MonoEventReceiver>();
        if(receiver != null) 
        {
            RegisterEvent();
        }
    }

    public bool IsValidIndex(int idx, int count)
    {
        return idx >= 0 && idx < count;
    }
    public virtual void FixedUpdateMove() { 

    }
    public void RegisterEvent() { 
        receiver?.AddListener(EventDefine.StartEngine, OnStart);
        receiver?.AddListener(EventDefine.StopEngine, OnStop);
        receiver?.AddListener(EventDefine.Visible, Visible);
        receiver?.AddListener(EventDefine.InVisible, InVisible);
        // receiver?.AddListener(MonoEventType.Start, OnStart);
        // receiver?.AddListener(MonoEventType.Start, OnStart);
    }

    void FixedUpdate()
    {
        if(IsRunning) FixedUpdateMove();
    }
    // Update is called once per frame



    public virtual void OnStart(BaseEventArgs baseEventArgs)
    {
        IsRunning = true;
    }
    public virtual void OnStop(BaseEventArgs baseEventArgs)
    {
        IsRunning = false;
    }
    public virtual void Visible(BaseEventArgs baseEventArgs)
    {
        this.gameObject.SetActive(true);
    }
    public virtual void InVisible(BaseEventArgs baseEventArgs)
    {
        this.gameObject.SetActive(false);
    }

}

