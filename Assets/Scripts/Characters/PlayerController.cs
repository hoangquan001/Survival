
using System.Collections;
using UnityEngine;


public class PlayerData
{
    public float RotationSpeed = 10;
    public float MoveSpeed = 5;

}

public enum PlayerState { Idle = 0, Walk = 1, Run = 2, Jump = 3, Climb = 6, Falling = 4, Landing = 5 }
public class PlayerController : EntityController
{
    public static PlayerController Instance { get; private set; }
    CharacterController m_Controller;
    Animator m_animator;

    public float RotationSpeed = 15;
    public float Acceleration = 20;

    float JumpTime = 0;
    public PlayerState playerState = PlayerState.Idle;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    public override void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    public override void Update()
    {
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
            transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * RotationSpeed);
        }

        CalculateRunSpeed(movement);


        m_animator.SetFloat("speed", Velocity.x);
        if (Input.GetKeyUp(KeyCode.V))
        {
            m_animator.SetTrigger("turn180");
        }
        if (Grounded)
        {
            if (Velocity.y < 0)
            {
                Velocity.y = -2;
            }
        }
        else
        {
            Velocity.y += Gravity * Time.deltaTime;
        }
        JumpTime += Time.deltaTime;


        UpdateAnyState();
        UpdateStateMachine();
        m_animator.SetInteger("playerState", (int)playerState);

        m_animator.SetBool("isGrounded", Grounded);
        Move(Vector3.up * Velocity.y * Time.deltaTime);

    }
    void UpdateAnyState()
    {
        if (Input.GetKeyUp(KeyCode.Space) && Grounded)
        {
            playerState = PlayerState.Jump;
            StartCoroutine(Jump());
            JumpTime = 0;
        }

    }
    void UpdateStateMachine()
    {
        switch (playerState)
        {
            case PlayerState.Jump:
                if (Velocity.y < 0 && JumpTime > 0.8)
                {
                    playerState = PlayerState.Falling;
                }
                break;
            case PlayerState.Falling:
                if (Grounded)
                {
                    playerState = PlayerState.Landing;
                }
                break;
            case PlayerState.Landing:
                // playerState = PlayerState.Idle;
                break;
            case PlayerState.Idle:
                break;
        }
    }
    IEnumerator Idle(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        playerState = PlayerState.Idle;
    }
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.16f);
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
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);
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
    }

    private void CalculateRunSpeed(Vector3 movement)
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
        Grounded = Physics.CheckSphere(transform.TransformPoint(m_Controller.center) + Vector3.down * m_Controller.height / 2, m_Controller.radius, 1, QueryTriggerInteraction.Ignore);

    }
    // Update is called once per frame




    float starttime = 0;
    float distance = 0;
    private void OnAnimatorMove()
    {
        Vector3 move = m_animator.deltaPosition;
        Debug.Log("Velocity:" + move.magnitude / Time.deltaTime);
        if (!Grounded)
        {
            move = Velocity.x/2 * transform.forward * Time.deltaTime;
        }
        move.y = 0;
        Move(move);
    }

}
