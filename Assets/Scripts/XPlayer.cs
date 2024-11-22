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

    public float MoveSpeed = 5;
    public float RotationSpeed = 15;
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement != Vector3.zero)
        {
            m_animator.SetFloat("speed", MoveSpeed);
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            Vector3 right= Camera.main.transform.right;
            right.y = 0;

            movement = right * horizontal + forward * vertical;
            m_CharacterController.Move(movement * MoveSpeed * Time.deltaTime);
            transform.position = StickGround();


            forward = Quaternion.LookRotation(Vector3.forward * vertical + Vector3.right * horizontal) * forward;
            transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * RotationSpeed);
        }
        else
        {
            m_animator.SetFloat("speed", 0);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            m_animator.SetTrigger("jump");
        }
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


    }

}
