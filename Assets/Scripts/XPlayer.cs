using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Cinemachine;
using UnityEngine;

public class XPlayer : MonoBehaviour
{
    CharacterController m_CharacterController;
    Camera m_cam;

    GameObject gunGameObject;
    [SerializeField]
    CinemachineFreeLook m_cinemachine;

    [SerializeField]
    float m_aimSpeed = 5;

    public float m_Speed = 5;
    Vector3 mousePostion;
    Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        // m_cam = transform.Find("CameraHover").GetComponent<Camera>();
        // mousePostion = Input.mousePosition;
        // gunGameObject = transform.Find("Weapon_02").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            m_cinemachine.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");

        }
        if (Input.GetMouseButtonUp(0))
        {
            m_cinemachine.m_XAxis.m_InputAxisValue = 0;
        }
        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement);

        if (movement != Vector3.zero)
        {
            // CollisionFlags flags = m_CharacterController.Move(movement * m_Speed * Time.deltaTime);
            transform.position = StickGround();
            m_animator.SetFloat("speed", m_Speed);
            float x = Camera.main.transform.forward.x;
            float z = Camera.main.transform.forward.z;
            if (_z < 0)
            {
                Vector3 backward = Quaternion.Euler(0, 180, 0) * Camera.main.transform.forward;
                backward.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, backward, Time.deltaTime * 10);

            }
            else if (_z > 0)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, forward, Time.deltaTime * 10);
            }
            if (_x < 0)
            {
                Vector3 left = Quaternion.Euler(0, 180, 0) * Camera.main.transform.right;
                left.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, left, Time.deltaTime * 10);
            }
            else if (_x > 0)
            {
                Vector3 right = Camera.main.transform.right;
                right.y = 0;
                transform.forward = Vector3.Lerp(transform.forward, right, Time.deltaTime * 10);

            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 dir = ray.direction;

            // transform.forward = new Vector3(x, 0, z);
        }
        else
        {
            m_animator.SetFloat("speed", 0);
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
