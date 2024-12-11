using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public enum RobotState
{
    Idle,
    Attack,
    Move,
    Jump
}
public class EnemyController : EntityController
{
    // SerializeField
    private EntityController _target;
    public EntityController Target {
        get => _target;
        set {
            _target = value;
        }
    }
    public Boundary boundary;
    //Non SerializeField
    private NavMeshAgent agent;

    private bool _isOnOffMeshLink = false;

    private float idleTimer = 0;
    RobotState robotState = RobotState.Idle;

    private Vector3 born;
    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        
        // _Nav = new NavgationConponent();
        // AddComponent(_Nav);
        // _Nav.SetNavMeshAgent(agent);
        // born = transform.position;

        // _Nav.NaviToTarget(transform.position + transform.forward * 100);
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(transform.position, new Vector3(AttackRange, 2, AttackRange));
    // }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1))
            {
                agent.SetDestination(hit.point);
            }
        }
        if (agent.hasPath)
        {
            if (agent.isOnOffMeshLink)
            {
                if (_isOnOffMeshLink) return;
                agent.isStopped = true;
                _isOnOffMeshLink = true;
                robotState = RobotState.Jump;
                StartCoroutine(HandleJumpOnOffMeshLink(agent.currentOffMeshLinkData));

            }
            else
            {
                _isOnOffMeshLink = false;
                transform.position = agent.nextPosition;
                Velocity = agent.velocity;
                robotState = RobotState.Move;
            }
        }

       switch (robotState)
       {
           case RobotState.Idle:
               idleTimer -= Time.deltaTime;
               if (idleTimer <= 0) {
                    RandomMove();
                    idleTimer = Random.Range(2f, 5f);
               }
               break;
           case RobotState.Attack:
               break;
           case RobotState.Move:
               if(agent.remainingDistance < 0.5f) {
                   robotState = RobotState.Idle;
               }
               break;
           case RobotState.Jump:
               break;

       }

    }
    public IEnumerator HandleJumpOnOffMeshLink(OffMeshLinkData linkData)
    {
        Physics.Raycast(linkData.startPos+Vector3.up, Vector3.down, out RaycastHit hit, Mathf.Infinity, 1);
        Physics.Raycast(linkData.endPos+Vector3.up, Vector3.down, out RaycastHit hit2, Mathf.Infinity, 1);
        float height = hit.point.y - hit2.point.y;
        if(height >2f) {
            agent.isStopped = false;
            yield break;
        }
        Vector3 start = hit.point;
        Vector3 end = hit2.point;
        transform.forward = end - start;
        Velocity.x = agent.speed;
        Velocity.y = JumpForce;
        while ((transform.position - end).magnitude > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, Time.deltaTime* Velocity.x);
            Velocity.y += Gravity * Time.deltaTime;
            transform.position += Vector3.up * Velocity.y * Time.deltaTime;
            yield return null;
        }
        transform.position = end;
        agent.isStopped = false;
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

    void RandomMove(){
        if(boundary == null) 
        {
            return;
        }
        Vector3 randomPoint = boundary.RandomPointInBoundary();
        agent.SetDestination( randomPoint);
    }
}
