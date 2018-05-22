using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] string m_bossScene = null;
    [SerializeField] string m_tag = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(m_tag))
        {
            TransitionManager.Instance.PlayTransition(m_bossScene, 3.0f, true);
        }
    }
}
