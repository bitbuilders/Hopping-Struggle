using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVScroll : MonoBehaviour
{
    public enum ScrollDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    
    [SerializeField] ScrollDirection m_direction = ScrollDirection.RIGHT;
    [SerializeField] [Range(1.0f, 150.0f)] float m_distanceReset = 35.0f;
    [SerializeField] GameObject m_target = null;
    [SerializeField] Transform m_min = null;
    [SerializeField] Transform m_max = null;
    [SerializeField] List<Transform> m_trees = null;

    List<Vector2> m_treeStartPositions;
    Material m_material;
    Vector2 m_offset;
    Vector2 m_startPosition;

    private void Start()
    {
        m_material = GetComponent<Renderer>().material;
        m_offset = Vector2.zero;
        m_startPosition = m_target.transform.position;
        m_treeStartPositions = new List<Vector2>();
        foreach (Transform t in m_trees)
        {
            m_treeStartPositions.Add(t.position);
        }
    }

    private void Update()
    {
        Vector2 velocity = Vector2.zero;

        switch (m_direction)
        {
            case ScrollDirection.UP:
                velocity = Vector2.up;
                break;
            case ScrollDirection.DOWN:
                velocity = Vector2.down;
                break;
            case ScrollDirection.LEFT:
                velocity = Vector2.left;
                break;
            case ScrollDirection.RIGHT:
                velocity = Vector2.right;
                break;
        }

        float position = ((m_target.transform.position.x - m_startPosition.x) % m_distanceReset) / m_distanceReset;
        velocity *= position;
        m_offset = velocity;
        m_material.SetTextureOffset("_MainTex", m_offset);

        for (int i = 0; i < m_trees.Count; ++i)
        {
            Vector2 range = (m_max.position - m_min.position);
            Vector2 pos = ((Vector3)m_startPosition - m_target.transform.position) * 0.35f + (Vector3)m_treeStartPositions[i];
            if (pos.x >= m_max.position.x)
            {
                pos.x = m_min.position.x;
                m_treeStartPositions[i] -= Vector2.right * range.x;
            }
            else if (pos.x <= m_min.position.x)
            {
                pos.x = m_max.position.x;
                m_treeStartPositions[i] += Vector2.right * range.x;
            }

            m_trees[i].position = new Vector3(pos.x, m_trees[i].position.y);
        }
    }
}
