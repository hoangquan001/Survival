
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;


public class LinePlatform : Platform
{
    public List<Transform> Points = new List<Transform>();
    public int StartIdx = 0;

    private void OnEnable()
    {
        transform.position = Points[StartIdx].position;
    }
    private void OnDrawGizmosSelected()
    {

        if (Points.Any(x => x == null))
            return;
        for (int i = 0; i < Points.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Points[i].position, 0.2f);
        }
        for (int i = 0; i < Points.Count; i++)
        {
            Gizmos.color = Color.green;
            if (Type is LoopType.LoopPingPong or LoopType.LoopOnce)
            {
                if (i + 1 >= Points.Count) continue;
            }
            Gizmos.DrawLine(Points[i].position, Points[(i + 1) % Points.Count].position);
        }

    }
    void CalculateIndex(LoopType type)
    {
        switch (type)
        {
            case LoopType.LoopRepeat:
                StartIdx = (StartIdx + Direction) % Points.Count;
                break;
            case LoopType.LoopPingPong:
                // direction = IsValidIndex(StartIdx + direction, Points.Count) ? direction : -direction;
                StartIdx = (StartIdx + Direction) % Points.Count;
                break;
            case LoopType.LoopOnce:
                if (StartIdx + 1 >= Points.Count) IsRuning = false;
                StartIdx = (StartIdx + 1) % Points.Count;
                break;
            default:
                StartIdx = (StartIdx + 1) % Points.Count;
                break;

        }
    }
    public override void FixedUpdateMove()
    {
        if ((transform.position - Points[StartIdx].position).magnitude < 0.01)
        {
            CalculateIndex(Type);
        }
        var next = Vector3.MoveTowards(transform.position, Points[StartIdx].position, Time.fixedDeltaTime * Speed / 10);
        Vector3 detalMove = next - transform.position;
        transform.position = next;
        // UnityEngine.Debug.Log(detalMove.magnitude);

        otherCC?.Move(detalMove);
    }
}
