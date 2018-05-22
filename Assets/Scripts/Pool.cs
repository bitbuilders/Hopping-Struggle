using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] GameObject m_template = null;
    [SerializeField] [Range(1, 100)] int m_poolSize = 25;

    List<GameObject> m_pool;

    private void Start()
    {
        m_pool = new List<GameObject>();
        for (int i = 0; i < m_poolSize; ++i)
        {
            GameObject item = Instantiate(m_template, Vector3.zero, Quaternion.identity, transform);
            item.SetActive(false);
            m_pool.Add(item);
        }
    }

    public GameObject Get()
    {
        GameObject item = null;
        for (int i = 0; i < m_pool.Count; ++i)
        {
            if (!m_pool[i].activeSelf)
            {
                item = m_pool[i];
            }
        }

        return item;
    }
}
