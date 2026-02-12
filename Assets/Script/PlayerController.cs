using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private float moveInput;
    private bool IsJump = false;

    void Update()
    {
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);

        if(transform.position.y <= -10f)
        {
            RestartGame();
        }
    }

    // ฟังก์ชันสำหรับกดปุ่มค้างไว้ (เรียกใช้จาก UI Event Trigger)
    public void Move(float direction)
    {
        moveInput = direction;
    }

    // ฟังก์ชันเมื่อปล่อยปุ่ม
    public void StopMoving()
    {
        moveInput = 0;
    }

    // ฟังก์ชันสำหรับปุ่มกระโดด
    public float jumpForce = 10f; // ปรับความสูงของการกระโดดตรงนี้

    public void Jump()
    {
        // เรียกใช้ Rigidbody2D ของตัวละครเพื่อใส่แรงส่งขึ้นไปข้างบน
        if (IsJump)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // ใส่แรงกระโดด (Velocity Change)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ถ้าสิ่งที่ชนมี Tag ชื่อว่า Obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            RestartGame();
        }
        if (collision.gameObject.CompareTag("Plantform"))
        {
            IsJump = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plantform"))
        {
            IsJump = false;
        }
    }

    void RestartGame()
    {
        // สั่งให้โหลดฉากปัจจุบันใหม่ (เริ่มเกมใหม่)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}