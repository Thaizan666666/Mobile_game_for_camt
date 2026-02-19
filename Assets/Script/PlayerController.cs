using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;

    private float moveInput;
    private bool facingRight = true;
    private PlayerState currentState = PlayerState.Idle;

    Animator animator;
    Rigidbody2D rb;
    CheackOverLap cheack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cheack = GetComponentInChildren<CheackOverLap>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);

        if (transform.position.y <= -10f)
            RestartGame();

        UpdateState();
        HandleAnimationState();
    }

    // ========== State Management ==========

    void UpdateState()
    {
        if (!cheack.Canjump)
        {
            // ถ้าความเร็วขึ้น = Jump, ถ้าความเร็วลง = Fall
            if (rb.linearVelocity.y > 0.1f)
                currentState = PlayerState.Jump;
            else
                currentState = PlayerState.Fall; // ✅ เปลี่ยนเป็น Fall ทันทีที่เริ่มตก
        }
        else if (moveInput != 0)
        {
            currentState = PlayerState.Run;
        }
        else
        {
            currentState = PlayerState.Idle;
        }
    }

    void HandleAnimationState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("isMove", false);
                animator.SetBool("isJump", false);
                animator.SetBool("CanJump", true);
                break;

            case PlayerState.Run:
                animator.SetBool("isMove", true);
                animator.SetBool("isJump", false);
                animator.SetBool("CanJump", true);
                break;

            case PlayerState.Jump:
                animator.SetBool("isMove", false);
                animator.SetBool("isJump", true);
                animator.SetBool("CanJump", false);
                break;

            case PlayerState.Fall:
                animator.SetBool("isMove", false);
                animator.SetBool("isJump", false);
                animator.SetBool("CanJump", false);
                break;
        }
    }

    // ========== Input / UI ==========

    public void Move(float direction)
    {
        moveInput = direction;

        if (direction < 0 && facingRight)
            Flip();
        else if (direction > 0 && !facingRight)
            Flip();
    }

    public void StopMoving()
    {
        moveInput = 0;
    }

    public void Jump()
    {
        if (cheack.Canjump && rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            currentState = PlayerState.Jump;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            RestartGame();
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }
}