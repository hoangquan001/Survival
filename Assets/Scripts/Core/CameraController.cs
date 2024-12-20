using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SurvivalGame.Core
{


public class CameraController : MonoBehaviour
{

    [SerializeField]
    CinemachineFreeLook m_cinemachine;

    public float Speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            m_cinemachine.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X")* Speed;
            m_cinemachine.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y") * Speed;
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_cinemachine.m_XAxis.m_InputAxisValue = 0;
            m_cinemachine.m_YAxis.m_InputAxisValue = 0;

        }
    }
}
}