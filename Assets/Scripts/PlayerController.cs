using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private Rigidbody2D rb; 
    private SpriteRenderer sr;
    private Animator anim;
    float moveX;
    bool isGrounded;
    public bool isHidden = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (isHidden) 
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0);
            return;
        }
        moveX = Input.GetAxisRaw("Horizontal");
        if (moveX > 0)
        {
            sr.flipX = false;
        }
        else if (moveX < 0)
        {
            sr.flipX = true;
        }
        anim.SetFloat("Speed", Mathf.Abs(moveX));
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetBool("isJumping", true);
        }
    }
    void FixedUpdate()
    {
        if (isHidden) return;
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
    }
}
