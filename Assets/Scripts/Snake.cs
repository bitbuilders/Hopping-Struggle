using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 500.0f)] float m_headSpeed = 100.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_bobForce = 2.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_bobRate = 0.1f;
    [SerializeField] [Range(1.0f, 500.0f)] float m_jumpForce = 7.0f;
    [SerializeField] List<Transform> m_points = null;

    LineRenderer m_lineRenderer;
    float m_bobTime = 0.0f;
    int m_bobIndex = 0;

    void Start()
    {
        m_lineRenderer = GetComponentInChildren<LineRenderer>();
        ConnectPoints();
    }

    private void Update()
    {
        m_bobTime += Time.deltaTime;
        if (m_bobTime >= m_bobRate)
        {
            m_points[m_bobIndex].GetComponent<Rigidbody2D>().AddForce(Vector3.up * m_bobForce, ForceMode2D.Impulse);

            m_bobIndex += 1;
            if (m_bobIndex >= m_points.Count)
            {
                m_bobIndex = 0;
            }

            m_bobTime = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = new Vector2(-m_headSpeed, 0.0f);

        for (int i = 0; i < m_points.Count; ++i)
        {
            m_points[i].GetComponent<Rigidbody2D>().AddForce(velocity * Time.deltaTime, ForceMode2D.Force);
            velocity *= 0.90f;
        }
    }

    void LateUpdate()
    {
        ConnectPoints();
    }

    private void ConnectPoints()
    {
        m_lineRenderer.positionCount = m_points.Count;
        for (int i = 0; i < m_points.Count; ++i)
        {
            m_lineRenderer.SetPosition(i, m_points[i].position);
        }
    }
}
