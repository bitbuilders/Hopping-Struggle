using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 25.0f)] float m_amplitude = 5.0f;
    [SerializeField] [Range(1.0f, 25.0f)] float m_rateHoriz = 1.0f;
    [SerializeField] [Range(1.0f, 25.0f)] float m_rateVert = 1.5f;

    Vector3 m_position;
    Vector3 m_offset;

    private void Start()
    {
        m_position = transform.position;
    }

    private void Update()
    {
        float x = Mathf.Sin(Time.time * m_rateHoriz) * m_amplitude;
        float y = Mathf.Sin(Time.time * m_rateVert) * m_amplitude;
        m_offset = new Vector3(x, y);

        transform.position = m_position + m_offset;
    }
}
