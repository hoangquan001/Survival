
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class NavgationConponent : EntityComponent
{
    public NavMeshAgent navMeshAgent;

    public Vector3[] paths;
    public override void Initialization(EntityController host)
    {
        base.Initialization(host);
    }

    public bool IsNavigating = false;

    public void SetNavMeshAgent(NavMeshAgent agent)
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;
        navMeshAgent = agent;
    }
    public void NaviToTarget(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        if (navMeshAgent.CalculatePath(target, path))
        {
            paths = path.corners;
           host.StartCoroutine(Move(paths, 0.1f)); 
        }
    }

    public IEnumerator Move(Vector3[] paths, float speed)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            while ((host.transform.position - paths[i]).magnitude > 0.2f)
            {
               
                host.Move((paths[i] - host.transform.position).normalized * navMeshAgent.speed* Time.deltaTime);
                yield return null;
            }

        }
        yield return null;
    }

    public override void Update()
    {

    }

    public void FixedUpdate()
    {

    }
}