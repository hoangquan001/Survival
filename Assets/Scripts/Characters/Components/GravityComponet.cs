
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class GravityComponet : EntityComponent
{

    public override void Update()
    {
        if(host.velocity.y < 0 && host.Grounded)
        {
            host.velocity.y = -2;
        }
        host.Move(host.velocity.y * Time.deltaTime * Vector3.up);
    }

}