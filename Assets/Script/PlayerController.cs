using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private float moveInput;
    private bool facingRight = true;
    private PlayerState currentState = PlayerState.Idle;
    private Vector3 startPosition;
    private bool isDead = false;  // ✅ เพิ่มตัวแปรเช็คว่าตายแล้วหรือยัง

    Animator animator;
    Rigidbody2D rb;
    CheackOverLap cheack;
    SpriteRenderer spriteRenderer;  // ✅ เพิ่มเพื่อซ่อน Sprite

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cheack = GetComponentInChildren<CheackOverLap>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // ✅ เพิ่ม
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return;  // ✅ ถ้าตายแล้วไม่ให้ควบคุมได้
        HandleKeyboardInput();
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);

        if (transform.position.y <= -10f)
            Die();

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

    public void Move(float direction)
    {
        if (isDead) return;  // ✅ ถ้าตายแล้วไม่ให้เคลื่อนที่

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
        if (isDead) return;  // ✅ ถ้าตายแล้วไม่ให้กระโดด

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
        if (isDead) return;  // ✅ ป้องกันเรียก Die() ซ้ำ
        isDead = true;

        // ✅ ซ่อน Player Sprite
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // ✅ หยุดการเคลื่อนที่
        rb.linearVelocity = Vector2.zero;
        moveInput = 0;

        // ✅ เก็บตำแหน่งที่ตาย
        Vector3 deathPosition = transform.position;

        // ✅ เรียก GameManager พร้อมส่งตำแหน่ง
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied(deathPosition);
        }
        SoundManager.PlayDeath();
        // ✅ Reset Objects
        foreach (var obj in FindObjectsByType<Respawnable>(FindObjectsSortMode.None))
            obj.ResetObject();

        // ✅ Respawn Player (รอ Coroutine)
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(GameManager.Instance.deathDelayTime);

        // ย้าย Player ไป Checkpoint
        Vector3 respawn = CheckpointManager.Instance.GetRespawnPoint(startPosition);
        transform.position = respawn;

        // Reset state
        isDead = false;
        currentState = PlayerState.Idle;

        // แสดง Sprite อีกครั้ง
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

    void HandleKeyboardInput()
    {
        // รับค่าจาก A/D หรือ Arrow Keys
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D = -1/1, Arrow Left/Right = -1/1

        if (horizontalInput != 0)
        {
            Move(horizontalInput);
        }
        else
        {
            StopMoving();
        }

        // กระโดดด้วย Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
}