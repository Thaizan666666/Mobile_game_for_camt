using UnityEngine;

public class Fling : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    [Header("Custom Pattern")]
    public bool[] movePattern = { true, false, true, false };  // true = ขึ้น, false = ลง

    private Vector3 upPosition;
    private Vector3 downPosition;
    private Vector3 targetPosition;
    private Vector3 initialObjectPosition;  // ✅ เก็บตำแหน่งเริ่มต้น
    private bool isMoving = false;
    private int currentIndex = 0;

    void Start()
    {
        if (targetObject != null)
        {
            initialObjectPosition = targetObject.position;  // ✅ บันทึกตำแหน่งเริ่มต้น
            downPosition = targetObject.position;
            upPosition = downPosition + Vector3.up * moveDistance;
            targetPosition = movePattern[currentIndex] ? upPosition : downPosition;
        }

        // ✅ Subscribe to Player Die Event
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerRespawn.AddListener(ResetFling);
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

            if (Vector3.Distance(targetObject.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                targetObject.position = targetPosition;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            bool shouldMoveUp = movePattern[currentIndex];
            Debug.Log($"{currentIndex}");
            targetPosition = shouldMoveUp ? upPosition : downPosition;
            isMoving = true;

            if (currentIndex < movePattern.Length - 1)
            {
                currentIndex = (currentIndex + 1);
            }
        }
    }

    // ✅ ฟังก์ชัน Reset
    public void ResetFling()
    {
        isMoving = false;
        currentIndex = 0;

        if (targetObject != null)
        {
            targetObject.position = initialObjectPosition;
        }

        Debug.Log("Fling Reset เรียบร้อย");
    }

    void OnDestroy()
    {
        // ✅ Unsubscribe เมื่อ Object ถูกทำลาย
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.RemoveListener(ResetFling);
        }
    }
}