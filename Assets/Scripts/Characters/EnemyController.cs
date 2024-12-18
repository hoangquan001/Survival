using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using EditorAttributes;
using UnityEngine;
using UnityEngine.AI;




public class EnemyController : EntityController
{
    // SerializeField
    private EntityController _target;
    public Boundary boundary;
    //Non SerializeField
    private NavMeshAgent agent;
    private float idleTimer = 0;

    public EntityController Target
    {
        get => _target;
        set
        {
            _target = value;
        }
    }

    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
    }

    public override void RegisterEvents()
    {
        base.RegisterEvents();
        m_receiver.AddListener(EventDefine.DetectedTarget, OnDetectedTarget);

    }

    private void OnDetectedTarget(BaseEventArgs arg0)
    {
        EntityDetectedEventArgs entityDetectedEventArgs = arg0 as EntityDetectedEventArgs;
        _target = entityDetectedEventArgs.target;
        agent.SetDestination(_target.transform.position);
    }

    protected override void OnStateChange(EntityState oldState, EntityState newState)
    {
        base.OnStateChange(oldState, newState);
    }

    protected override void OnStateExit(EntityState enemyState)
    {
        // EnemyState state = (EnemyState)enemyState;

    }
    protected override void OnStateEnter(EntityState state)
    {
        switch (state)
        {
            case EntityState.Jump:
                agent.isStopped = false;
                StartCoroutine(HandleJumpOnOffMeshLink(agent.currentOffMeshLinkData));
                break;
        }
    }

    protected override void OnStateUpdate(EntityState state)
    {
        switch (state)
        {
            case EntityState.Idle:
                idleTimer -= Time.deltaTime;
                Velocity.x = 0;
                if (idleTimer <= 0)
                {
                    RandomMove();
                    idleTimer = Random.Range(2f, 5f);
                }
                break;
            case EntityState.Attack:
                break;
            case EntityState.Run:
                agent.speed = Speed;
                if (agent.remainingDistance < agent.stoppingDistance || agent.isPathStale)
                {
                    agent.ResetPath();
                    CurState = (int)EntityState.Idle;
                    break;
                }
                Velocity.x = (agent.nextPosition - transform.position).magnitude / Time.deltaTime;
                transform.position = agent.nextPosition;
                break;
            case EntityState.Jump:
                break;

        }
    }


    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1))
            // {
            //     agent.SetDestination(hit.point);
            // }
        }
        if (agent.hasPath)
        {
            if (agent.isOnOffMeshLink)
            {
                if (CurState == EntityState.Jump) return;
                CurState = EntityState.Jump;
            }
            else
            {
                CurState = EntityState.Run;
            }
        }
        m_animator.SetFloat("velocityX", Velocity.x);

    }

    public IEnumerator HandleJumpOnOffMeshLink(OffMeshLinkData linkData)
    {
        Physics.Raycast(linkData.startPos + Vector3.up, Vector3.down, out RaycastHit hit, Mathf.Infinity, 1);
        Physics.Raycast(linkData.endPos + Vector3.up, Vector3.down, out RaycastHit hit2, Mathf.Infinity, 1);
        float height = hit.point.y - hit2.point.y;
        if (height > 2f)
        {
            agent.isStopped = false;
            yield break;
        }
        Vector3 start = hit.point;
        Vector3 end = hit2.point;
        transform.forward = end - start;
        Velocity.x = 10;
        Velocity.y = JumpForce;
        while ((transform.position - end).magnitude > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime * Velocity.x);
            Velocity.y += Gravity * Time.deltaTime;
            transform.position += Vector3.up * Velocity.y * Time.deltaTime;
            yield return null;
        }
        transform.position = end;
        agent.isStopped = false;
    }
    [Button("Sync Attribute")]
    public void SyncAttribute()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = RunSpeed;
    }
    public override void Move(Vector3 motion)
    {
        transform.position += motion;
    }


    public bool IsInside(Vector3 position)
    {
        if (boundary == null) return true;
        return boundary.IsInside(position);
    }

    void RandomMove()
    {
        if (boundary == null)
        {
            return;
        }
        Vector3 randomPoint = boundary.RandomPointInBoundary();
        agent.SetDestination(randomPoint);
    }
}
