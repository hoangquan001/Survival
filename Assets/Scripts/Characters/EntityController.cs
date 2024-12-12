using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Velocity;
    public bool Grounded{ get; protected set; }
    public float GravityScale = 1f;
    public float Speed = 10;
    public float JumpForce = 5;
    protected int _curState = 0;
    protected Animator m_animator;
    protected MonoEventReceiver m_receiver;
    public event StateChangeEvent onStateChange;
    public float Gravity { get { return Physics.gravity.y * GravityScale; } }
    public virtual int CurState
    {
        get => _curState;
        set
        {
            if (_curState == value) return;
            onStateChange?.Invoke(_curState, (int)value);
            _curState = (int)value;
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
    }

    public virtual void TakeDamage(float damage)
    {
        
    }
    protected virtual void OnStateChange(int oldState, int newState)
    {
        if (oldState == newState) return;

        OnStateExit(oldState);
        OnStateEnter(newState);
    }

    protected virtual void OnStateExit(int State)
    {
        EnemyState state = (EnemyState)State;

    }
    protected virtual void OnStateEnter(int State)
    {
      
    }

    protected virtual void OnStateUpdate(int State)
    {
       
    }

    // Start is called before the first frame update


    // Update is called once per frame
    public virtual void Update()
    {
        OnStateUpdate(_curState);
    }
}
