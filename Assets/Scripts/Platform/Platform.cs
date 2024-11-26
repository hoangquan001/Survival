
using UnityEngine;

public enum LoopType { LoopRepeat, LoopPingPong, LoopOnce }

public class Platform : MonoBehaviour
{
    public LoopType Type = LoopType.LoopRepeat;
    public float Speed = 10;
    protected int Direction => Invert? 1:-1;
    public bool IsRuning = true;
    public bool Invert = true;

    protected CharacterController otherCC;
    void Start()
    {

    }

    public bool IsValidIndex(int idx, int count)
    {
        return idx >= 0 && idx < count;
    }
    public virtual void FixedUpdateMove() { 

    }

    void FixedUpdate()
    {
        if(IsRuning) FixedUpdateMove();
    }
    // Update is called once per frame

    private void OnTriggerEnter(Collider other) {
        otherCC = other.GetComponent<CharacterController>();
    }
    private void OnTriggerExit(Collider other) {
        otherCC = null;
    }
}

