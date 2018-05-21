using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : Entity
{
    [SerializeField] [Range(1.0f, 1000.0f)] float m_speed = 5.0f;
    [SerializeField] [Range(0.0f, 100.0f)] float m_health = 100.0f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_jumpForce = 15.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_fallMultiplier = 3.0f;
    [SerializeField] Transform m_groundTouch = null;
    [SerializeField] Transform m_checkpoint = null;
    [SerializeField] LayerMask m_groundMask = 0;

    Rigidbody2D m_rigidbody;
    Animator m_animator;
    Vector3 m_startingScale;
    
    public bool OnGround { get; private set; }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        m_startingScale = GetComponentsInChildren<Transform>()[1].localScale;
        Health = m_health;
    }

    void Update()
    {
        OnGround = Physics2D.OverlapCircle(m_groundTouch.position, 0.15f, m_groundMask);
        m_animator.SetBool("OnGround", OnGround);

        if (Input.GetButtonDown("Jump") && OnGround)
        {
            m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            m_animator.SetTrigger("Jump");
        }

        if (Health <= 0.0f)
        {
            Die();
        }
    }

    private void LateUpdate()
    {
        Vector2 velocity = Vector2.zero;
        velocity.x = Input.GetAxis("Horizontal");
        velocity *= m_speed * Time.deltaTime;

        m_rigidbody.AddForce(velocity, ForceMode2D.Force);

        if (m_rigidbody.velocity.y > 0.5f)
        {
            m_rigidbody.velocity += (Vector2.up * Physics2D.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y < -0.5f)
        {
            m_rigidbody.velocity += (Vector2.up * Physics2D.gravity.y) * (m_fallMultiplier - 1.0f) * Time.deltaTime;
        }

        if (m_rigidbody.velocity.x < 1.0f && velocity.x < 0.0f)
        {
            Flip(true);
        }
        else if (m_rigidbody.velocity.x > -1.0f && velocity.x > 0.0f)
        {
            Flip(false);
        }

        float speed = Mathf.Abs(velocity.x);
        m_animator.SetFloat("WalkSpeed", speed * 0.125f);
        m_animator.SetFloat("Walk", speed);
        m_animator.SetFloat("yVelocity", m_rigidbody.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Snake"))
        {
            Health -= collision.GetComponentInParent<Snake>().m_attackDamage;
            Vector2 direction = transform.position - collision.transform.position;
            float mult = direction.x < 0.0f ? -1.0f : 1.0f;
            Quaternion jumpAngle = Quaternion.AngleAxis(45.0f * mult, Vector3.forward);

            Vector2 force = jumpAngle * direction.normalized * 10.0f;
            m_rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void Flip(bool left)
    {
        Transform child = GetComponentsInChildren<Transform>()[1];
        if (left)
        {
            child.localScale = new Vector3(-m_startingScale.x, m_startingScale.y, 1.0f);
        }
        else
        {
            child.localScale = new Vector3(m_startingScale.x, m_startingScale.y, 1.0f);
        }
    }

    private void Die()
    {
        Health = 100.0f;
        Respawn();
    }

    private void Respawn()
    {
        StartCoroutine(DelayedRespawn());
    }

    private IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(1.0f);
        transform.position = m_checkpoint.position;
    }
}
