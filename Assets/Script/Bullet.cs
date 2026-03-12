using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float lifetime;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0;

        gameObject.tag = "Obstacle";
    }

    public void Initialize(Vector2 dir, float spd, float life)
    {
        direction = dir;
        speed = spd;
        lifetime = life;

        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        // ทำลายตัวเองหลังจากเวลาที่กำหนด
        Destroy(gameObject, lifetime);
    }
}