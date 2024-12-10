
using UnityEngine;

public class EntityComponent
{
    public EntityController host;
    public virtual void Initialization(EntityController host) { 
       this. host = host;
    }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void Update() { }

}