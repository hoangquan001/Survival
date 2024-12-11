
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class GravityComponet : EntityComponent
{

    public override void Update()
    {
        if(host.Velocity.y < 0 && host.Grounded)
        {
            host.Velocity.y = -2;
        }
        host.Move(host.Velocity.y * Time.deltaTime * Vector3.up);
    }

}