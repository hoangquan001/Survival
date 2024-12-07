using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using SingularityGroup.HotReload;
using UnityEngine;


public class PlayerData
{
    public float RotationSpeed = 10;
    public float MoveSpeed = 5;

}
public class PlayerController : MonoBehaviour
{
    CharacterController m_CharacterController;
    Animator m_animator;
    [Range(5, 15)]
    public float MaxSpeed = 5;
    public float RotationSpeed = 15;
    public float Acceleration = 20;
    public float MaxJumpSpeed = 0;
    public bool Grounded = false;
    Vector2 velocity = Vector2.zero;

    float JumpTime = 0;
    float jumpSpeed = 0;
    [HideInInspector]
    public Vector3 detalMove = Vector3.zero;
    public float Gravity = -2;
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
    void HandleVertical()
    {

    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        GUI.Label(new Rect(0, 0, 300, 24), "RunSpeed: " + velocity.x, style);
        GUI.Label(new Rect(0, 24, 300, 24), "IsGrounded: " + Grounded, style);
    }

    void CalculateRunSpeed(Vector3 movement)
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
        Grounded = Physics.CheckSphere(transform.position+Vector3.up*0.1f, 0.2f, 1, QueryTriggerInteraction.Ignore);

    }
    // Update is called once per frame
    void Update()
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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_animator.SetBool("isJumping", true);
            m_animator.SetBool("isFalling", false);

            JumpTime = 0;
            velocity.y = MaxJumpSpeed;
        }

        JumpTime += Time.deltaTime;


        if (velocity.y < 0 && !Grounded)
        {
            m_animator.SetBool("isJumping", false);
            m_animator.SetBool("isFalling", true);
        }
        // if(isGrounded)
        // {
        //     m_animator.SetBool("isJumping", false);
        //     m_animator.SetBool("isFalling", false);
        // }
        m_animator.SetBool("isGrounded", Grounded);


    }


    void FixedUpdate()
    {
        HandleFalling();
    }

    void HandleFalling()
    {
        velocity.y = velocity.y + Gravity * JumpTime;
        if(Grounded)
        {
            velocity.y = -2;
        }
        else
        {
            velocity.y = Mathf.Clamp(jumpSpeed, Gravity, -Gravity);
        }

    }



    void OnAnimatorMove()
    {
        Vector3 move = m_animator.deltaPosition;
        move += detalMove;
        if (!Grounded)
        {
            move = velocity * MaxSpeed * Time.deltaTime / 2;
        }
        move.y = velocity.y * Time.deltaTime;
        m_CharacterController.Move(move);
    }

}
