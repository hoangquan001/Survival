
using System.Collections;
using UnityEngine;


public class PlayerData
{
    public float RotationSpeed = 10;
    public float MoveSpeed = 5;

}

public enum PlayerState { Idle = 0, Walk=1, Run=2, Jump=3, Climb=6, Falling=4, Landing=5 }
public class PlayerController : MonoBehaviour
{
    CharacterController m_Controller;
    Animator m_animator;
    [Range(4, 15)]
    public float MaxSpeed = 5;
    public float RotationSpeed = 15;
    public float Acceleration = 20;
    public float JumpForce = 5;
    public bool Grounded = false;
    Vector2 velocity = Vector2.zero;

    float JumpTime = 0;
    public PlayerState playerState = PlayerState.Idle;
    public float GravityScale = 1;
    public float Gravity { get { return Physics.gravity.y * GravityScale; } }
    // Start is called before the first frame update
    private void Start()
    {
        m_Controller = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    private void Update()
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
        

        m_animator.SetFloat("speed", velocity.x);
        if (Input.GetKeyUp(KeyCode.V))
        {
            m_animator.SetTrigger("turn180");
        }
        if(Grounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2;
            }
        }
        else
        {
            velocity.y += Gravity * Time.deltaTime;
        }
        JumpTime += Time.deltaTime;

    
        UpdateAnyState();
        UpdateStateMachine();
        m_animator.SetInteger("playerState", (int)playerState);

        m_animator.SetBool("isGrounded", Grounded);
        Move();

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
                if (velocity.y < 0 && JumpTime > 0.8)
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
        // velocity.y = JumpForce;
        
    }
    private void Move()
    {
        m_Controller.Move( Vector3.up * velocity.y * Time.deltaTime);
        // m_Controller.Move(transform.forward * velocity.x / 2 * Time.deltaTime);
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
        GUI.Label(new Rect(0, 0, 300, 24), "RunSpeed: " + velocity.x, style);
        GUI.Label(new Rect(0, 24, 300, 24), "IsGrounded: " + Grounded, style);
        GUI.Label(new Rect(0, 48, 300, 24), "Velocity: " + velocity, style);
    }

    private void CalculateRunSpeed(Vector3 movement)
    {
        float scale = 1;
        if (movement.magnitude <= 0)
        {
            scale = -1;
        }
        velocity.x += Acceleration * scale * Time.deltaTime;
        velocity.x = Mathf.Clamp(velocity.x, 0, MaxSpeed);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Grounded = Physics.CheckSphere(transform.TransformPoint(m_Controller.center) + Vector3.down * m_Controller.height/2, m_Controller.radius, 1, QueryTriggerInteraction.Ignore);

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
            move = velocity.x/2 * transform.forward * Time.deltaTime;
        }
            move.y = 0;
        m_Controller.Move(move);
    }

}
