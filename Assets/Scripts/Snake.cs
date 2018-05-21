using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] float m_detectionRange = 10.0f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_attackRange = 5.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_attackRate = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] public float m_flipRate = 3.0f;
    [SerializeField] [Range(1.0f, 500.0f)] public float m_headSpeed = 100.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_bobForce = 2.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_bobRate = 0.1f;
    [SerializeField] [Range(1.0f, 500.0f)] public float m_jumpForce = 7.0f;
    [SerializeField] LayerMask m_groundMask = 0;
    [SerializeField] List<Transform> m_points = null;

    public StateMachine<Snake> m_stateMachine;
    LineRenderer m_lineRenderer;
    public GameObject m_target;
    float m_bobTime = 0.0f;
    public float m_flipTime = 5.0f;
    float m_attackTime = 0.0f;
    int m_bobIndex = 0;

    public bool OnGround { get; private set; }

    void Start()
    {
        m_lineRenderer = GetComponentInChildren<LineRenderer>();
        m_stateMachine = new StateMachine<Snake>();
        m_stateMachine.AddState(new AttackState(this), "Attack");
        m_stateMachine.AddState(new WanderState(this), "Wander");
        m_stateMachine.AddState(new SeekState(this), "Seek");
        m_stateMachine.PushState("Wander");
        m_flipTime = 5.0f;
        ConnectPoints();
    }

    private void Update()
    {
        OnGround = Physics2D.OverlapCircle(m_points[0].position, 0.3f, m_groundMask);
        m_attackTime += Time.deltaTime;
        m_flipTime += Time.deltaTime;
        m_stateMachine.Update();

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
        m_stateMachine.FixedUpdate();
    }

    void LateUpdate()
    {
        m_stateMachine.LateUpdate();

        ConnectPoints();
    }

    public void Move(Vector2 speed)
    {
        Vector2 velocity = speed;
        for (int i = 0; i < m_points.Count; ++i)
        {
            m_points[i].GetComponent<Rigidbody2D>().AddForce(velocity * Time.deltaTime, ForceMode2D.Force);
            velocity *= 0.90f;
        }
    }

    public void Jump(Vector2 force)
    {
        Vector2 velocity = force;
        for (int i = 0; i < m_points.Count; ++i)
        {
            m_points[i].GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            velocity *= 0.90f;
        }
    }

    public void Flip(Vector2 direction)
    {
        for (int i = 0; i < m_points.Count / 2; ++i)
        {
            Transform temp = m_points[i];
            m_points[i] = m_points[m_points.Count - 1 - i];
            m_points[m_points.Count - 1 - i] = temp;
        }
    }

    public bool IsReversed()
    {
        Vector2 head = m_points[0].position;
        Vector2 tail = m_points[m_points.Count - 1].position;

        float distanceFromHead = (m_target.transform.position - (Vector3)head).magnitude;
        float distanceFromTail = (m_target.transform.position - (Vector3)tail).magnitude;

        if (distanceFromTail < distanceFromHead)
        {
            return true;
        }

        return false;
    }

    public Vector2 Position()
    {
        return m_points[0].position;
    }

    private void ConnectPoints()
    {
        m_lineRenderer.positionCount = m_points.Count;
        for (int i = 0; i < m_points.Count; ++i)
        {
            m_lineRenderer.SetPosition(i, m_points[i].position);
        }
    }

    public void UpdateState()
    {
        Vector2 distance = m_target.transform.position - (Vector3)Position();
        m_stateMachine.ClearStates();
        if (distance.magnitude <= m_attackRange && m_attackTime >= m_attackRate)
        {
            m_attackTime = 0.0f;
            m_stateMachine.PushState("Attack");
        }
        else if (distance.magnitude <= m_detectionRange)
        {
            m_stateMachine.PushState("Seek");
        }
        else
        {
            m_stateMachine.PushState("Wander");
        }
    }
}

public class AttackState : State<Snake>
{
    public AttackState(Snake owner) : base(owner)
    {

    }

    public override void Enter(bool fixedUpdate)
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        //m_owner.UpdateState();
    }

    public override void Update()
    {
        Vector2 targetPosition = m_owner.m_target.transform.position;
        Vector2 position = m_owner.Position();
        Vector2 direction = new Vector2(targetPosition.x - position.x, 0.0f);
        float mult = direction.x < 0.0f ? -1.0f : 1.0f;

        Quaternion jumpAngle = Quaternion.AngleAxis(45.0f * mult, Vector3.forward);
        Vector2 force = (jumpAngle * direction.normalized) * m_owner.m_jumpForce;
        m_owner.Jump(force);

        m_owner.UpdateState();
    }
}

public class WanderState : State<Snake>
{
    public WanderState(Snake owner) : base(owner)
    {

    }

    public override void Enter(bool fixedUpdate)
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        m_owner.UpdateState();
    }

    public override void Update()
    {
        //m_owner.UpdateState();
    }
}

public class SeekState : State<Snake>
{
    public SeekState(Snake owner) : base(owner)
    {

    }

    public override void Enter(bool fixedUpdate)
    {

    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        Vector2 targetPosition = m_owner.m_target.transform.position;
        Vector2 position = m_owner.Position();
        Vector2 direction = new Vector2(targetPosition.x - position.x, 0.0f);
        m_owner.Move(direction.normalized * m_owner.m_headSpeed);
        if (m_owner.IsReversed() && m_owner.m_flipTime >= m_owner.m_flipRate)
        {
            m_owner.m_flipTime = 0.0f;
            m_owner.Flip(direction.normalized);
        }

        m_owner.UpdateState();
    }

    public override void Update()
    {
        m_owner.UpdateState();
    }
}