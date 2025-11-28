using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform player;

    public Transform visionArea; // VisionArea objesi
    public Animator anim;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool goingRight = true;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        if (isChasing)
        {
            moveDirection = (player.position - transform.position).normalized;
            ChasePlayer(moveDirection);
        }
        else
        {
            moveDirection = goingRight ? Vector2.right : Vector2.left;
            Patrol();
        }

        // ------------------- VisionArea Yönünü Manuel Güncelle -------------------
        if (visionArea != null)
        {
            // FlipX ile değil rotation ile yön veriyoruz
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            visionArea.localRotation = Quaternion.Euler(0, 0, angle);
        }

        // Animator
        if (anim != null)
            anim.SetBool("isChasing", isChasing);
    }

    void Patrol()
    {
        float speed = patrolSpeed;

        if (goingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

            if (transform.position.x >= rightPoint.position.x)
                goingRight = false;

            sr.flipX = false;
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);

            if (transform.position.x <= leftPoint.position.x)
                goingRight = true;

            sr.flipX = true;
        }
    }

    void ChasePlayer(Vector2 direction)
    {
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);

        sr.flipX = direction.x >= 0 ? false : true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
            Debug.Log("Guard seni gördü, koşuyor!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("YAKALANDIN!");
        }
    }
}
