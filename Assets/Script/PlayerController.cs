using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private float keyboardInput;  // ✅ Input จาก Keyboard
    private float uiInput;        // ✅ Input จาก UI
    private float moveInput;      // ✅ Input รวม
    private bool facingRight = true;
    private PlayerState currentState = PlayerState.Idle;
    private Vector3 startPosition;
    private bool isDead = false;

    Animator animator;
    Rigidbody2D rb;
    CheackOverLap cheack;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cheack = GetComponentInChildren<CheackOverLap>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return;

        // ✅ รับ Input จาก Keyboard
        keyboardInput = Input.GetAxisRaw("Horizontal");

        // ✅ รวม Input (UI มี Priority สูงกว่า)
        moveInput = uiInput != 0 ? uiInput : keyboardInput;

        // ✅ Flip ตัวละคร
        if (moveInput < 0 && facingRight)
            Flip();
        else if (moveInput > 0 && !facingRight)
            Flip();

        // ✅ เคลื่อนที่
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);

        if (transform.position.y <= -10f)
            Die();

        // ✅ กระโดดด้วย Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        UpdateState();
        HandleAnimationState();
    }

    void UpdateState()
    {
        if (!cheack.Canjump)
        {
            if (rb.linearVelocity.y > 0.1f)
                currentState = PlayerState.Jump;
            else
                currentState = PlayerState.Fall;
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
                animator.SetBool("isFall", false);
                animator.SetBool("CanJump", true);
                break;
            case PlayerState.Run:
                animator.SetBool("isMove", true);
                animator.SetBool("isFall", false);
                animator.SetBool("CanJump", true);
                break;
            case PlayerState.Jump:
                animator.SetBool("isMove", false);
                animator.SetBool("isFall", false);
                animator.SetBool("CanJump", false);
                break;
            case PlayerState.Fall:
                animator.SetBool("isMove", false);
                animator.SetBool("isFall", true);
                animator.SetBool("CanJump", false);
                break;
        }
    }

    // ========== UI Buttons ==========

    public void MoveLeft()
    {
        if (isDead) return;
        uiInput = -1f;
    }

    public void MoveRight()
    {
        if (isDead) return;
        uiInput = 1f;
    }

    public void StopMoving()
    {
        uiInput = 0f;
    }

    public void Jump()
    {
        if (isDead) return;

        if (cheack.Canjump && rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            currentState = PlayerState.Jump;
            SoundManager.PlayJump();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        rb.linearVelocity = Vector2.zero;
        moveInput = 0;
        uiInput = 0;
        keyboardInput = 0;

        Vector3 deathPosition = transform.position;

        SoundManager.PlayDeath();

        if (GameManager.Instance != null)
            GameManager.Instance.PlayerDied(deathPosition);
    }
    public void Respawn()
    {
        Vector3 respawn = startPosition;

        if (CheckpointManager.Instance != null)
            respawn = CheckpointManager.Instance.GetRespawnPoint(startPosition);

        transform.position = respawn;
        isDead = false;
        currentState = PlayerState.Idle;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        facingRight = !facingRight;
    }
}