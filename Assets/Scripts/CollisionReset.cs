using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReset : MonoBehaviour
{
    CircleCollider2D m_collider;
    float m_disableTime;

    private void Start()
    {
        m_collider = GetComponent<CircleCollider2D>();
        m_disableTime = 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bunny"))
        {
            m_collider.enabled = false;
            StartCoroutine(DisableCollider(m_disableTime));
        }
    }

    IEnumerator DisableCollider(float time)
    {
        yield return new WaitForSeconds(time);
        m_collider.enabled = true;
    }
}
