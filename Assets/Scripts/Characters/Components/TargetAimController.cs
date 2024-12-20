using UnityEngine;

public class TargetAimController : MonoBehaviour
{

    void Update()
    {

        Vector3 screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            transform.position = hit.point;
        }
    }
}