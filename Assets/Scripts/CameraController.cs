using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject m_target = null;
    [SerializeField] [Range(1.0f, 10.0f)] float m_stiffness = 3.0f;

    private void LateUpdate()
    {
        Vector3 newPosition = m_target.transform.position;
        newPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * m_stiffness);
    }

    public void DeathTransition()
    {

    }

    public void BossTransition()
    {

    }
}
