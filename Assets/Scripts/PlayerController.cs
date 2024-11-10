using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;
    private Rigidbody2D rb;

    private bool facingRight;
    private bool lockLunge = false;
    public float dashSpeed;

    public Transform hitbox;
    public Transform secret;

    public Transform feetPos;
    public float checkRadius;
    private bool isGrounded;
    public LayerMask WhatIsGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * moveInput, rb.velocity.y);

        if (moveInput > 0 && facingRight == true)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight == false)
        {
            Flip();
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, WhatIsGround);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
       if (Input.GetKey(KeyCode.Q) && !lockLunge && facingRight)
        {
            lockLunge = true;
            Invoke("lungeLock", 3.15f);
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.left * dashSpeed);
        }
       else if (Input.GetKey(KeyCode.Q) && !lockLunge && !facingRight)
        {
            lockLunge = true;
            Invoke("lungeLock", 2f);
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(Vector2.right * dashSpeed);
        }
       if (Input.GetKey(KeyCode.Equals))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       else if (Input.GetKey(KeyCode.Minus))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            Debug.Log(collision.gameObject.name);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
        if (collision.gameObject.tag == "Secret")
        {
            Destroy(hitbox.gameObject);
        }
    }
    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void lungeLock()
    {
        lockLunge = false;
    }
}
