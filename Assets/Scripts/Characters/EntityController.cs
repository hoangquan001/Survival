using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
public delegate void StateChangeEvent(EntityState oldState, EntityState newState);

public enum EntityState { 
    Idle = 0, 
    Walk = 1, 
    Run = 2, 
    Jump = 3, 
    Climb = 6, 
    Falling = 4, 
    Landing = 5, 
    Jumping = 6, 
    Death = 7,
    Attack = 8, 
    GettingUp = 9,
};
public enum AttackState
{
    Idle = 0,
    
}
public class EntityController : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Velocity;
    // [Header("Movement")]
    public float WalkSpeed = 4;
    public float SprintSpeed = 15;
    public float RunSpeed = 10;

    public float Acceleration = 20;

    // [Header("AirBorne")]
    public float JumpForce = 5;
    public float GravityScale = 1f;

    // [Header("Other")]
    public LayerMask GroundLayer = 1;

    public bool Grounded{ get; protected set; }

    protected float Speed {
        get
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                return SprintSpeed;
            }
            return RunSpeed;
        }
    }
    protected EntityState _curState = 0;
    protected Animator m_animator;
    protected MonoEventReceiver m_receiver;
    public StateChangeEvent onStateChange;
    public float Gravity { get { return Physics.gravity.y * GravityScale; } }
    public virtual EntityState CurState
    {
        get => _curState;
        set
        {
            if (_curState == value) return;
            onStateChange?.Invoke(_curState, value);
            _curState = value;
        }
    }

    public virtual void Start()
    {
       
    }
    public virtual void Awake() {
        m_animator = GetComponent<Animator>();
        m_receiver = GetComponent<MonoEventReceiver>();
        if(m_receiver != null)
        {
            RegisterEvents();
        }
    }

    public virtual void Die()
    {
       
    }

    public void Freeze()
    {
        
    }

    public virtual void  Move(Vector3 motion)
    {
        
    }

    public virtual void RegisterEvents()
    {
        onStateChange += OnStateChange;
        onStateChange += SendChangeState;
    }

    private void SendChangeState(EntityState oldState, EntityState newState)
    {
        StateChangeEventArgs changeStateEventArgs = ObjectPoll<StateChangeEventArgs>.Get();
        changeStateEventArgs.oldState = oldState;
        changeStateEventArgs.newState = newState;
        m_receiver.Send(changeStateEventArgs.type, changeStateEventArgs);
    }
    public virtual void TakeDamage(float damage)
    {
        
    }
    protected virtual void OnStateChange(EntityState oldState, EntityState newState)
    {
        if (oldState == newState) return;

        OnStateExit(oldState);
        OnStateEnter(newState);
    }

    protected virtual void OnStateExit(EntityState State)
    {

    }
    protected virtual void OnStateEnter(EntityState State)
    {
      
    }

    protected virtual void OnStateUpdate(EntityState State)
    {
       
    }

    // Start is called before the first frame update


    // Update is called once per frame
    public virtual void Update()
    {
        OnStateUpdate(_curState);
    }
}
