using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Cinemachine;
using SingularityGroup.HotReload;
using Unity.VisualScripting.FullSerializer;
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

    private float acceleration = 10;

    private float jumpHeight = 0;

    public bool isGrounded
    {
        get
        {
            // Vector3 p1 = transform.position + m_CharacterController.center - m_CharacterController.height * Vector3.up / 2;
            // Vector3 p2 = p1 + Vector3.up * m_CharacterController.height;
            // return Physics.CapsuleCast(p1, p2, m_CharacterController.radius, Vector3.down, 0.1f);
            float h = m_CharacterController.height;
            return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1 + 0.1f);
            // return m_CharacterController.isGrounded;
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
        float scale = 1;
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            scale = (1 - movement.magnitude);
            RunSpeed -= acceleration * ((RunSpeed + 0.1f)) * Time.deltaTime * scale;

        }
        else
        {
            scale = movement.magnitude * 1;
            RunSpeed += acceleration * (MaxSpeed / (RunSpeed + 0.1f)) * Time.deltaTime * scale;
        }
        RunSpeed = Mathf.Clamp(RunSpeed, 0, MaxSpeed);
    }
    Vector3 velocity = Vector3.zero;

    float JumpTime = 0;
    float JumpSpeed = 0;

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        velocity = new Vector3(horizontal, 0, vertical);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        Vector3 right = Camera.main.transform.right;
        right.y = 0;

        if (velocity != Vector3.zero)
        {
            forward = Quaternion.LookRotation(Vector3.forward * vertical + Vector3.right * horizontal) * forward;
            velocity = forward;
            transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * RotationSpeed);
        }
        if (Vector3.Angle(transform.forward, forward) > 179)
        {

        }

        CalculateRunSpeed(velocity);
        velocity.y += Gravity * Time.deltaTime;
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
            JumpSpeed = 3;
            // movement.y -= 2 * Gravity * Time.deltaTime;
        }


        JumpSpeed = JumpSpeed + Gravity * JumpTime * 2;
        velocity.y = JumpSpeed * Time.deltaTime;
        JumpTime += Time.deltaTime * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, Gravity, -Gravity);
        if (velocity.y < 0)
        {
            m_animator.SetBool("isJumping", false);
            m_animator.SetBool("isFalling", true);
        }
        m_animator.SetBool("isGrounded", isGrounded);

    }

    Vector3 StickGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100))
        {
            return hit.point;
        }
        return transform.position;
    }


    void FixedUpdate()
    {
        HandleFalling();
    }

    void HandleFalling()
    {
        // if (isGrounded)
        // m_CharacterController.Move(Vector3.down * 2);
        if (isGrounded)
        {
            // transform.position -= Vector3.down * Gravity * Time.deltaTime;
        }
    }

    float Gravity
    {
        get
        {
            return Physics.gravity.y;
        }
    }

    void OnAnimatorMove()
    {
        Vector3 move = m_animator.deltaPosition;
        if (!isGrounded)
        {
            move = velocity * MaxSpeed * Time.deltaTime / 2;
        }
        move.y = velocity.y;
        m_CharacterController.Move(move);


    }

}
