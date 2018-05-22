using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] Boss m_boss = null;
    [SerializeField] Bunny m_bunny = null;

    private void Update()
    {
        if (!m_boss.Alive && m_bunny.Alive)
        {
            m_bunny.invincible = true;
            TransitionManager.Instance.PlayTransition("MainMenu", 3.0f, false);
        }
    }
}
