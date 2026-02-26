using UnityEngine;

public class Moveto : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;  // The object that will move up
    public float moveSpeed = 2f;    // Speed of upward movement
    public float moveDistance = 3f; // How far up to move

    private Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 initialObjectPosition;  // ✅ เก็บตำแหน่งเริ่มต้น
    private bool isMoving = false;
    private bool hasTriggered = false;  // ✅ เช็คว่า Trigger แล้วหรือยัง

    void Start()
    {
        if (targetObject != null)
        {
            initialObjectPosition = targetObject.position;  // ✅ บันทึกตำแหน่งเริ่มต้น
            startPosition = targetObject.position;
            targetPosition = startPosition + Vector3.right * moveDistance;
        }

        // ✅ Subscribe to Player Die Event
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.AddListener(ResetMoveto);
        }
    }

    void Update()
    {
        if (isMoving && targetObject != null)
        {
            targetObject.position = Vector3.MoveTowards(
                targetObject.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Stop when reached
            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                hasTriggered = true;  // ✅ เคลื่อนที่เสร็จแล้ว
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)  // ✅ เช็คว่ายัง Trigger ครั้งแรก
        {
            isMoving = true;
        }
    }

    // ✅ ฟังก์ชัน Reset
    public void ResetMoveto()
    {
        isMoving = false;
        hasTriggered = false;  // ✅ Reset เพื่อให้ Trigger ได้อีก

        if (targetObject != null)
        {
            targetObject.position = initialObjectPosition;
        }

        Debug.Log("Moveto Reset เรียบร้อย");
    }

    void OnDestroy()
    {
        // ✅ Unsubscribe เมื่อ Object ถูกทำลาย
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.RemoveListener(ResetMoveto);
        }
    }
}