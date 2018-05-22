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
    [SerializeField] AudioClip m_hurt = null;
    [SerializeField] AudioClip m_jump = null;
    [SerializeField] Transform m_groundTouch = null;
    [SerializeField] LayerMask m_groundMask = 0;
    [SerializeField] string m_currentRoom = null;

    AudioSource m_audioSource;
    Rigidbody2D m_rigidbody;
    Animator m_animator;
    Vector3 m_startingScale;
    public Vector2 m_checkpoint;
    public bool invincible = false;
    
    public bool OnGround { get; private set; }
    public bool Alive { get { return Health > 0.0f; } }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
        m_startingScale = GetComponentsInChildren<Transform>()[1].localScale;
        Health = m_health;
        m_checkpoint = transform.position;
    }

    void Update()
    {
        OnGround = Physics2D.OverlapCircle(m_groundTouch.position, 0.15f, m_groundMask);
        m_animator.SetBool("OnGround", OnGround);

        if (Input.GetButtonDown("Jump") && OnGround)
        {
            m_rigidbody.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
            m_animator.SetTrigger("Jump");
            m_audioSource.clip = m_jump;
            m_audioSource.Play();
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
        if (collision.CompareTag("Snake") && !invincible)
        {
            Health -= collision.GetComponentInParent<Snake>().m_attackDamage;
            Vector2 direction = transform.position - collision.transform.position;
            float mult = direction.x < 0.0f ? -1.0f : 1.0f;
            Quaternion jumpAngle = Quaternion.AngleAxis(45.0f * mult, Vector3.forward);

            Vector2 force = jumpAngle * direction.normalized * 10.0f;
            m_rigidbody.AddForce(force, ForceMode2D.Impulse);

            m_audioSource.clip = m_hurt;
            m_audioSource.Play();
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
        TransitionManager.Instance.PlayTransition(m_currentRoom, 2.0f, false);
        yield return new WaitForSeconds(1.0f);
        transform.position = m_checkpoint;
    }
}
