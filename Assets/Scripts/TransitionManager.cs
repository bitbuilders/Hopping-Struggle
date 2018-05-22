using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : Singleton<TransitionManager>
{
    [SerializeField] Image m_tint = null;

    CameraController m_camera;

    private void Start()
    {
        m_camera = FindObjectOfType<CameraController>();
    }

    public void PlayTransition(string sceneTo, float duration, bool boss)
    {
        StartCoroutine(Transition(sceneTo, duration, boss));
    }

    private IEnumerator Transition(string scene, float time, bool boss)
    {
        if (boss) m_camera.BossTransition(time, 1.0f);
        else m_camera.DeathTransition(time, 2.0f);

        m_tint.gameObject.SetActive(true);
        for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / time)
        {
            Color c = m_tint.color;
            c.a = i;
            m_tint.color = c;

            yield return null;
        }

        Game.Instance.LoadScene(scene);
        Color color = m_tint.color;
        color.a = 1.0f;
        m_tint.color = color;
    }
}
