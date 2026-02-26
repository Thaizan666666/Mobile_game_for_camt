using UnityEngine;

public class FlingTime : MonoBehaviour
{
    [Header("References")]
    public Transform targetObject;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    [Header("Times")]
    public float waitTime = 1f;

    [Header("Custom Pattern")]
    public bool[] movePattern = { true, false, true, false };

    [Header("Movement Direction")]
    public bool isHorizontal = false;
    public bool invertDirection = false;

    private Vector3 firstPosition;
    private Vector3 secondPosition;
    private Vector3 targetPosition;
    private Vector3 initialObjectPosition;  // ✅ เก็บตำแหน่งเริ่มต้น
    private bool isMoving = false;
    private int currentIndex = 0;
    private float timer = 0f;
    private bool isWaiting = false;
    private bool hasStarted = false;

    void Start()
    {
        if (targetObject != null)
        {
            initialObjectPosition = targetObject.position;  // ✅ เก็บตำแหน่งเริ่มต้น
            firstPosition = targetObject.position;

            if (isHorizontal)
            {
                if (invertDirection)
                    secondPosition = firstPosition + Vector3.left * moveDistance;
                else
                    secondPosition = firstPosition + Vector3.right * moveDistance;
            }
            else
            {
                if (invertDirection)
                    secondPosition = firstPosition + Vector3.down * moveDistance;
                else
                    secondPosition = firstPosition + Vector3.up * moveDistance;
            }
        }
        else
        {
            Debug.LogWarning("Target Object ยังไม่ได้กำหนด!");
        }

        // ✅ Subscribe to Player Die Event
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.AddListener(ResetFling);
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

                if (hasStarted && currentIndex < movePattern.Length)
                {
                    isWaiting = true;
                    timer = 0f;
                }
            }
        }

        if (isWaiting)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                isWaiting = false;
                MoveToNextPosition();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasStarted && !isMoving)
        {
            hasStarted = true;
            currentIndex = 0;
            MoveToNextPosition();
        }
    }

    void MoveToNextPosition()
    {
        if (currentIndex < movePattern.Length)
        {
            bool moveToSecond = movePattern[currentIndex];
            targetPosition = moveToSecond ? secondPosition : firstPosition;

            string direction = GetDirectionName(moveToSecond);
            Debug.Log($"รอบที่ {currentIndex + 1}: {direction}");

            isMoving = true;
            currentIndex++;
        }
        else
        {
            Debug.Log("จบ Pattern แล้ว");
        }
    }

    string GetDirectionName(bool moveToSecond)
    {
        if (isHorizontal)
        {
            if (invertDirection)
                return moveToSecond ? "ซ้าย" : "กลับ";
            else
                return moveToSecond ? "ขวา" : "กลับ";
        }
        else
        {
            if (invertDirection)
                return moveToSecond ? "ลง" : "กลับ";
            else
                return moveToSecond ? "ขึ้น" : "กลับ";
        }
    }

    // ✅ ฟังก์ชัน Reset
    public void ResetFling()
    {
        hasStarted = false;
        isMoving = false;
        isWaiting = false;
        currentIndex = 0;
        timer = 0f;

        if (targetObject != null)
        {
            targetObject.position = initialObjectPosition;
        }

        Debug.Log("Reset Fling เรียบร้อย");
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