using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public enum RobotState{
    Idle,
    Attack,
    Move,
    Jump
}
public class RobotController : EntityController
{
    // SerializeField
    public GameObject target;
    public Boundary boundary;
    public float AttackRange = 10;
    public float AttackRate = 2;
    public float AttackCountdown = 0;
    
    //Non SerializeField
    private NavMeshAgent agent;

    private Vector3 born;
    public override void Awake() {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        _Nav = new NavgationConponent();
        AddComponent(_Nav);
        _Nav.SetNavMeshAgent(agent);
        born = transform.position;

        _Nav.NaviToTarget(transform.position + transform.forward * 100);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(AttackRange, 2, AttackRange));
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void  Move(Vector3 motion){
        transform.position += motion;
    }


    public bool IsInside(Vector3 position)
    {
        if(boundary == null) return true;
        return boundary.IsInside(position);
    }

    // IEnumerator RandomMove(){
        
    // }
}


