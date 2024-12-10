using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public Vector2 velocity;
    public bool Grounded = false;
    private List<EntityComponent> _ListConponent = new List<EntityComponent>();
    protected NavgationConponent _Nav;

    public bool IsNavigating => _Nav.IsNavigating;
    public void AddComponent(EntityComponent component)
    {
        _ListConponent.Add(component);
        component.Initialization(this);
    }

    public virtual void Start()
    {
        
    }
    public virtual void Awake() {
        // _Nav = GetComponent<NavgationConponent>();
        // AddComponent(_Nav);
      
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
        
    }

    public virtual void TakeDamage(float damage)
    {
        
    }

    // Start is called before the first frame update


    // Update is called once per frame
    public virtual void Update()
    {
        _ListConponent.ForEach(x => x.Update());
    }
}
