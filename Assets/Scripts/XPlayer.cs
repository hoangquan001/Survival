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
    [Range(5,15)]
    public float MaxSpeed = 5;
    public float RunSpeed = 0;
    public float RotationSpeed = 15;

    private float acceleration = 10;

    public bool isGrounded
    {
        get
        {
            return m_CharacterController.isGrounded;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    void HandleVertical()
    {
        
    }

    void CalculateRunSpeed(Vector3 movement)
    {
        float scale = 1;
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            scale = (1 - movement.magnitude);
            RunSpeed -= acceleration * ( (RunSpeed + 0.1f)) * Time.deltaTime * scale;

        }
        else
        {
            scale = movement.magnitude * 1;
            RunSpeed += acceleration * (MaxSpeed / (RunSpeed + 0.1f)) * Time.deltaTime * scale;
        }
        RunSpeed = Mathf.Clamp(RunSpeed, 0, MaxSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        Vector3 right= Camera.main.transform.right;
        right.y = 0;
        
        if (movement != Vector3.zero)
        {
            forward = Quaternion.LookRotation(Vector3.forward * vertical + Vector3.right * horizontal) * forward;
            transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * RotationSpeed);
        }
        if(Vector3.Angle(transform.forward, forward) > 179)
        {
            
        }
            
        CalculateRunSpeed(movement);
        m_animator.SetFloat("speed", RunSpeed);

        if(Input.GetKeyUp(KeyCode.Space))
        {
            m_animator.SetTrigger("turn180");

            m_animator.SetTrigger("jump");
        }
        // if (m_CharacterController.isGrounded)
        // {
        //     m_animator.SetBool("grounded", true);
        // }
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
        if (!isGrounded)
        {

            m_CharacterController.Move(Vector3.down * -Gravity * Time.deltaTime);
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
        Vector3 movement = m_animator.deltaPosition;
        m_CharacterController.Move(movement);
        
        
    }

}
