using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] float m_speed = 10.0f;
    [SerializeField] [Range(0.0f, 100.0f)] float m_attackRate = 5.0f;
    [SerializeField] [Range(0.0f, 100.0f)] float m_difficultyIncreaseRate = 1.1f;
    [SerializeField] [Range(0.0f, 25.0f)] float m_amplitude = 5.0f;
    [SerializeField] [Range(1.0f, 25.0f)] float m_rateHoriz = 1.0f;
    [SerializeField] [Range(1.0f, 25.0f)] float m_rateVert = 1.5f;
    [SerializeField] [Range(1, 10)] int m_health = 3;
    [SerializeField] GameObject m_target = null;
    [SerializeField] Transform m_min = null;
    [SerializeField] Transform m_max = null;
    [SerializeField] List<GameObject> m_links = null;

    Vector2 m_actualPosition;
    Vector2 m_destination;
    Vector2 m_offset;
    float m_difficulty;
    float m_attackTime;

    public bool Alive { get { return m_health > 0; } }

    private void Start()
    {
        m_difficulty = 1.0f;
        m_attackTime = 0.0f;
        
        for (int i = 0; i < m_links.Count; ++i)
        {
            m_links[i].GetComponent<BossLink>().boss = this;
        }
        SetLinkActive();
    }

    private void Update()
    {
        m_attackTime += m_difficulty * Time.deltaTime;
        if (m_attackTime >= m_attackRate)
        {
            m_difficulty += m_difficultyIncreaseRate * Time.deltaTime;
            m_attackTime = 0.0f;
            SpawnSnake();
        }

        float x = Mathf.Sin(Time.time * m_rateHoriz) * m_amplitude;
        float y = Mathf.Sin(Time.time * m_rateVert) * m_amplitude;
        m_offset = new Vector3(x, y);

        Vector2 direction = m_destination - m_actualPosition;
        if (direction.magnitude < 0.1f)
        {
            PickRandomDestination();
        }
        else
        {
            Vector2 velocity = direction.normalized * m_speed;
            velocity *= Time.deltaTime;
            m_actualPosition += velocity;

            transform.position = m_actualPosition + m_offset;
        }
    }

    private void PickRandomDestination()
    {
        Vector2 min = m_target.transform.position + (Vector3)(Vector2.left * 2.0f);
        Vector2 max = m_target.transform.position + (Vector3)(Vector2.right * 2.0f);
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);

        if (x < m_min.position.x) x = m_min.position.x;
        else if (x > m_max.position.x) x = m_max.position.x;
        if (y < m_min.position.y) y = m_min.position.y;
        if (y > m_max.position.y) y = m_max.position.y;

        m_destination = new Vector2(x, y);
    }

    private void SpawnSnake()
    {
        GameObject obj = SnakePool.Instance.Get();
        obj.SetActive(true);
        obj.transform.position = transform.position;
        Snake snake = obj.GetComponent<Snake>();
        snake.m_target = m_target;
        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector2 force = direction * Random.Range(1.0f, 10.0f);
        snake.Jump(force);
    }

    public void TakeDamage()
    {
        m_health--;
        SetLinkActive();
    }

    private void SetLinkActive()
    {
        int x = Random.Range(0, m_links.Count);
        while (m_links[x].activeSelf)
        {
            x = Random.Range(0, m_links.Count);
        }

        m_links[x].SetActive(true);
    }
}
