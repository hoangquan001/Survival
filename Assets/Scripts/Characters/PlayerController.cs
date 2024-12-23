using System.Collections;
using DesignPatterns.Utilities;
using EditorAttributes;
using UnityEditor;
using UnityEngine;


public class PlayerData
{
    public float RotationSpeed = 10;
    public float MoveSpeed = 5;

}

public class PlayerController : EntityController
{
    public GunScriptableObject gunScriptableObject;
    private CharacterController m_Controller;
    public float RotationSpeed = 15;
    [Range(0, 1)]
    [SerializeField]
    private float DelayBeforeJump = 0;

    public bool UseRootMotion = true;

    // Start is called before the first frame update

    public override void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        SpawnGun();
    }

    [Button("SpawnGun")]
    public void SpawnGun()
    {
        
        gunScriptableObject.Spawn(transform.RecursiveFind("GunHolder", true),this);
    }
    public override void Update()
    {
        base.Update();
        GroundedCheck();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        var movement = new Vector3(horizontal, 0, vertical);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        Vector3 right = Camera.main.transform.right;
        right.y = 0;

        if (movement != Vector3.zero)
        {
            forward = Quaternion.LookRotation(Vector3.forward * vertical + Vector3.right * horizontal) * forward;
            // movement = forward;
            transform.forward = Vector3.RotateTowards(transform.forward, forward, Time.deltaTime * RotationSpeed, .1f);
        }

        UpdateRun(movement);
        UpdateGravity();


        UpdateAnyState();
        animator.SetInteger("state", (int)_curState);
        animator.SetFloat("speed", Velocity.x);
        animator.SetBool("isGrounded", Grounded);



        HandleWeaponFire();



    }
    /// <summary>
    /// Updates the player's gravity effect based on current state and velocity.
    /// </summary>
    void UpdateGravity()
    {
        // Adjust gravity when player is grounded
        if (Grounded && Velocity.y < 0)
        {
            Velocity.y = -2;
        }
        // Apply gravity when player is in the air
        else
        {
            Velocity.y += Gravity * Time.deltaTime;
            // Clamp the velocity to prevent excessive falling speed
            Velocity.y = Mathf.Clamp(Velocity.y, Physics.gravity.y, -Physics.gravity.y);
        }
        Move(Vector3.up * Velocity.y * Time.deltaTime);
    }
    void UpdateAnyState()
    {
        if (Input.GetKeyUp(KeyCode.Space) && Grounded)
        {
            CurState = EntityState.Jump;

        }
        if (Velocity.y < 0 && !Grounded)
        {
            CurState = EntityState.Falling;
        }

    }
    protected override void OnStateEnter(EntityState State)
    {
        base.OnStateEnter(State);

        switch (State)
        {
            case EntityState.Jump:
                StartCoroutine(Jump());
                break;
            case EntityState.Death:
                // StartCoroutine(Death());
                break;
            case EntityState.Idle:
                break;
        }
    }

    protected override void OnStateExit(EntityState state)
    {
        base.OnStateExit(state);
    }

    protected override void OnStateUpdate(EntityState state)
    {
        base.OnStateUpdate(state);
        switch (state)
        {
            case EntityState.Jump:

                break;
            case EntityState.Falling:
                if (Grounded)
                {
                    CurState = EntityState.Landing;
                }
                break;
            case EntityState.Landing:
                CurState = EntityState.Idle;
                Velocity.x = Speed / 4;
                break;
            case EntityState.Run:
                if(Velocity.x <= 0.0f)
                {
                    CurState = EntityState.Idle;
                }
                break;
            case EntityState.Idle:
                if(Velocity.x > 0)
                {
                    CurState = EntityState.Run;
                }
                break;
        }
    }

    private IEnumerator Idle(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        CurState = (int)EntityState.Idle;
    }
    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(DelayBeforeJump);
        Velocity.y = JumpForce;

    }
    public override void Move(Vector3 motion)
    {
        m_Controller.Move(motion);
    }
    private void FixedUpdate()
    {

    }
    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, 0.2f);
    }
    private void HandleVertical()
    {

    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        GUI.Label(new Rect(0, 0, 300, 24), "RunSpeed: " + Velocity.x, style);
        GUI.Label(new Rect(0, 24, 300, 24), "IsGrounded: " + Grounded, style);
        GUI.Label(new Rect(0, 48, 300, 24), "Velocity: " + Velocity, style);
        GUI.Label(new Rect(0, 72, 300, 24), "RealVelocity: " + realVelocity, style);
    }

    private void UpdateRun(Vector3 movement)
    {
        float scale = 1;
        if (movement.magnitude <= 0)
        {
            scale = -1;
        }
        Velocity.x += Acceleration * scale * Time.deltaTime;
        Velocity.x = Mathf.Clamp(Velocity.x, 0, Speed);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Grounded = Physics.CheckSphere(transform.TransformPoint(m_Controller.center) + Vector3.down * m_Controller.height / 2, m_Controller.radius, GroundLayer, QueryTriggerInteraction.Ignore);
    }
    // Update is called once per frame
    float realVelocity;
    private void OnAnimatorMove()
    {
        Vector3 move = animator.deltaPosition;
        realVelocity = move.magnitude / Time.deltaTime;
        if (UseRootMotion)
        {
            if (!Grounded)
            {
                move = Velocity.x / 2 * transform.forward * Time.deltaTime;
            }
        }
        else
        {
            move = Velocity.x / 2 * transform.forward * Time.deltaTime;
        }
        move.y = 0;
        Move(move);
    }
    [Button]

    public override void Die()
    {
        base.Die();
        CurState = EntityState.Death;
    }
    [Button]

    public  void Revice()
    {
        CurState = EntityState.Death;
    }

    private void HandleWeaponFire()
    {
       if(Input.GetKey(KeyCode.Mouse0))
       {
           gunScriptableObject.TryShot();
       }
    }



}