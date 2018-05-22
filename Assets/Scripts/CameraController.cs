using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject m_target = null;
    [SerializeField] [Range(1.0f, 10.0f)] float m_stiffness = 3.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_yOffset = 0.0f;

    Camera m_camera;

    private void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 newPosition = m_target.transform.position + m_yOffset * Vector3.up;
        newPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * m_stiffness);
    }

    public void DeathTransition(float duration, float zoomSpeed)
    {
        ZoomCam(zoomSpeed, duration);
    }

    public void BossTransition(float duration, float zoomSpeed)
    {
        float speed = 360.0f / duration;

        StartCoroutine(SpinCam(speed, duration));
        StartCoroutine(ZoomCam(zoomSpeed, duration));
    }

    private IEnumerator SpinCam(float speed, float duration)
    {
        float delta = Time.deltaTime;
        for (float i = 0.0f; i < duration; i += Time.deltaTime)
        {
            Vector3 rotation = Vector3.forward * speed * Time.deltaTime;
            if (rotation.z > 1.0f) rotation.z = 1.0f;
            transform.Rotate(rotation);

            yield return null;
        }

        transform.rotation = Quaternion.identity;
    }

    private IEnumerator ZoomCam(float speed, float duration)
    {
        for (float i = 0.0f; i < duration; i += Time.deltaTime)
        {
            float size = speed * Time.deltaTime;
            m_camera.orthographicSize -= size;
            if (m_camera.orthographicSize < 2.0f)
            {
                m_camera.orthographicSize = 2.0f;
            }

            yield return null;
        }

        transform.rotation = Quaternion.identity;
    }
}
