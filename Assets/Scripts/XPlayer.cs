using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Cinemachine;
using SingularityGroup.HotReload;
using UnityEngine;


public class PlayerData
{
    public float RotationSpeed = 10;
    public float MoveSpeed = 5;

}
public class XPlayer : MonoBehaviour
{
    CharacterController m_CharacterController;
    Animator m_animator;
    [Range(5, 15)]
    public float MaxSpeed = 5;
    public float RunSpeed = 0;
    public float RotationSpeed = 15;

    public float acceleration = 20;

    private float jumpHeight = 0;
    Vector3 velocity = Vector3.zero;

    float JumpTime = 0;
    float JumpSpeed = 0;
    [HideInInspector]
    public Vector3 detalMove = Vector3.zero;
    float Gravity
    {
        get
        {
            return -1;
        }
    }

    public bool isGrounded
    {
        get
        {
            float h = m_CharacterController.height;
            return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1 + 0.1f);

        }
    }
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
        GUI.Label(new Rect(0, 0, 300, 24), "RunSpeed: " + RunSpeed, style);
        GUI.Label(new Rect(0, 24, 300, 24), "IsGrounded: " + isGrounded, style);
    }

    void CalculateRunSpeed(Vector3 movement)
    {
        Debug.Log("RunSpeed: " + (movement.magnitude));
        float scale = 1;
        if (movement.magnitude <= 0)
        {
            scale = -1;
        }
        RunSpeed += acceleration * scale * Time.deltaTime;
        RunSpeed = Mathf.Clamp(RunSpeed, 0, MaxSpeed);
    }


    // Update is called once per frame
    void Update()
    {

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

        m_animator.SetFloat("speed", RunSpeed);
        if (Input.GetKeyUp(KeyCode.V))
        {
            m_animator.SetTrigger("turn180");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_animator.SetBool("isJumping", true);
            m_animator.SetBool("isFalling", false);

            JumpTime = 0;
            JumpSpeed = 5;
        }

        JumpTime += Time.deltaTime;


        if (velocity.y < 0)
        {
            m_animator.SetBool("isJumping", false);
            m_animator.SetBool("isFalling", true);
        }
        m_animator.SetBool("isGrounded", isGrounded);

    }


    void FixedUpdate()
    {
        HandleFalling();
    }

    void HandleFalling()
    {
        JumpSpeed = JumpSpeed + Gravity * JumpTime;
        velocity.y = Mathf.Clamp(JumpSpeed, Gravity * 2, -Gravity * 2) * Time.fixedDeltaTime;

    }



    void OnAnimatorMove()
    {
        Vector3 move = m_animator.deltaPosition;
        move += detalMove;
        if (!isGrounded)
        {
            move = velocity * MaxSpeed * Time.deltaTime / 2;
        }
        move.y = velocity.y;
        m_CharacterController.Move(move);
    }

}
